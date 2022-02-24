using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    //inputs
    private float horizontalInput = 0;
    private float verticalInput = 0;
    private Vector3 movementInput;
    private Vector3 movementDirection;
    private Vector2 recibedInputs;
    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction runAction;


    [Header("Player Movment")]
    [SerializeField]
    [Tooltip("Esta es la velocidad base que tendra el personaje")]
    private float playerSpeed;
    [SerializeField]
    [Tooltip("Velocidad en la que el personaje gira a los lados")]
    private float rotationSpeed;
    [Tooltip("Vector de velocidad del personaje (seria como el rigidbody.velocity pero para el character controller)")]
    private Vector3 playerVelocity;
    [Tooltip("Saber si el personaje se esta pulsando algun input de movimiento , esto se usa para la aceleracion o la deceleracion")]
    private bool isAccelerating = false;
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
    private float walkAccelSpeed;
    [SerializeField]
    [Tooltip("Valor al que va a ajustarse el multiplicador de velocidad mientras corre")]
    private float runAccelSpeed;
    [SerializeField]
    [Tooltip("Valor al que va a ajustarse el multiplicador de velocidad mientras esta volando")]
    private float airAccelSpeed;

    [Header("Jump")]
    [SerializeField]
    [Tooltip("Altura que tendra el salto base estando en el suelo")]
    private float jumpHeight;
    [SerializeField]
    [Tooltip("Altura del doble salto")]
    private float doubleJumpHeight;
    [SerializeField]
    [Tooltip("Valor con el que se haran los calculos de la gravedad")]
    private float gravityValue;
    [SerializeField]
    [Tooltip("Valor de la gravedad al estar en el suelo o en el aire")]
    private float gravityOnFloor;
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
    private bool groundedPlayer;
    private bool doubleJumped = false;
    private bool canCoyote;

    [Header("Components & External objects")]
    [SerializeField]
    private Camera followCamera;
    [SerializeField]
    private GameObject Paraguas;
    private CharacterController controller;
    private PlayerInput playerInput;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();

    //Movment Events
        moveAction = playerInput.actions["Move"];
        moveAction.started += SetMovmentValues;
        moveAction.performed += SetMovmentValues;
        moveAction.canceled += SetMovmentValues;

        // Jump Events
        jumpAction = playerInput.actions["Jump"];
        jumpAction.started += Jump;
        jumpAction.performed += SetGliding;
        jumpAction.canceled += StopGlide;

        // Run Events
        runAction = playerInput.actions["Run"];
        runAction.started += SetRunning;
        runAction.canceled += SetRunning;

        playerInput.actions["Restart"].started += Reset;

        //Setear la velocidad inicial
        currentStateSpeed = walkAccelSpeed;
        maxFallSpeed = normalFallSpeed;


    }

    private void SetMovmentValues(InputAction.CallbackContext obj)
    {
        recibedInputs = moveAction.ReadValue<Vector2>();
    }

    private void Update()
    {

        horizontalInput = recibedInputs.x;
        verticalInput = recibedInputs.y;




        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;


        CheckAccelSpeed();
        CheckIfCanJump();
        CheckIfGliding();
        MovePlayer();
    }

    private void MovePlayer()
    {
        if (groundedPlayer)
        {
            //movimiento en el suelo
            movementInput = Quaternion.Euler(0, followCamera.transform.eulerAngles.y, 0) * new Vector3(recibedInputs.x, 0, verticalInput);
        }
        else
        {
            //definir movimiento en el aire
            if (horizontalInput != 0 || verticalInput != 0)
            {
                movementInput = Quaternion.Euler(0, followCamera.transform.eulerAngles.y, 0) * new Vector3(recibedInputs.x, 0, verticalInput);
                currentStateSpeed = airAccelSpeed; 
            }
        }
        // Se coge la direccion en la que va a moverse el personaje en base a los inputs pulsados y los 
        movementDirection = movementInput.normalized;
        // Se le indica al controller que se mueva hacia la direccion indicada y ajustandolo a la velocidad que le queramos aplicar

        controller.Move(movementDirection * playerSpeed * currentSpeed * Time.deltaTime);
        

        RotatePlayer();

       
        // Se aplica la gravedad que le pasemos pero que no supere limite de velocidad de caida
        if (playerVelocity.y > maxFallSpeed)
        {
            playerVelocity.y += gravityValue * Time.deltaTime;
        }
        else
        {
            playerVelocity.y = maxFallSpeed;
        }

        controller.Move(playerVelocity * Time.deltaTime);
       
        
        
    }

    private void RotatePlayer() {

        if (movementDirection != Vector3.zero)
        {
            // Mira hacia la direccion donde se esta moviendo utilizando un lerp esferico
            Quaternion desiredRotation = Quaternion.LookRotation(movementDirection, Vector3.up);

            transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, rotationSpeed * Time.deltaTime);
        }
    }

    private void CheckIfCanJump() {

        groundedPlayer = controller.isGrounded;

        if (groundedPlayer && playerVelocity.y < 0)
        {
            //Si el player esta en el suelo esto hace que no se caiga por la gravedad y reseteamos los valores del salto (numer de saltos, coyote time ...)
            playerVelocity.y = 0f;
            doubleJumped = false;
            gravityValue = gravityOnFloor;
            Paraguas.SetActive(false);
            canCoyote = true;
            maxFallSpeed = normalFallSpeed;
            gliding = false;
        }

        if (!groundedPlayer && canCoyote)
        {
            // Si no esta tocando el suelo espera el tiempo indicado par que aun pueda hacer el primer salto
            StartCoroutine(WaitForCoyoteTime());
        }


    }
    private void CheckIfGliding() {

        //Si el boton de saltar esta presionado 
        if (playerVelocity.y < 0 && gliding)
        {
            //En el momento en el que el personaje deje de subir empezara a planear
            Paraguas.SetActive(true);
            gravityValue = gravityGliding;
            maxFallSpeed = glidingFallSpeed;
        }
    
    }
    IEnumerator WaitForCoyoteTime()
    {

        yield return new WaitForSeconds(coyoteTime);
        canCoyote = false;

    }

 

    private void CheckAccelSpeed() {

        // En esta funcion se revisara la velocidad a la que tiene que ir el personaje segun su estado

        if (horizontalInput != 0 || verticalInput != 0)
        {
            // En caso de que este presionando algun imput contara como que esta acelerando
            isAccelerating = true;
        }
        else
        {
            // En caso de que no presione ningun input contara como que esta decelerando
            isAccelerating = false;
        }


        if (isAccelerating)
        {

            //En caso de que esta acelerando revisaremos si tiene que hacelerar o frenar para ajustar el multiplicador al valor del estado actual
            if (groundedPlayer)
            {
                // Si estamos en el suelo
                if (currentSpeed < currentStateSpeed)
                {
                    // Aceleraremos con la velocidad de aceleracion en el suelo
                    currentSpeed += floorAccel / 1000;
                }
                else if(currentSpeed > currentStateSpeed)
                {
                    // Frenaremos con la velocidad de frenar en el suelo
                    currentSpeed -= floorBraking / 1000;
                }
                else
                {
                    
                    currentSpeed = currentStateSpeed;
                }

            }
            else
            {
                if (currentSpeed > currentStateSpeed)
                {
                    currentSpeed -= airBraking / 1000;
                }
                else
                {
                    currentSpeed = currentStateSpeed;
                }
            }


        }
        else
        {
            if (currentSpeed > 0)
            {
                if (groundedPlayer)
                {
                    currentSpeed -= floorBraking / 1000;
                }
                else
                {
                    currentSpeed -= airBraking / 1000;
                }

            }
            else
            {
                currentSpeed = 0;
            }
        }
    }

  

   

    private void Jump(InputAction.CallbackContext obj)
    {
        if (groundedPlayer || canCoyote)
        {
            // En caso de que este en el suelo o aun este a tiempo de utilizar el coyote time haz el 1r salto
            playerVelocity.y = 0;
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
            canCoyote = false;
        }
        else if (!doubleJumped)
        {
            // En caso de que no haya hecho el doble salto que haga el doble salto
            playerVelocity.y = 0;
            playerVelocity.y += Mathf.Sqrt(doubleJumpHeight * -3.0f * gravityValue);
            doubleJumped = true;
        }
    }
    private void SetGliding(InputAction.CallbackContext obj)
    {

        gliding = true;
        
    }
    private void StopGlide(InputAction.CallbackContext obj)
    {
        // Si deja de apretar el boton de salto que deje de planear
        gravityValue = gravityOnFloor;
        Paraguas.SetActive(false);
        maxFallSpeed = normalFallSpeed;
        gliding = false;
    }

    private void SetRunning(InputAction.CallbackContext obj)
    {

        if (groundedPlayer)
        {
            // Revisar segun si ha apretado el boton de correr o no empezara a correr o dejara de hacerlo solo si esta en el suelo
            if (runAction.ReadValue<float>() == 1)
            {
                currentStateSpeed = runAccelSpeed;
            }
            else
            {
                currentStateSpeed = walkAccelSpeed;

            }


        }

    }

    private void Reset(InputAction.CallbackContext obj)
    {
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);    
        
    }

}