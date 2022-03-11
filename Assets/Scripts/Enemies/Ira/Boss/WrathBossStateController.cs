using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WrathBossStateController : MonoBehaviour
{
    public enum BossFase { NONE, FASE_1, FASE_2, FASE_3 };
    public enum BossState {NONE, BELOW_FLOOR, ATTACKING, DEAD };
    public enum BossAttacks {NONE, LAVA_CIRCLE, ENEMIE_CIRCLE, BRAKE_FLOOR, FIRE_COLUMN, ROCK_RAIN };

    private BossFase currentFase = BossFase.NONE;
    private BossState currentState = BossState.NONE;
    private BossAttacks currentAttacks = BossAttacks.NONE;

    [Header("Boss Stats")]
    [SerializeField]
    private float maxHP;
    [SerializeField]
    private float[] fasesHP;

    [Header("Lava Circle")]
    [SerializeField]
    private GameObject lavaCircleObj;
    [SerializeField]
    private int[] lavaCircle_AttackAmount;
    private int lavaCircle_CurrentMaxAttackAmount = 0;
    private int lavaCircle_AttacksLeft = 0;
    [SerializeField]
    private float lavaCircle_TimeToWait;
    private float lavaCircle_TimeWaited = 0;

    [Header("Enemie Circle")]
    [SerializeField]
    private int[] enemieCircle_AttackAmount;
    private int enemieCircle_CurrentMaxAttackAmount = 0;
    private int enemieCircle_AttacksLeft = 0;
    [SerializeField]
    private float enemieCircle_TimeToWait;
    private float enemieCircle_TimeWaited = 0;

    [Header("Brake Floor")]

    
    [Header("Fire Columns")]
    

    [Header("Rock Rain")]

    private float currentHP;

    private GameObject player;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Beleth");

        currentHP = maxHP;

    }

    // Update is called once per frame
    void Update()
    {
        DoCurrentAction();
        CheckCurrentFase();


    }

    //Checkers
    private void ChangeCurrentState(BossState _newState) {
        //Esta es la funcion que se utiliza para cambiar el estado del boss, en ella ya se setea todo lo necesario para cambiar ese estado
        currentState = _newState;

        switch (currentState)
        {
            case BossState.NONE:
                break;
            case BossState.BELOW_FLOOR:
                break;
            case BossState.ATTACKING:
                ChooseRandomAttack();
                ResetAttacksStats();
                break;
            case BossState.DEAD:
                Die();
                break;
            default:
                break;
        }

    }
    private void CheckCurrentFase() {
        switch (currentFase)
        {
            case BossFase.NONE:
                break;
            case BossFase.FASE_1:
                if (currentHP <= fasesHP[0])
                {
                    SetAttackValues(1);

                    currentFase = BossFase.FASE_2;
                    Debug.Log("Entramos en la fase 2");
                }

                break;
            case BossFase.FASE_2:
                if (currentHP <= fasesHP[1])
                {
                    SetAttackValues(2);

                    currentFase = BossFase.FASE_3;
                    Debug.Log("Entramos en la fase 2");

                }

                break;
            case BossFase.FASE_3:
                if (currentHP <= fasesHP[2]) {
                    currentState = BossState.DEAD;
                    ChangeCurrentState(BossState.DEAD);
                }
                break;
            default:
                break;
        }
    }
    private void EndCurrentState() {

        switch (currentState)
        {
            case BossState.NONE:

                break;
            case BossState.BELOW_FLOOR:

                break;
            case BossState.ATTACKING:

                break;
            case BossState.DEAD:

                break;
            default:
                break;
        }
    }
    private void CheckWhatToDoAfterAttack() { 
    
    }


    //Boss Actions
    private void ChooseRandomAttack() {

        int nextAttack = Random.Range(0, 6);

        switch (nextAttack)
        {
            case 0:
                currentAttacks = BossAttacks.LAVA_CIRCLE;
                lavaCircle_AttacksLeft = lavaCircle_CurrentMaxAttackAmount;

                break;
            case 1:
                currentAttacks = BossAttacks.ENEMIE_CIRCLE;
                enemieCircle_AttacksLeft = enemieCircle_CurrentMaxAttackAmount;
                break;
            case 2:
                currentAttacks = BossAttacks.BRAKE_FLOOR;

                break;
            case 3:
                currentAttacks = BossAttacks.FIRE_COLUMN;

                break;
            case 4:
                currentAttacks = BossAttacks.ROCK_RAIN;

                break;
            default:
                break;
        }


        switch (currentAttacks)
        {
            case BossAttacks.NONE:

                break;
            case BossAttacks.LAVA_CIRCLE:

                break;
            case BossAttacks.ENEMIE_CIRCLE:

                break;
            case BossAttacks.BRAKE_FLOOR:

                break;
            case BossAttacks.FIRE_COLUMN:

                break;
            case BossAttacks.ROCK_RAIN:

                break;
            default:
                break;
        }
        
    }
    private void DoCurrentAction()
    {

        // Aqui esta definido que hara de forma continua en cada estado

        switch (currentState)
        {
            case BossState.NONE:

                break;
            case BossState.BELOW_FLOOR:
                break;
            case BossState.ATTACKING:
                DoCurrentAttack();
                break;
            case BossState.DEAD:

                break;
            default:
                break;
        }



    }
    private void DoCurrentAttack()
    {
        switch (currentAttacks)
        {
            case BossAttacks.NONE:
                Debug.Log("Error no hay ataque");
                break;
            case BossAttacks.LAVA_CIRCLE:
                LavaCircleAttack();
                Debug.Log("ARO DE LAVAA");
                break;
            case BossAttacks.ENEMIE_CIRCLE:
                EnemieCircleAttack();
                Debug.Log("ARO DE ENEMIGOS");
                break;
            case BossAttacks.BRAKE_FLOOR:
                BrakeFloorAttack();
                Debug.Log("ROMPISION DE SUELO");
                break;
            case BossAttacks.FIRE_COLUMN:
                FireColumnAttack();
                Debug.Log("COLUMNA DE FUEGO");
                break;
            case BossAttacks.ROCK_RAIN:
                RockRainAttack();
                Debug.Log("LLUVIA DE ROCAS");
                break;
            default:
                break;
        }
    }
    private void Die() {
        Debug.Log("El boss ha muerto");



    }


    //Boss Attacks
    private void LavaCircleAttack() {
        
        if (lavaCircle_AttacksLeft > 0)
        {
            lavaCircle_TimeWaited += Time.deltaTime;
            if (lavaCircle_TimeWaited >= lavaCircle_TimeToWait)
            {
                lavaCircle_TimeWaited = 0;
                lavaCircle_AttacksLeft--;

                // Hacer el ataque

            }
        }
        else
        {
            // Condicion de salida
        }
    }
    private void EnemieCircleAttack() {

        if (enemieCircle_AttacksLeft > 0)
        {
            enemieCircle_TimeWaited += Time.deltaTime;
            if (enemieCircle_TimeWaited >= enemieCircle_TimeToWait)
            {
                enemieCircle_TimeWaited = 0;
                enemieCircle_AttacksLeft--;

                // Hacer el ataque

            }
        }
        else
        {
            // Condicion de salida

        }
    }
    private void BrakeFloorAttack() {
        

    }
    private void FireColumnAttack() { 
    
    }
    private void RockRainAttack() { 
    
    }

    private void ResetAttacksStats() { 
    
    }

    // ExternActions
    public void GetDamage(float _damageDealt) {
        
        currentHP -= _damageDealt;
    }


    //Setters
    private void SetAttackValues(int _currentFase) {

        lavaCircle_CurrentMaxAttackAmount = lavaCircle_AttackAmount[_currentFase];
        enemieCircle_CurrentMaxAttackAmount = enemieCircle_AttackAmount[_currentFase];
    }


}
