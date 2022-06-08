using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WrathBossStateController : MonoBehaviour
{
    public enum BossFase { NONE, FASE_1, FASE_2, FASE_3 };

    public enum BossActions { NONE, RESET_POS, BELOW_FLOOR, LAVA_CIRCLE, ENEMIE_CIRCLE, BRAKE_FLOOR }

    public bool fighting = false;

    public BossActions currentAction = BossActions.NONE;

    private int attackIndex = 0;

    [Header("Boss Stats")]
    [SerializeField]
    private float rotationSpeed;
    [SerializeField]
    private float maxHP;
    [SerializeField]
    private float currentHP;
    
    [Header("Fases")]
    public BossFase currentFase = BossFase.NONE;
    [SerializeField]
    [Tooltip("En la posicion 0 ha de estar la vida minima para estar en la fase 1, en la posicion 1 ha deestar la vida minima para la fase 2, lo que es inferior sera para la fase 3")]
    private float[] fasesHealth;
    [SerializeField]
    private BossActions[] fase1Patron;
    [SerializeField]
    private BossActions[] fase2Patron;
    [SerializeField]
    private BossActions[] fase3Patron;

    [Header("Meteoritos")]
    [SerializeField]
    public GameObject rocksManager;

    [Header("Audios")]
    

    [HideInInspector]
    public bool isDoingAction = false;
    [HideInInspector]
    public bool isDamaged = false;

    [HideInInspector]
    public GameObject player;
    private WrathBossAttackController attackController;
    private Animator animator;
    private AudioSource audioSource;

    private void Awake()
    {
        attackController = GetComponent<WrathBossAttackController>();
        animator = GetComponentInChildren<Animator>();
        audioSource = GetComponent<AudioSource>();


        currentHP = maxHP;

        player = GameObject.FindWithTag("Player");

        rocksManager.SetActive(false);
    }


    private void Update()
    {
        if (fighting && !isDamaged)
        {
            CheckCurrentFase();
            DoCurrentFase();

            LookAtPlayer();
            if (Input.GetKeyDown(KeyCode.K))
            {
                currentHP -= 33;
            }
        }
    }

    private void LookAtPlayer()
    {
        Vector3 lookPos = player.transform.position - transform.position;
        lookPos.y = 0;
        Quaternion rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * rotationSpeed);
    }


    #region Check Fase
    private void CheckCurrentFase() 
    {
        switch (currentFase)
        {
            case BossFase.NONE:
                break;
            case BossFase.FASE_1:
                if (currentHP <= fasesHealth[0] && !isDoingAction)
                {
                    currentFase = BossFase.FASE_2;
                    if (!attackController.InCenter())
                    {
                        DoCurrentAttack(BossActions.RESET_POS);
                        isDoingAction = true;
                    }
                    attackIndex = 0;
                }
                break;
            case BossFase.FASE_2:
                if (currentHP <= fasesHealth[1])
                {
                    currentFase = BossFase.FASE_3;
                    
                    if (!attackController.InCenter())
                    {
                        DoCurrentAttack(BossActions.RESET_POS);
                        isDoingAction = true;
                    }

                    attackIndex = 0;

                    //Hacer que empiezen a caer rocas y el boss te persiga
                    //Activar ataque de rocas
                }
                break;
            case BossFase.FASE_3:
                if (currentHP <= 0)
                {
                    //Hacer muerte del boss
                    currentFase = BossFase.NONE;
                    //Lamar a la funcion de muerte
                    rocksManager.SetActive(false);
                    animator.SetBool("Dead", true);
                    SoundManager._SOUND_MANAGER.WrathBossDeadSound(audioSource);
                    SoundManager._SOUND_MANAGER.ChangeMusicLevel();

                }
                break;
            default:
                break;
        }
    }

    private void DoCurrentFase() 
    {
        if (!isDoingAction)
        {
            switch (currentFase)
            {
                case BossFase.NONE:
                    break;
                case BossFase.FASE_1:
                    Fase1();
                    break;
                case BossFase.FASE_2:
                    Fase2();
                    break;
                case BossFase.FASE_3:
                    Fase3();
                    break;
                default:
                    break;
            }
        }
        

    }

    private void Fase1() 
    {
        //Hace 1 subida de lava, a los 2 segundos har� un topo truco a la posici�n relativa de Beleth. < -Y repetir� esto 1 vez m�s. El jugador deber� huir/ esquivar durante estos ataques.
        //Entonces har� el topo truco al centro del coliseo y har� el aro de enemigos 2.
        //Esto le sirve al jugador para hacerle da�o aplicando ir� a los enemigos 2 y de este modo da�ar al boss.
        //Si el jugador solo esquiva a los enemigos y no les aplica ira, el boss volver� a hacer el ataque.Hasta un l�mite de 2 o 3 veces.

        CheckAttackIndex(0);

        DoCurrentAttack(fase1Patron[attackIndex]);

    }

    private void Fase2()
    {
        //Resetea posici�n en el centro del coliseo.Hace una subida de lava v2 a los 2 segundos hace un topo truco a la posici�n relativa de Beleth. < -Y repetir� esto 2 veces m�s.
        //El jugador deber� huir/ esquivar durante estos ataques.
        //Despu�s se va al centro del coliseo y hace el ataque de aros de enemigos 2(ahora m�nimo 2 oleadas) + aros de lava.
        //Esto le sirve al jugador para hacerle da�o aplicando ir� a los enemigos 2 y de este modo da�ar al boss.
        //Si el jugador solo esquiva a los enemigos y no les aplica ira, el boss volver� a hacer el ataque.Hasta un l�mite de 2 o 3 veces.


        CheckAttackIndex(1);

        DoCurrentAttack(fase2Patron[attackIndex]);


    }

    void Fase3()
    {
        // har� el ataque de ca�da de piedras y topo truco. 
        //Durante esta fase el mundo entrara en ira y el �coliseo de pelea� tendr� la mec�nica del nivel.
        //El jugador deber� huir/ esquivar durante estos ataques.Pero podr� atacar atrayendo al boss a las rocas restantes de la ca�da de piedras y/ o a las subidas de lava.
        CheckAttackIndex(2);
        DoCurrentAttack(fase3Patron[attackIndex]);

    }

    
    #endregion

    #region Attack
    private void CheckAttackIndex(int _fase) 
    {
        switch (_fase)
        {
            case 0:
                if (attackIndex >= fase1Patron.Length)
                {
                    attackIndex = 0;
                }
                break;
            case 1:
                if (attackIndex >= fase2Patron.Length)
                {
                    attackIndex = 0;
                }
                break;
            case 2:
                if (attackIndex >= fase3Patron.Length)
                {
                    attackIndex = 0;
                }
                break;
            default:
                break;
        }
    }

    private void DoCurrentAttack(BossActions _attack) 
    {

        isDoingAction = true;
        attackIndex++;
        if (rocksManager.activeInHierarchy)
        {
            switch (_attack)
            {
                case BossActions.NONE:
                    break;
                case BossActions.RESET_POS:
                    attackController.GoToStarterPos();
                    break;
                case BossActions.BELOW_FLOOR:
                    attackController.GoBelowFloor();
                    break;
                case BossActions.LAVA_CIRCLE:
                    attackController.LavaCircleAttack();
                    break;
                case BossActions.ENEMIE_CIRCLE:
                    attackController.EnemieCircleAttack();
                    break;
                case BossActions.BRAKE_FLOOR:
                    attackController.BrakeFloorAttack();
                    break;
                default:
                    break;
            }

            currentAction = _attack;

        }
        else
        {

            animator.SetTrigger("MeteorSummon");
            rocksManager.SetActive(true);

        }
    }


    #endregion

    #region ExternActions
    public void GetDamage(float _damageDealt, GameObject _obj) {

        //Hacer que deje de atacar
        attackController.SetIsAttacking(false);
        //Que deje de hacer cualquier ataque
        isDoingAction = false;
        //Reproducir Sonido
        SoundManager._SOUND_MANAGER.WrathBossDamagedSound(audioSource);
        //Quitarle vida
        currentHP -= _damageDealt;
        //Activar la animacion
        animator.SetTrigger("Damaged");
        //Decirle que ha sido da�ado
        isDamaged = true;
        Debug.Log("Toma danyo puta");

    }

    public void StartFight() 
    {
        fighting = true;
        isDamaged = false;
        isDoingAction = false;
        currentFase = BossFase.FASE_1;          
    }

    public void StopFight() 
    {
        attackController.PlayerDead();
        fighting = false;
        isDoingAction = true;
        currentFase = BossFase.NONE;
        currentAction = BossActions.NONE;
        rocksManager.SetActive(false);

    }

    #endregion

    public float GetMaxLife()
    {
        return maxHP;
    }

    public float GetActualLife()
    {
        return currentHP;
    }
}
