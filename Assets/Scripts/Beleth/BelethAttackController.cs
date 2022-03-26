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


    public bool canAttack = true;

    private CharacterController charController;
    private BelethAnimController animController;
    private BelethMovementController movementController;
    private TridentController tridentController;
    private PlayerInput playerInput;
    private InputAction attackAction;

    // Start is called before the first frame update
    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        charController = GetComponent<CharacterController>();
        animController = GetComponent<BelethAnimController>();
        movementController = GetComponent<BelethMovementController>();
        tridentController = GetComponentInChildren<TridentController>();

        //Attack Events
        attackAction = playerInput.actions["Attack"];
        attackAction.started += _ => AttackAction_started(0);

    }

    public void AttackAction_started(int _attackType)
    {
        if (canAttack && charController.isGrounded)
        {
            //Segun el tipo de ataque cambiara el tag del tridente
            switch (_attackType)
            {
                case 0:
                    // En caso de que sea un ataque normal
                    tridentController.ChangeTridentTag("Trident");
                    break;
                case 1:
                    // En caso de que sea un ataque con ira
                    tridentController.ChangeTridentTag("Wrath");
                    break;
                default:
                    break;
            }

            tridentController.SetTridentPos(0);

            //Hacer la animacion
            animController.AttackTrigger();
            //Congelar el movimiento durante un tiempo mientras hace el ataque
            StartCoroutine(movementController.DoAttack(attackDecelSpeed, attackDuration));
            //Empezar el CD del ataque
            StartCoroutine(WaitAttackCD());

            tridentController.ResetTridentPosTimer();
        }


    }

    private IEnumerator WaitAttackCD() {
        
        canAttack = false;
        yield return new WaitForSeconds(attackCD);
        canAttack = true;
        animController.ResetAttackTrigger();
    
    }

}
