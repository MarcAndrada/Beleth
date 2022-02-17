using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField]
    private Camera followCamera;
    [SerializeField]
    GameObject Paraguas;

    [SerializeField]
    private float playerSpeed;
    [SerializeField]
    private float rotationSpeed;
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

    

    private float horizontalInput = 0;
    private float verticalInput = 0;
    private bool groundedPlayer;
    private bool doubleJumped = false;
    private bool canCoyote;
    private Vector3 playerVelocity;
    private Vector3 movementInput;
    private Vector3 movementDirection;

    private CharacterController controller;


    private void Start()
    {
        controller = GetComponent<CharacterController>();

    }

    private void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        Jump();
        MovePlayer();
    }

    void MovePlayer()
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

        controller.Move(movementDirection * playerSpeed * Time.deltaTime);

        RotatePlayer();


        // aplica la gravedad
        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }

    void RotatePlayer() {

        if (movementDirection != Vector3.zero)
        {
            Quaternion desiredRotation = Quaternion.LookRotation(movementDirection, Vector3.up);

            transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, rotationSpeed * Time.deltaTime);
        }
    }

    void Jump() {

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


    IEnumerator WaitForCoyoteTime() {

        yield return new WaitForSeconds(coyoteTime);
        canCoyote = false;

    }



}