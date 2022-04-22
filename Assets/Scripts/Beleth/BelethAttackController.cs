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
    private float wrathAttackCD;
    [SerializeField]
    private float wrathAttackDuration;


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
        if (canAttack && Time.timeScale > 0)
        {
            //Segun el tipo de ataque cambiara el tag del tridente
            switch (_attackType)
            {
                case 0:
                    // En caso de que sea un ataque normal
                    //Cambiarle el tag al tridente para saber el tipo de ataque
                    tridentController.ChangeTridentTag("Trident");
                    //Poner el tridente en su posicion
                    tridentController.SetTridentPos(1);
                    //Reproducir sonido
                    audioController.AttackSound(_attackType);
                    //Hacer la animacion
                    animController.AttackTrigger();
                    //Empezar el CD del ataque
                    StartCoroutine(WaitAttackCD(attackCD));
                    break;

                case 1:
                    // En caso de que sea un ataque con ira
                    if (movementController.groundedPlayer)
                    {
                        //Cambiarle el tag al tridente para saber el tipo de ataque
                        tridentController.ChangeTridentTag("Wrath");
                        //Poner el tridente en su posicion
                        tridentController.SetTridentPos(2);
                        //Reproducir sonido
                        audioController.AttackSound(_attackType);
                        //Hacer la animacion
                        animController.WrathAttackTrigger();

                        //Congelar el movimiento durante un tiempo mientras hace el ataque
                        StartCoroutine(movementController.DoAttack(wrathAttackDuration));
                        //Empezar el CD del ataque
                        StartCoroutine(WaitAttackCD(wrathAttackCD));

                    }
                    break;

                default:
                    break;
            }

            

            tridentController.ResetTridentPosTimer();
        }


    }

    private IEnumerator WaitAttackCD(float _timeToWait) {
        
        canAttack = false;
        yield return new WaitForSeconds(attackCD);
        canAttack = true;
        animController.ResetAttackTrigger();
    
    }

}
