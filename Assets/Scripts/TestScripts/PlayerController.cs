using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField]
    private float playerSpeed;

    [SerializeField]
    private float rotationSpeed;

    [SerializeField]
    private Camera followCamera;

    

    [SerializeField]
    private float jumpHeight;
    [SerializeField]
    private float gravityValue;



    private float horizontalInput = 0;
    private float verticalInput = 0;
    private bool groundedPlayer;

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
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        
        
        Jump();
        MovePlayer();
        RotatePlayer();
        
    }

    void MovePlayer()
    {
        
        movementInput = Quaternion.Euler(0, followCamera.transform.eulerAngles.y, 0) * new Vector3(horizontalInput, 0, verticalInput);
        movementDirection = movementInput.normalized;

        controller.Move(movementDirection * playerSpeed * Time.deltaTime);
       

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
        }

        if (Input.GetButtonDown("Jump") && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }

    }



}