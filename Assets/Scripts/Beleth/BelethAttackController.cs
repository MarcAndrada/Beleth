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

    [HideInInspector]
    public bool canAttack = true;

    private BelethAnimController animController;
    private BelethMovementController movementController;
    private TridentController tridentController;
    private BelethAudioController audioController;
    private PlayerInput playerInput;
    private InputAction attackAction;

    // Start is called before the first frame update
    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        animController = GetComponent<BelethAnimController>();
        movementController = GetComponent<BelethMovementController>();
        tridentController = GetComponentInChildren<TridentController>();
        audioController = GetComponentInChildren<BelethAudioController>();

        //Attack Events
        attackAction = playerInput.actions["Attack"];
        attackAction.started += _ => AttackAction_started(0);

    }

    public void AttackAction_started(int _attackType)
    {
        if (canAttack && movementController.groundedPlayer)
        {
            //Segun el tipo de ataque cambiara el tag del tridente
            switch (_attackType)
            {
                case 0:
                    // En caso de que sea un ataque normal
                    tridentController.ChangeTridentTag("Trident");
                    audioController.AttackSound(_attackType);
                    break;
                case 1:
                    // En caso de que sea un ataque con ira
                    tridentController.ChangeTridentTag("Wrath");
                    audioController.AttackSound(_attackType);
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
