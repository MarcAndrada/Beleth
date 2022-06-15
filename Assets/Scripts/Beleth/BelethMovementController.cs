using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using System;

public class BelethMovementController : MonoBehaviour
{
    
    #region Inputs Variables
    private Vector3 movementInput;
    private Vector3 movementDirection;
    private Vector2 recibedInputs;
    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction runAction;

    #endregion

    #region MovementVariables
    [Header("Movment")]
    [SerializeField]
    private Transform[] floorRayPlaces;
    [SerializeField]
    private Transform[] rampRayPlaces;
    [SerializeField]
    private bool canMove = true;
    [SerializeField]
    [Tooltip("Velocidad en la que el personaje gira a los lados")]
    private float floorRotationSpeed;
    [SerializeField]
    private float airRotationSpeed;
    [SerializeField]
    [Tooltip("Velocidad actual a la que va a acelerar el personaje")]
    private float currentAccel;
    [SerializeField]
    [Tooltip("Velocidad en la que el personaje acelera estando en el suelo")]
    private float walkSpeed;
    [SerializeField]
    [Tooltip("Velocidad en la que el personaje acelera estando en el suelo")]
    private float runSpeed;
    [SerializeField]
    [Tooltip("Velocidad en la que el personaje acelera estando en el aire")]
    private float airSpeed;
    [SerializeField]
    [Tooltip("Velocidad en la que el personaje acelera estando en el aire")]
    private float glidingSpeed;
    [SerializeField]
    public LayerMask walkableLayers;
    private bool running;
    [SerializeField]
    public bool onPlatform = false;
    private bool onRamp;
    #endregion

    #region Jump Variables

    [Header("Jump")]
    [SerializeField]
    [Tooltip("Altura que tendra el salto base estando en el suelo")]
    private float jumpHeight;
    [SerializeField]
    [Tooltip("Altura del doble salto")]
    private float doubleJumpHeight;
    [SerializeField]
    private float jumpImpulse;
    [SerializeField]
    public float maxFloorCheckDistance;
    [SerializeField]
    private float maxRampCheckDistance;
    [SerializeField]
    public bool groundedPlayer;
    [SerializeField]
    private float extraGravity;
    [SerializeField]
    private float glidingGravity;
    [SerializeField]
    [Tooltip("El tiempo que dispondra el player para saltar y no tener que hacer el salto pixel perfect")]
    private float coyoteTime;
    private bool gliding = false;
    private bool doubleJumped = false;
    private bool canCoyote;
    RaycastHit groundHit;



    #endregion

    #region Components Variables
    [Header("Components & External objects")]
    [SerializeField]
    private Camera followCamera;
    private CapsuleCollider coll;
    private Rigidbody rb;
    private PlayerInput playerInput;
    private BelethAnimController animController;
    private BelethCheckPointManager checkPointManager;
    private BelethAudioController audioController;
    private bool isAttacking = false;
    #endregion

    #region Shadow 
    [Header("Shadow GameObject")]
    [SerializeField] GameObject Shadow;
    [SerializeField] LayerMask floorLayer;
    [SerializeField] float maxShadowDistance;
    #endregion

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        coll = GetComponent<CapsuleCollider>();
        playerInput = GetComponent<PlayerInput>();
        animController = GetComponent<BelethAnimController>();
        checkPointManager = GetComponent<BelethCheckPointManager>();
        audioController = GetComponentInChildren<BelethAudioController>();

        //Movment Events
        moveAction = playerInput.actions["Move"];
        moveAction.started += SetMovmentValues;
        moveAction.performed += SetMovmentValues;
        moveAction.canceled += SetMovmentValues;

        // Jump Events
        jumpAction = playerInput.actions["Jump"];
        jumpAction.started += Jump;
        jumpAction.performed += StartGliding;
        jumpAction.canceled += _ => StopGlide();

        // Run Events
        runAction = playerInput.actions["Run"];
        runAction.started += _ => SetRunning();
        runAction.canceled += _ => SetRunning();

        
    }

    private void Start()
    {
        //Setear valor a las animaciones
        //animController.SetSpeedValue(currentSpeed);
        animController.SetGliding(gliding);

    }

    private void Update()
    {
        
        CheckIfGrounded();
        CheckAccelSpeed();
        CheckIfCanJump();
        CheckMovementInput();

        MoveShadow();
    }

    private void FixedUpdate()
    {

        //Comprobar si esta en el suelo y se le debe quitar la grabedad (para las rampas)
        if (movementInput != Vector3.zero  && onRamp || !onRamp) 
        {
            ApplyGravity();
        }

        //rb.AddForce(transform.forward * axis.y * 200,ForceMode.Acceleration);

        if (canMove && !CinematicsController._CINEMATICS_CONTROLLER.isPlayingCinematic)
        {
            RotatePlayer();
            MovePlayer();
        }


        CheckRampMovement();
        

    }

    #region Actions
    private void MovePlayer()
    {
        if (movementInput != Vector3.zero)
        {
            rb.AddForce(movementDirection * currentAccel, ForceMode.VelocityChange);
//            rb.MovePosition(rb.velocity);

        }
        else
        {
            rb.AddForce(new Vector3(-rb.velocity.x, 0, -rb.velocity.z) * 30);   
        }

    }
    private void RotatePlayer()
    {

        if (movementDirection != Vector3.zero)
        {
            // Mira hacia la direccion donde se esta moviendo utilizando un lerp esferico
            Quaternion desiredRotation = Quaternion.LookRotation(movementDirection, Vector3.up);


            // Cambiara la velocidad de rotacion en caso de si estamos en el suelo o no
            if (groundedPlayer || onPlatform)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, floorRotationSpeed * Time.fixedDeltaTime);
            }
            else
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, airRotationSpeed * Time.fixedDeltaTime);
            }

        }
    }
    private void ApplyGravity() 
    {

        if (!gliding || isAttacking && gliding)
        {
            rb.AddForce(Physics.gravity * (rb.mass * rb.mass + extraGravity));
        }
        else
        {
            rb.AddForce(Physics.gravity * (rb.mass * rb.mass - glidingGravity));
        }
   
    }
    private void MoveShadow()
    {
        RaycastHit shadowHit;
        bool isGroundedNow = false;

        Ray[] floorRay = new Ray[floorRayPlaces.Length];
        floorRay[0] = new Ray(floorRayPlaces[0].position, -transform.up);

        foreach (Ray item in floorRay)
        {
            if (Physics.Raycast(item, out shadowHit, maxShadowDistance, floorLayer))
            {
                isGroundedNow = true;
                Shadow.SetActive(true);
                Shadow.transform.position = shadowHit.point;
                break;
            }
        }

        if (!isGroundedNow)
        {
            Shadow.SetActive(false);
        }
    }

    #endregion

    #region Checkers

    private void CheckMovementInput() 
    {
        if (recibedInputs.x != 0 && canMove && !CinematicsController._CINEMATICS_CONTROLLER.isPlayingCinematic || recibedInputs.y != 0 && canMove && !CinematicsController._CINEMATICS_CONTROLLER.isPlayingCinematic)
        {
            movementInput = Quaternion.Euler(0, followCamera.transform.eulerAngles.y, 0) * new Vector3(recibedInputs.x, 0, recibedInputs.y);
            animController.SetMovmentInput(true);
        }
        else
        {
            movementInput = Vector3.zero;
            animController.SetMovmentInput(false);

        }

        // Se coge la direccion en la que va a moverse el personaje en base a los inputs pulsados y los 
        movementDirection = movementInput.normalized;

        
    }
    private void CheckIfGrounded() 
    {
        // Detecta si esta tocando el suelo
        bool isGroundedNow = false;
        int rayCounter = 0;
        Ray[] floorRay = new Ray[floorRayPlaces.Length];
        floorRay[0] = new Ray(floorRayPlaces[0].position, -transform.up);
        floorRay[1] = new Ray(floorRayPlaces[1].position, -transform.up);
        floorRay[2] = new Ray(floorRayPlaces[2].position, -transform.up);
        floorRay[3] = new Ray(floorRayPlaces[3].position, -transform.up);
        floorRay[4] = new Ray(floorRayPlaces[4].position, -transform.up);

        foreach (Ray item in floorRay)
        {
            rayCounter++;
            if (Physics.Raycast(item, out groundHit, maxFloorCheckDistance, walkableLayers))
            {
                isGroundedNow = true;
                break;
            }
        }

        if (isGroundedNow)
        {
            for (int i = 0; i < floorRayPlaces.Length; i++)
            {
                Debug.DrawRay(floorRayPlaces[i].position, -transform.up, Color.blue);
            }

            groundedPlayer = true;
            animController.SetOnAir();
            animController.SetFirstJump(true);
        }
        else
        {
            groundedPlayer = false;
            animController.SetOnAir();
            animController.SetFirstJump(false);


        }

    }
    private void CheckRampMovement()
    {

        float angleFloor = Vector3.Angle(groundHit.normal, Vector3.up);

        //Si se esta moviendo y estas en un suelo que esta inclinado
        if (angleFloor != 0)
        {
            onRamp = true;

            if (movementInput != Vector3.zero)
            {

                //Dibujar los gizmos de los rayos de deteccion de las rampas
                Debug.DrawRay(rampRayPlaces[0].position, rampRayPlaces[0].forward, Color.red);
                Debug.DrawRay(rampRayPlaces[1].position, rampRayPlaces[1].forward, Color.red);
                Debug.DrawRay(rampRayPlaces[2].position, rampRayPlaces[2].forward, Color.red);
                Debug.DrawRay(rampRayPlaces[3].position, rampRayPlaces[3].forward, Color.green);
                Debug.DrawRay(rampRayPlaces[4].position, rampRayPlaces[4].forward, Color.green);
                Debug.DrawRay(rampRayPlaces[5].position, rampRayPlaces[5].forward, Color.green);


                RaycastHit groundHit2;
                Vector3 slopeOffset;
                Vector3 keepBackForce = Vector3.zero;
                bool goingDown = false;

                float angleDivieder = 1f;
                float accelMultiplier = 7f;
                //Esta subiendo
                if (Physics.Raycast(new Ray(rampRayPlaces[0].position, rampRayPlaces[0].forward), out groundHit2, maxRampCheckDistance, walkableLayers))
                {
                    //Se aplica el movimiento de subida segun el angulo de la subida
                    angleFloor = Vector3.Angle(groundHit2.normal, Vector3.up);
                    slopeOffset = new Vector3(movementDirection.x * currentAccel * accelMultiplier, angleFloor / angleDivieder, movementDirection.z * currentAccel );

                }
                else if (Physics.Raycast(new Ray(rampRayPlaces[1].position, rampRayPlaces[1].forward), out groundHit2, maxRampCheckDistance, walkableLayers))
                {
                    //Se aplica el movimiento de subida segun el angulo de la subida
                    angleFloor = Vector3.Angle(groundHit2.normal, Vector3.up);
                    slopeOffset = new Vector3(movementDirection.x * currentAccel * accelMultiplier, angleFloor / angleDivieder, movementDirection.z * currentAccel);
                }
                else if (Physics.Raycast(new Ray(rampRayPlaces[2].position, rampRayPlaces[2].forward), out groundHit2, maxRampCheckDistance, walkableLayers))
                {
                    //Se aplica el movimiento de subida segun el angulo de la subida
                    angleFloor = Vector3.Angle(groundHit2.normal, Vector3.up);
                    slopeOffset = new Vector3(movementDirection.x * currentAccel * accelMultiplier, angleFloor / angleDivieder, movementDirection.z * currentAccel * accelMultiplier);
                }
                else
                {

                    //Esta bajando
                    RaycastHit groundHit3;
                    


                    if (Physics.Raycast(new Ray(rampRayPlaces[3].position, rampRayPlaces[3].forward), out groundHit3, maxRampCheckDistance * 1.5f, walkableLayers) ||
                        Physics.Raycast(new Ray(rampRayPlaces[3].position, rampRayPlaces[4].forward), out groundHit3, maxRampCheckDistance * 1.5f, walkableLayers) ||
                        Physics.Raycast(new Ray(rampRayPlaces[3].position, rampRayPlaces[5].forward), out groundHit3, maxRampCheckDistance * 1.5f, walkableLayers))
                    {

                       

                        angleFloor = -Vector3.Angle(groundHit3.normal, Vector3.up);
                        Debug.Log(Vector3.Angle(groundHit3.normal, transform.forward));
                        float angleMultiplier = 2.5f;
                        float accelMultiplierGoingDown = 5.5f;
                        float backForceMultiplyer = 9;

                        //Debug.Log(angleFloor);
                        if (angleFloor > -40)
                        {
                            
                            if (rb.velocity.magnitude < 15)
                            {
                                slopeOffset = new Vector3(movementDirection.x * currentAccel * accelMultiplierGoingDown, angleFloor * angleMultiplier, movementDirection.z * currentAccel * accelMultiplierGoingDown);
                            }
                            else
                            {
                                slopeOffset = new Vector3(movementDirection.x * currentAccel, angleFloor * angleMultiplier, movementDirection.z * currentAccel);
                            }
                        }
                        else
                        {
                            slopeOffset = new Vector3(movementDirection.x * currentAccel / 2, angleFloor * angleMultiplier / 2, movementDirection.z * currentAccel / 2);

                        }



                        keepBackForce += -transform.forward * backForceMultiplyer;

                        goingDown = true;

                    }
                    else
                    {
                        //Si no que no agregue ninguna fuerza
                        slopeOffset = Vector3.zero;
                        angleFloor = 0;
                    }


                }

                if (!goingDown)
                {
                    rb.AddForce(slopeOffset, ForceMode.Acceleration);
                }
                else
                {
                    if (slopeOffset != Vector3.zero)
                    {
                        rb.AddForce(slopeOffset, ForceMode.VelocityChange);

                       
                    }

                    rb.AddForce(keepBackForce, ForceMode.Force);
                    if (rb.velocity.magnitude > 22.5f)
                    {
                        rb.AddForce(keepBackForce, ForceMode.Force);
                    }

                }

                animController.SetGoingDown(goingDown);
            }
            else
            {
                //En caso de que este quieto en una rampa se le aplica una fuerza para que no se mueva
                if (rb.velocity.y > 0)
                {
                    rb.AddForce(new Vector3(0, -rb.velocity.y, 0), ForceMode.VelocityChange);

                }
                else
                {
                    rb.AddForce(new Vector3(-rb.velocity.x, rb.velocity.y, -rb.velocity.z ) , ForceMode.VelocityChange);

                }
            }

        }
        else
        {
            onRamp = false;
        }

        if (angleFloor > 10 || angleFloor < -10)
        {
            animController.SetOnRamp(onRamp);
        }
        else
        {
            animController.SetOnRamp(false);
        }

    }
    private void CheckIfCanJump() {


        //Si el player esta en el suelo esto hace que no se caiga por la gravedad y reseteamos los valores del salto (cantidad de saltos, coyote time ...)
        if (groundedPlayer && rb.velocity.y < 0)
        {
            //Resetear doble salto, el coyote y de planeo
            doubleJumped = false;
            canCoyote = true;
            gliding = false;
            //Poner la velocidad a 0
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            // Setear el valor de la animacion de planear y resetear el trigger de planear para que no 
            animController.SetGliding(gliding);
            animController.ResetJumpTrigger();


        }


        // Si no esta tocando el suelo espera el tiempo indicado par que aun pueda hacer el primer salto
        if (!groundedPlayer && canCoyote)
        {
            StartCoroutine(WaitForCoyoteTime());
            
        }

        // En caso de estar tocando el suelo y que acabemos de tocar el suelo despues de saltar hacer que se ponga la velocidad de movimiento normal
        if (groundedPlayer && canMove)
        {
            SetRunning();
        }

    }
    private void CheckAccelSpeed()
    {
        if (groundedPlayer || onPlatform )
        {
            if (!running)
            {
                currentAccel = walkSpeed;
            }
            else
            {
                currentAccel = runSpeed;

            }
        }
        else
        {

            if (rb.velocity.y > 0 || !gliding)
            {
                currentAccel = airSpeed;
            }
            else
            {
                currentAccel = glidingSpeed;
            }
        }
        
    }

    #endregion

    #region Timers
    IEnumerator WaitForCoyoteTime()
    {
        //Esperar el coyote time

        yield return new WaitForSeconds(coyoteTime);
        canCoyote = false;


    }

    #endregion

    #region Input Actions
    private void SetMovmentValues(InputAction.CallbackContext obj)
    {
        //Guardamos el valor de los inputs de movimiento
        recibedInputs = moveAction.ReadValue<Vector2>();
    }
    private void Jump(InputAction.CallbackContext obj)
    {
        if (canMove && !CinematicsController._CINEMATICS_CONTROLLER.isPlayingCinematic)
        {
            if (groundedPlayer && !isAttacking || canCoyote && !isAttacking)
            {
                if (groundedPlayer && !onPlatform)
                {
                    // En caso de que el salto sea mientras se esta en el suelo se guardara el punto desde el que hemos saltado como el ultimo punto de spawn
                    checkPointManager.SetNewRespawn(transform.position);
                }


                // En caso de que este en el suelo o aun este a tiempo de utilizar el coyote time haz el 1r salto
                if (rb.velocity.y < 0)
                {
                    rb.velocity = new Vector3 (rb.velocity.x, 0, rb.velocity.z);
                }
                rb.AddForce(transform.up * jumpHeight * 10, ForceMode.Impulse);
                canCoyote = false;
                animController.JumpTrigger();
                audioController.JumpSound();

            }
            else if (!doubleJumped)
            {
                // En caso de que no haya hecho el doble salto que haga el doble salto
                if (rb.velocity.y < 0)
                {
                    rb.velocity = new Vector3(rb.velocity.x, 0 ,rb.velocity.z);
                }
                rb.velocity = Vector3.zero;
                rb.AddForce(transform.up * doubleJumpHeight * 10, ForceMode.Impulse);

                doubleJumped = true;
                animController.JumpTrigger();
                audioController.JumpSound();
            }
        }
        
    }
    private void StartGliding(InputAction.CallbackContext obj)
    {
        //Cuando apriete el boton empieza a planear
        gliding = true;
        animController.SetGliding(gliding);
    }
    private void StopGlide()
    {
        // Si deja de apretar el boton de salto que deje de planear
        gliding = false;
        animController.SetGliding(gliding);

    }
    private void SetRunning()
    {

        if (groundedPlayer || onPlatform )
        {
            // Revisar segun si ha apretado el boton de correr o no empezara a correr o dejara de hacerlo solo si esta en el suelo
            if (runAction.ReadValue<float>() == 1)
            {
                running = false;
            }
            else
            {
                running = true;
            }

            animController.SetRunning(running);

        }

    }
    #endregion

    #region Setters

    public void SetCanMove(bool _CanMove) {
        canMove = _CanMove;
    }
    
    #endregion

    #region Getters
    public bool GetGliding()
    {
        if (gliding && rb.velocity.y < 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    #endregion

    #region Extern Actions
    public void AddImpulse(Vector3 _impulseDir, float _impulseForce) {
        //A�adir un impulso con la fuerza que te pasen
        rb.AddForce(_impulseDir * _impulseForce * 10, ForceMode.Impulse);
        canCoyote = false;

    }

    public IEnumerator DoAttack(float _timeToWait, bool _canMove) {

        
        isAttacking = true;
        animController.SetIsAttacking(isAttacking);
        if (!_canMove)
        {
            rb.velocity = Vector3.zero;
            canMove = false;
        }
        
        yield return new WaitForSeconds(_timeToWait);
        isAttacking = false;
        animController.SetIsAttacking(isAttacking);
        if (!_canMove)
        {
            canMove = true;
        }

    }

    #endregion
}
