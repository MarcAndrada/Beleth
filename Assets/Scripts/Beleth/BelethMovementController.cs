using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

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
    private bool running;
    public bool onPlatform = false;
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
    private float maxFloorCheckDistance;
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
    private bool jump = false;
    private bool doubleJumped = false;
    private bool canCoyote;




    #endregion

    #region Components Variables
    [Header("Components & External objects")]
    [SerializeField]
    private Camera followCamera;
    [SerializeField]
    private PhysicMaterial physicMaterial;
    private Rigidbody rb;
    private PlayerInput playerInput;
    private BelethAnimController animController;
    private BelethCheckPointManager checkPointManager;
    private BelethAudioController audioController;
    private CapsuleCollider coll;
    private bool isAttacking = false;
    #endregion

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
        animController = GetComponent<BelethAnimController>();
        checkPointManager = GetComponent<BelethCheckPointManager>();
        audioController = GetComponentInChildren<BelethAudioController>();
        coll = GetComponent<CapsuleCollider>();

        //Setear valor a las animaciones
        //animController.SetSpeedValue(currentSpeed);
        animController.SetGliding(gliding);

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

        //maxWalkSpeed *= 1000;
        //maxRunSpeed *= 1000;
        //maxAirSpeed *= 1000;
        //maxGlidingSpeed *= 1000;

        //floorAccel *= 10000;
        //airAccel *= 10000;

       

    }

    private void Update()
    {
        
        CheckIfGrounded();
        CheckAccelSpeed();
        CheckIfCanJump();
        CheckMovementInput();
       

    }

    private void FixedUpdate()
    {
        ApplyGravity();
        //rb.AddForce(transform.forward * axis.y * 200,ForceMode.Acceleration);

        if (canMove)
        {
            RotatePlayer();
            MovePlayer();
        }

    }

    #region Actions
    private void MovePlayer()
    {

        rb.AddForce(movementDirection * currentAccel, ForceMode.Acceleration);

        //animController.SetSpeedValue(currentSpeed);

    }
    private void RotatePlayer()
    {

        if (movementDirection != Vector3.zero)
        {
            // Mira hacia la direccion donde se esta moviendo utilizando un lerp esferico
            Quaternion desiredRotation = Quaternion.LookRotation(movementDirection, Vector3.up);


            // Cambiara la velocidad de rotacion en caso de si estamos en el suelo o no
            if (groundedPlayer)
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

        if (!gliding)
        {
            rb.AddForce(Physics.gravity * (rb.mass * rb.mass + extraGravity));
        }
        else
        {
            rb.AddForce(Physics.gravity * (rb.mass * rb.mass - glidingGravity));
        }
   
    }
    #endregion

    #region Checkers

    private void CheckMovementInput() 
    {
        if (recibedInputs.x != 0 && canMove || recibedInputs.y != 0 && canMove)
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
        RaycastHit hit;
        if (Physics.Raycast(new Ray( new Vector3(transform.position.x,transform.position.y - coll.height/2,transform.position.z), -Vector3.up), out hit, maxFloorCheckDistance))
        {
            groundedPlayer = true;
            
        }
        else
        {
            groundedPlayer = false;
        }
    }
    private void CheckIfCanJump() {


        //Si el player esta en el suelo esto hace que no se caiga por la gravedad y reseteamos los valores del salto (cantidad de saltos, coyote time ...)
        if (groundedPlayer && rb.velocity.y < 0)
        {
            jump = false;
            doubleJumped = false;
            canCoyote = true;
            gliding = false;
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            // Setear el valor de la animacion de planear
            animController.SetGliding(gliding);
            animController.SetFirstJump(false);
            


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
        if (groundedPlayer)
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
        
        animController.SetSpeedValue(currentAccel);
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
        if (canMove)
        {
            if (groundedPlayer || canCoyote)
            {
                if (groundedPlayer && !onPlatform)
                {
                    // En caso de que el salto sea mientras se esta en el suelo se guardara el punto desde el que hemos saltado como el ultimo punto de spawn
                    checkPointManager.SetNewRespawn(transform.position);
                }

                animController.SetFirstJump(false);

                // En caso de que este en el suelo o aun este a tiempo de utilizar el coyote time haz el 1r salto
                if (rb.velocity.y < 0)
                {
                    rb.velocity = new Vector3 (rb.velocity.x, 0, rb.velocity.z);
                }
                rb.AddForce(transform.up * jumpHeight * 10, ForceMode.Impulse);
                canCoyote = false;
                animController.JumpTrigger();
                jump = true;

                audioController.JumpSound();
            }
            else if (!doubleJumped)
            {
                // En caso de que no haya hecho el doble salto que haga el doble salto
                if (rb.velocity.y < 0)
                {
                    rb.velocity = new Vector3(rb.velocity.x, 0 ,rb.velocity.z);
                }
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

        if (groundedPlayer)
        {
            // Revisar segun si ha apretado el boton de correr o no empezara a correr o dejara de hacerlo solo si esta en el suelo
            if (runAction.ReadValue<float>() == 1)
            {
                running = true;
            }
            else
            {
                running = false;
            }


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
    public void AddImpulse(float _impulseForce) {

        jump = true;
        //A�adir un impulso con la fuerza que te pasen

        

    }

    public IEnumerator DoAttack(float _attackBraking, float _timeToWait) {


        isAttacking = true;

        yield return new WaitForSeconds(_timeToWait);

        isAttacking = false;

    }

    #endregion
}
