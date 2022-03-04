using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BelethAttackController : MonoBehaviour
{

    [Header("Attack")]
    [SerializeField]
    private float attackCD;
    [SerializeField]
    private float attackDuration;
    [SerializeField]
    private float attackDecelSpeed;
    [SerializeField]
    private GameObject followCamera;


    private bool canAttack = true;

    private CharacterController charController;
    private BelethAnimController animController;
    private BelethMovementController movementController;
    private PlayerInput playerInput;
    private InputAction attackAction;


    // Start is called before the first frame update
    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        charController = GetComponent<CharacterController>();
        animController = GetComponent<BelethAnimController>();
        movementController = GetComponent<BelethMovementController>();

        //Attack Events
        attackAction = playerInput.actions["Attack"];
        attackAction.started += AttackAction_started;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void AttackAction_started(InputAction.CallbackContext obj)
    {
        if (canAttack && charController.isGrounded)
        {
            //Hacer la animacion
            animController.AttackTrigger();
            //Congelar el movimiento durante el ataque
            StartCoroutine(movementController.DoAttack(attackDecelSpeed, attackDuration));
            //Empezar el CD del personaje
            StartCoroutine(WaitAttackCD());

        }


    }

    private IEnumerator WaitAttackCD() {
        
        canAttack = false;

        yield return new WaitForSeconds(attackCD);
        
        canAttack = true;
        animController.ResetAttackTrigger();
    
    }

}
