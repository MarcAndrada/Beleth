using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WrathBossStateController : MonoBehaviour
{
    public enum BossFase { NONE, FASE_1, FASE_2, FASE_3 };
    public enum BossState {NONE, CHASING, ATTACKING, DEAD };
    public enum BossAttacks {NONE, LAVA_CIRCLE, ENEMIE_CIRCLE, BRAUM_R, BRAND_W, ROCK_RAIN };

    private BossFase currentFase = BossFase.NONE;
    private BossState currentState = BossState.NONE;
    private BossAttacks currentAttacks = BossAttacks.NONE;

    [SerializeField]
    private float maxHP;


    private float currentHP;
    private bool isChasing = false;

    private GameObject player;
    private NavMeshAgent navAgent;


    // Start is called before the first frame update
    void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
        player = GameObject.Find("Beleth");
        currentHP = maxHP;

        currentState = BossState.CHASING;
        CheckCurrentState();
    }

    // Update is called once per frame
    void Update()
    {
        DoCurrentAction();
    }


    //Check things
    private void CheckCurrentState() {

        switch (currentState)
        {
            case BossState.NONE:
                break;
            case BossState.CHASING:
                isChasing = true;
                ChasePlayer();
                break;
            case BossState.ATTACKING:
                DoRandomAttack();
                break;
            case BossState.DEAD:
                Die();
                break;
            default:
                break;
        }

    }
    private void DoCurrentAction() {
    
    }

    //Boss Actions
    private void ChasePlayer() {
        navAgent.SetDestination(player.transform.position);
    }
    private void DoRandomAttack() {

        switch (Random.Range(0, 6))
        {
            case 0:
                currentAttacks = BossAttacks.LAVA_CIRCLE;
                break;
            case 1:
                currentAttacks = BossAttacks.ENEMIE_CIRCLE;
                break;
            case 2:
                currentAttacks = BossAttacks.BRAUM_R;
                break;
            case 3:
                currentAttacks = BossAttacks.BRAND_W;
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
            case BossAttacks.BRAUM_R:

                break;
            case BossAttacks.BRAND_W:

                break;
            case BossAttacks.ROCK_RAIN:

                break;
            default:
                break;
        }
        
    }
    private void Die() {
    
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
                    CheckCurrentState();
                }

                break;
            default:
                break;
        }

    }
    public void SetBossState(BossState _nextState) {

        currentState = _nextState;
        CheckCurrentState();

    }



}
