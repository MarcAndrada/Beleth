using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    

    //inputs
    private float horizontalInput = 0;
    private float verticalInput = 0;
    private Vector3 movementInput;
    private Vector3 movementDirection;

    //Player Speeds
    [SerializeField]
    private float playerSpeed;
    [SerializeField]
    private float rotationSpeed;
    private Vector3 playerVelocity;
    private float currentSpeed;
    private bool isAccelerating = false;
    [SerializeField]
    private float accelSpeed;
    [SerializeField]
    private float dragSpeed;

    //Jump
    [SerializeField]
    private float jumpHeight;
    [SerializeField]
    private float doubleJumpHeight;
    [SerializeField]
    private float gravityValue;
    [SerializeField]
    private float gravityOnFloor;
    [SerializeField]
    private float gravityGliding;
    [SerializeField]
    private float coyoteTime;
    private bool groundedPlayer;
    private bool doubleJumped = false;
    private bool canCoyote;


    //Components
    [SerializeField]
    private Camera followCamera;
    [SerializeField]
    private GameObject Paraguas;
    private CharacterController controller;
    

    private void Start()
    {
        controller = GetComponent<CharacterController>();

    }

    private void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        CheckInputSpeed();
        Jump();
        MovePlayer();
    }

    private void MovePlayer()
    {
        if (groundedPlayer)
        {
            //movimiento en el suelo
            movementInput = Quaternion.Euler(0, followCamera.transform.eulerAngles.y, 0) * new Vector3(horizontalInput, 0, verticalInput);
        }
        else
        {
            //definir movimiento en el aire
            if (horizontalInput != 0 || verticalInput != 0)
            {
                movementInput = Quaternion.Euler(0, followCamera.transform.eulerAngles.y, 0) * new Vector3(horizontalInput, 0, verticalInput);
            }
        }
        // Se coge la direccion en la que va a moverse el personaje en base a los inputs pulsados y los 
        movementDirection = movementInput.normalized;

        controller.Move(movementDirection * playerSpeed * currentSpeed * Time.deltaTime);

        RotatePlayer();


        // aplica la gravedad
        playerVelocity.y += gravityValue * Time.deltaTime;
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

    private void Jump() {

        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            //Si el player esta en el suelo esto hace que no se caiga por la gravedad
            playerVelocity.y = 0f;
            doubleJumped = false;
            gravityValue = gravityOnFloor;
            Paraguas.SetActive(false);
            canCoyote = true;

        }

        if (!groundedPlayer && canCoyote)
        {
            StartCoroutine(WaitForCoyoteTime());
        }

        if (Input.GetButtonDown("Jump"))
        {

            if (groundedPlayer || canCoyote)
            {
                playerVelocity.y = 0;
                playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
                canCoyote = false;
            }
            else if (!doubleJumped)
            {
                playerVelocity.y = 0;
                playerVelocity.y += Mathf.Sqrt(doubleJumpHeight * -3.0f * gravityValue);
                doubleJumped = true;
            }
            
        }

        if (Input.GetButton("Jump"))
        {

            //Si el boton de saltar esta presionado 
            if (playerVelocity.y < 0)
            {
                Paraguas.SetActive(true);
                gravityValue = gravityGliding;
            }
        }

        if (Input.GetButtonUp("Jump"))
        {
            gravityValue = gravityOnFloor;
            Paraguas.SetActive(false);
        }


    }

    private void CheckInputSpeed() {


        if (Input.GetButton("Horizontal") || Input.GetButton("Vertical"))
        {
            isAccelerating = true;
        }
        else
        {
            isAccelerating = false;
        }

        if (isAccelerating)
        {
            if (currentSpeed < 1)
            {
                currentSpeed += accelSpeed/1000;
            }
            else
            {
                currentSpeed = 1;
            }
        }
        else
        {
            if (currentSpeed > 0)
            {
                currentSpeed -= dragSpeed/1000;

            }
            else
            {
                currentSpeed = 0;
            }
        }
    }

    IEnumerator WaitForCoyoteTime() {

        yield return new WaitForSeconds(coyoteTime);
        canCoyote = false;

    }



}