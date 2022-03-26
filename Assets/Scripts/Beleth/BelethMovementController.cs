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
    private bool canMove = true;
    #endregion

    #region MovementVariables
    [Header("Movment")]
    [SerializeField]
    [Tooltip("Esta es la velocidad base que tendra el personaje")]
    private float playerSpeed;
    [SerializeField]
    [Tooltip("Velocidad en la que el personaje gira a los lados")]
    private float floorRotationSpeed;
    [SerializeField]
    private float airRotationSpeed;
    [Tooltip("Vector de velocidad del personaje (seria como el rigidbody.velocity pero para el character controller)")]
    private Vector3 playerVelocity;
    [Tooltip("Saber si el personaje se esta pulsando algun input de movimiento , esto se usa para la aceleracion o la deceleracion")]
    private bool isAccelerating = false;
    [SerializeField]
    [Tooltip("Variable que contiene el valor actual del multiplicador para hacer la aceleracion y deceleracion ")]
    private float currentSpeed;
    [SerializeField]
    [Tooltip("Velocidad en la que el personaje acelera estando en el suelo")]
    private float floorAccel;
    [SerializeField]
    [Tooltip("Velocidad en la que el personaje decelera en el suelo")]
    private float floorBraking;
    [SerializeField]
    [Tooltip("Velocidad en la que el personaje acelera estando en el aire")]
    private float airAccel;
    [SerializeField]
    [Tooltip("Velocidad en la que el personaje decelera en el aire")]
    private float airBraking;
    [Tooltip("Este es el multiplicador de velocidad y aceleracion el cual ira cambiando segun el estado en el que este (corriendo saltando ...)")]
    private float currentStateSpeed;
    [SerializeField]
    [Tooltip("Valor al que va a ajustarse el multiplicador de velocidad mientras anda")]
    private float walkSpeed;
    [SerializeField]
    [Tooltip("Valor al que va a ajustarse el multiplicador de velocidad mientras corre")]
    private float runSpeed;
    [SerializeField]
    [Tooltip("Valor al que va a ajustarse el multiplicador de velocidad mientras esta volando")]
    private float airSpeed;
    [SerializeField]
    private float minAirSpeed;
    [SerializeField]
    private float maxAirSpeed;
    [SerializeField]
    private float minGlidingSpeed;
    [SerializeField]
    private float maxGlidingSpeed;
    #endregion

    #region Jump Variables
    [Header("Jump")]
    private bool jump = false;
    [SerializeField]
    [Tooltip("Altura que tendra el salto base estando en el suelo")]
    private float jumpHeight;
    [SerializeField]
    [Tooltip("Altura del doble salto")]
    private float doubleJumpHeight;
    [SerializeField]
    private float jumpImpulse;
    [SerializeField]
    [Tooltip("Valor con el que se haran los calculos de la gravedad")]
    private float gravityValue;
    [SerializeField]
    [Tooltip("Valor de la gravedad al estar en el suelo")]
    private float gravityOnFloor;
    [SerializeField]
    [Tooltip("Valor de la gravedad al estar en el aire")]
    private float gravityOnAir;
    [SerializeField]
    [Tooltip("Valor de la gravedad al estar planeando")]
    private float gravityGliding;
    [Tooltip("Maxima velocidad de en la que puede caer el personaje")]
    private float maxFallSpeed;
    [SerializeField]
    [Tooltip("Maxima velocidad de caida")]
    private float normalFallSpeed;
    [SerializeField]
    [Tooltip("Maxima velocidad de caida mientras se planea")]
    private float glidingFallSpeed;
    private bool gliding = false;
    [SerializeField]
    [Tooltip("El tiempo que dispondra el player para saltar y no tener que hacer el salto pixel perfect")]
    private float coyoteTime;
    [SerializeField]
    private float maxFloorCheckDistance;
    private bool groundedPlayer;
    private bool doubleJumped = false;
    private bool canCoyote;
    #endregion

    #region Components Variables
    [Header("Components & External objects")]
    [SerializeField]
    private Camera followCamera;
    [SerializeField]
    private PhysicMaterial physicMaterial;
    private CharacterController charController;
    private PlayerInput playerInput;
    private BelethAnimController animController;
    private BelethCheckPointManager checkPointManager;
    private BelethUIController uiController;
    private float attackBraking;
    private bool isAttacking = false;
    #endregion


    private void Start()
    {
        charController = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();
        animController = GetComponent<BelethAnimController>();
        checkPointManager = GetComponent<BelethCheckPointManager>();
        uiController = GetComponent<BelethUIController>();

        gravityValue = gravityOnAir;

        //Setear valor a las animaciones
        animController.SetSpeedValue(currentSpeed);
        animController.SetGliding(gliding);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        //Movment Events
        moveAction = playerInput.actions["Move"];
        moveAction.started += SetMovmentValues;
        moveAction.performed += SetMovmentValues;
        moveAction.canceled += SetMovmentValues;

        // Jump Events
        jumpAction = playerInput.actions["Jump"];
        jumpAction.started += Jump;
        jumpAction.performed += SetGliding;
        jumpAction.canceled += _ => StopGlide();

        // Run Events
        runAction = playerInput.actions["Run"];
        runAction.started += _ => SetRunning();
        runAction.canceled += _ => SetRunning();

        //Setear la velocidad inicial
        currentStateSpeed = walkSpeed;
        maxFallSpeed = normalFallSpeed;

        


    }

    private void Update()
    {

        CheckAccelSpeed();
        CheckIfCanJump();
        if (canMove)
        {
            CheckIfGliding();
            MovePlayer();
            RotatePlayer();
        }
        else if(isAttacking)
        {
            MoveWhileAttacking();
        }
        CheckCurrentGravity();
        ApplyGravity();
       

    }


    #region Actions
    private void MovePlayer()
    {
        if (recibedInputs.x != 0 || recibedInputs.y != 0)
        {
            if (groundedPlayer)
            {
            
                //movimiento en el suelo
                movementInput = Quaternion.Euler(0, followCamera.transform.eulerAngles.y, 0) * new Vector3(recibedInputs.x, 0, recibedInputs.y);

            }
            else
            {
                //movimiento en el aire
                movementInput = Quaternion.Euler(0, followCamera.transform.eulerAngles.y, 0) * new Vector3(recibedInputs.x, 0, recibedInputs.y);
                currentStateSpeed = airSpeed;
            }

        }

        // Se coge la direccion en la que va a moverse el personaje en base a los inputs pulsados y los 
        movementDirection = movementInput.normalized;

        // Se le indica al controller que se mueva hacia la direccion indicada y ajustandolo a la velocidad que le queramos aplicar

        charController.Move(movementDirection * playerSpeed * currentSpeed * Time.deltaTime);

        animController.SetSpeedValue(currentSpeed);

    }
    private void MoveWhileAttacking() {

        //En caso de que este atacando solo se movera un poco hacia la direccion a la que se estaba iendo
        charController.Move(movementDirection * playerSpeed * currentSpeed * Time.deltaTime);

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
                transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, floorRotationSpeed * Time.deltaTime);
            }
            else
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, airRotationSpeed * Time.deltaTime);
            }

        }
    }
    private void ApplyGravity()
    {
        // Se aplica la gravedad que le pasemos pero que no supere limite de velocidad de caida
        if (playerVelocity.y > maxFallSpeed)
        {
            playerVelocity.y += gravityValue * Time.deltaTime;
        }
        else
        {
            playerVelocity.y = maxFallSpeed;
        }

        charController.Move(playerVelocity * Time.deltaTime);
        

    }
    #endregion

    #region Checkers
    private void CheckIfCanJump() {

        // Detecta si esta tocando el suelo
        groundedPlayer = charController.isGrounded;

        //Si el player esta en el suelo esto hace que no se caiga por la gravedad y reseteamos los valores del salto (cantidad de saltos, coyote time ...)
        if (groundedPlayer && playerVelocity.y < 0)
        {
            jump = false;
            doubleJumped = false;
            canCoyote = true;
            gliding = false;
            playerVelocity.y = 0f;
            maxFallSpeed = normalFallSpeed;
            // Setear el valor de la animacion de planear
            animController.SetGliding(gliding);
            animController.SetFirstJump(false);
            if (airSpeed > 0)
            {
                airSpeed = 0;
                if (currentSpeed > 2)
                {
                    currentSpeed = 2;
                }
                else
                {
                    currentSpeed /= 2;
                }
            }


        }


        // Si no esta tocando el suelo espera el tiempo indicado par que aun pueda hacer el primer salto
        if (!groundedPlayer && canCoyote)
        {
            CheckCurrentSpeedOnAir(0);
            StartCoroutine(WaitForCoyoteTime());
        }

        // En caso de estar tocando el suelo y que acabemos de tocar el suelo despues de saltar hacer que se ponga la velocidad de movimiento normal
        if (groundedPlayer && canMove)
        {
            SetRunning();
        }

    }
    private void CheckIfGliding() {

        //Si esta cayendo y esta planeando 
        if (playerVelocity.y < 0 && gliding && uiController.GetStaminaValue() > 0.05f)
        {
            //En el momento en el que el personaje deje de subir empezara a planear
            maxFallSpeed = glidingFallSpeed;
            CheckCurrentSpeedOnAir(1);

        }
        else if (gliding && uiController.GetStaminaValue() < 0.05f) 
        {
            StopGlide();
        }
    
    }
    private void CheckAccelSpeed()
    {
        if (!isAttacking)
        {

            // En esta funcion se revisara la velocidad a la que tiene que ir el personaje segun su estado

            if (recibedInputs.x != 0 || recibedInputs.y != 0)
            {
                // En caso de que este presionando algun imput contara como que esta acelerando
                isAccelerating = true;
            }
            else
            {
                // En caso de que no presione ningun input contara como que esta decelerando
                isAccelerating = false;
            }

            // Decir si esta pulsando algun input o no
            animController.SetMovmentInput(isAccelerating);

            if (isAccelerating)
            {

                //En caso de que esta acelerando revisaremos si tiene que hacelerar o frenar para ajustar el multiplicador al valor del estado actual
                if (groundedPlayer)
                {
                    // Si estamos en el suelo
                    if (currentSpeed < currentStateSpeed)
                    {
                        //Acelerarar
                        currentSpeed += floorAccel * Time.deltaTime;
                    }
                    else if (currentSpeed > currentStateSpeed)
                    {
                        //Frenar
                        currentSpeed -= floorBraking * Time.deltaTime;
                    }
                    else
                    {

                        currentSpeed = currentStateSpeed;
                    }

                }
                else
                {
                    // Si estamos en el aire
                    if (currentSpeed < currentStateSpeed)
                    {
                        //Acelerarar
                        currentSpeed += airAccel * Time.deltaTime;
                        
                    }
                    else if (currentSpeed > currentStateSpeed)
                    {
                        //Frenar
                        currentSpeed -= airBraking * Time.deltaTime;
                    }
                    else
                    {
                        currentSpeed = currentStateSpeed;
                    }

                }


            }
            else
            {
                // En caso de que no estemos pulsando ningun boton simplemente frenara hasta que la velocidad horizontal sea 0

                if (currentSpeed > 0)
                {
                    if (groundedPlayer)
                    {
                        currentSpeed -= floorBraking * Time.deltaTime;
                    }
                    else
                    {
                        currentSpeed -= airBraking * Time.deltaTime;
                    }

                }
                else
                {
                    currentSpeed = 0;
                }
            }
        }else
        {
            //Si estamos atacando frenara con la velocidad de frenada que le hemos dado

            if (currentSpeed > 0)
            {
                currentSpeed -= attackBraking * Time.deltaTime;
            }
            else
            {
                currentSpeed = 0;
            }

        }
        
    }
    private void CheckCurrentGravity() 
    {
        RaycastHit hit;
        if (Physics.Raycast(new Ray(transform.position, -Vector3.up), out hit, maxFloorCheckDistance) && !jump)
        {
            gravityValue = gravityOnFloor;
        }
        else
        {
            
            if (!gliding || playerVelocity.y > 0)
            {
                gravityValue = gravityOnAir;
            }
            else
            {
                gravityValue = gravityGliding;
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
        if (canMove)
        {
            if (groundedPlayer || canCoyote)
            {
                if (groundedPlayer)
                {
                    // En caso de que el salto sea mientras se esta en el suelo se guardara el punto desde el que hemos saltado como el ultimo punto de spawn
                    checkPointManager.SetNewRespawn(transform.position);
                }

                animController.SetFirstJump(false);

                // En caso de que este en el suelo o aun este a tiempo de utilizar el coyote time haz el 1r salto
                if (playerVelocity.y < 0)
                {
                    playerVelocity.y = 0;
                }
                playerVelocity.y += Mathf.Sqrt(jumpHeight * -jumpImpulse * gravityOnAir);
                canCoyote = false;
                animController.JumpTrigger();
                jump = true;
                gravityValue = gravityOnAir;

                CheckCurrentSpeedOnAir(0);

            }
            else if (!doubleJumped)
            {
                // En caso de que no haya hecho el doble salto que haga el doble salto
                if (playerVelocity.y < 0)
                {
                    playerVelocity.y = 0;
                }
                playerVelocity.y += Mathf.Sqrt(doubleJumpHeight * -jumpImpulse * gravityOnAir);
                doubleJumped = true;
                animController.JumpTrigger();

            }
        }
        
    }
    private void SetGliding(InputAction.CallbackContext obj)
    {
        //Cuando apriete el boton empieza a planear
        gliding = true;
        animController.SetGliding(gliding);
        
    }
    private void StopGlide()
    {
        // Si deja de apretar el boton de salto que deje de planear
        maxFallSpeed = normalFallSpeed;
        gliding = false;
        animController.SetGliding(gliding);
        CheckCurrentSpeedOnAir(0);

    }
    private void SetRunning()
    {

        if (groundedPlayer)
        {
            // Revisar segun si ha apretado el boton de correr o no empezara a correr o dejara de hacerlo solo si esta en el suelo
            if (runAction.ReadValue<float>() == 1)
            {
                currentStateSpeed = runSpeed;
            }
            else
            {
                currentStateSpeed = walkSpeed;

            }


        }

    }
    #endregion

    #region Setters
    public void SetCanMove(bool _CanMove) {
        canMove = _CanMove;
    }
    public void SetCurrentSpeed(float _newCurrentSpeed)
    {
        currentSpeed = _newCurrentSpeed;
    }

    #endregion

    #region Getters
    public bool GetGliding()
    {
        if (gliding && playerVelocity.y < 0)
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
        playerVelocity.y = Mathf.Sqrt(_impulseForce * -jumpImpulse * gravityOnAir);
        gravityValue = gravityOnAir;


    }

    public IEnumerator DoAttack(float _attackBraking, float _timeToWait) {

        /* Para hacer el ataque:
         * Primero se congela el movimiento
         * Se cambia la velocidad en la que se frena el movimiento y se le cambia la velocidad a la que va
         */


        canMove = false;
        attackBraking = _attackBraking;
        isAttacking = true;

        yield return new WaitForSeconds(_timeToWait);

        // Despues de que acabe la duracion del ataque se desbloquea el movimiento
        isAttacking = false;
        canMove = true;

    }

    #endregion

    #region Others
    private void CheckCurrentSpeedOnAir(int _typeOfVelocity) {

        float minSpeedToCheck = 0;
        float maxSpeedToCheck = 0;

        //Seguna la velocidad en la que vaya antes de saltar se limitara si es mucha o muy poca
        switch (_typeOfVelocity)
        {
            case 0:
                minSpeedToCheck = minAirSpeed;
                maxSpeedToCheck = maxAirSpeed;
                break;
            case 1:
                minSpeedToCheck = minGlidingSpeed;
                maxSpeedToCheck = maxGlidingSpeed;
                break;
            default:
                break;
        }
        if (currentSpeed > maxSpeedToCheck)
        {
            airSpeed = maxSpeedToCheck;
        }
        else if (currentSpeed < minSpeedToCheck)
        {
            airSpeed = minSpeedToCheck;
        }
        else
        {
            airSpeed = currentSpeed;
        }

    }

    #endregion
}