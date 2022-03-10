using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WrathBossStateController : MonoBehaviour
{
    public enum BossFase { NONE, FASE_1, FASE_2, FASE_3 };
    public enum BossState {NONE, CHASING, ATTACKING, DEAD };
    public enum BossAttacks {NONE, LAVA_CIRCLE, ENEMIE_CIRCLE, BRAKE_FLOOR, FIRE_COLUMN, ROCK_RAIN };

    private BossFase currentFase = BossFase.NONE;
    private BossState currentState = BossState.NONE;
    private BossAttacks currentAttacks = BossAttacks.NONE;

    [SerializeField]
    private float maxHP;


    private float currentHP;

    private GameObject player;
    private NavMeshAgent navAgent;


    // Start is called before the first frame update
    void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
        player = GameObject.Find("Beleth");
        currentHP = maxHP;

        //ChangeCurrentState(BossState.CHASING);
    }

    // Update is called once per frame
    void Update()
    {
        DoCurrentAction();
    }

    //Checkers
    private void ChangeCurrentState(BossState _newState) {
        //Esta es la funcion que se utiliza para cambiar el estado del boss, en ella ya se setea todo lo necesario para cambiar ese estado
        currentState = _newState;

        switch (currentState)
        {
            case BossState.NONE:
                break;
            case BossState.CHASING:
                ChasePlayer();
                break;
            case BossState.ATTACKING:
                ChooseRandomAttack();
                break;
            case BossState.DEAD:
                Die();
                break;
            default:
                break;
        }

    }
    private void DoCurrentAction() {

        // Aqui esta definido que hara de forma continua en cada estado

        switch (currentState)
        {
            case BossState.NONE:
                break;
            case BossState.CHASING:
                ChasePlayer();
                break;
            case BossState.ATTACKING:
                break;
            case BossState.DEAD:
                break;
            default:
                break;
        }



    }


    //Boss Actions
    private void ChasePlayer() {
        navAgent.SetDestination(player.transform.position);
    }
    private void ChooseRandomAttack() {

        int nextAttack = Random.Range(0, 6);

        switch (nextAttack)
        {
            case 0:
                currentAttacks = BossAttacks.LAVA_CIRCLE;
                break;
            case 1:
                currentAttacks = BossAttacks.ENEMIE_CIRCLE;
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
    private void Die() {
    
    }


    //Boss Attacks
    private void LavaCircleAttack() {
    
    }
    private void EnemieCircleAttack() { 
    
    }
    private void BrakeFloorAttack() {
    
    }
    private void FireColumnAttack() { 
    
    }
    private void RockRainAttack() { 
    
    }


    // ExternActions
    public void GetDamage(float _damageDealt) {
        
        currentHP -= _damageDealt;
    }


    //Setters
    public void GoNextFase() {

        switch (currentFase)
        {
            case BossFase.NONE:
                currentFase = BossFase.FASE_1;

                break;
            case BossFase.FASE_1:
                currentFase = BossFase.FASE_2;

                break;
            case BossFase.FASE_2:
                currentFase = BossFase.FASE_3;

                break;
            case BossFase.FASE_3:
                if (currentHP <= 0) {
                    currentState = BossState.DEAD;
                    ChangeCurrentState(BossState.DEAD);
                }

                break;
            default:
                break;
        }

    }



}
