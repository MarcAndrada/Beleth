using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WrathBossStateController : MonoBehaviour
{
    public enum BossFase { NONE, FASE_1, FASE_2, FASE_3 };


    public enum BossActions { NONE, BELOW_FLOOR, LAVA_CIRCLE, ENEMIE_CIRCLE, BRAKE_FLOOR, DEAD }

    [SerializeField]
    public BossActions currentAction = BossActions.NONE;


    [Header("Boss Stats")]
    [SerializeField]
    private float maxHP;
    private float currentHP;

    [Header("Fases")]
    [SerializeField]
    private BossFase currentFase = BossFase.NONE;
    [SerializeField]
    [Tooltip("En la posicion 0 ha de estar la vida minima para estar en la fase 1, en la posicion 1 ha deestar la vida minima para la fase 2, lo que es inferior sera para la fase 3")]
    private float[] fasesHealth;

    [Header("Fase Attacks")]

    [SerializeField]
    private int[] timesToDo_BelowFloor;
    [SerializeField]
    private int[] timesToDo_LavaCircle;
    [SerializeField]
    private int[] timesToDo_EnemieCircle;
    [SerializeField]
    private int[] timesToDo_BrakeFloor;

    private int attacksDone = 0;


    public bool isDoingAction = false;

    private WrathBossAttackController attackController;
    public GameObject player;

    private void Start()
    {
        attackController = GetComponent<WrathBossAttackController>();

        currentHP = maxHP;
        StartCoroutine(StartFight());

        player = GameObject.FindWithTag("Player");


    }


    private void Update()
    {
        CheckCurrentFase();
        DoCurrentFase();

        transform.rotation = Quaternion.LookRotation(player.transform.forward, Vector3.up);
        

    }

    private void CheckCurrentFase() 
    {
        switch (currentFase)
        {
            case BossFase.NONE:
                break;
            case BossFase.FASE_1:
                if (currentHP <= fasesHealth[0])
                {
                    currentFase = BossFase.FASE_2;
                    currentAction = BossActions.BELOW_FLOOR;
                }
                break;
            case BossFase.FASE_2:
                if (currentHP <= fasesHealth[1])
                {
                    currentFase = BossFase.FASE_3;
                    currentAction = BossActions.BELOW_FLOOR;

                    //Activar ataque de rocas
                }
                break;
            case BossFase.FASE_3:
                if (currentHP <= 0)
                {
                    //Hacer muerte del boss
                    currentFase = BossFase.NONE;

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

        switch (currentAction)
        {
            case BossActions.NONE:
                break;
            case BossActions.BELOW_FLOOR:
                if (attacksDone < timesToDo_BelowFloor[0])
                {
                    isDoingAction = true;
                    //Debug.Log("Esta haciendo la subida " + attacksDone);
                    attackController.GoBelowFloor();
                    attacksDone++;
                    Debug.Log("Es la " + attacksDone + " que hace el topotruco");

                }
                else if (attacksDone < timesToDo_BelowFloor[0] + 1)
                {
                    //Hacer topo truco al centro
                    attackController.GoToStarterPos();
                    attacksDone++;
                    isDoingAction = true;
                    Debug.Log("Resetea la posicion");
                }
                else
                {
                    //Al acabar cambiar el esado al ataque de aros de enemigos
                    currentAction = BossActions.ENEMIE_CIRCLE;
                    attacksDone = 0;
                    Debug.Log("Ha acabado de moverse");
                }
                break;
            case BossActions.LAVA_CIRCLE:

                break;
            case BossActions.ENEMIE_CIRCLE:
                if (attacksDone < timesToDo_EnemieCircle[0])
                {
                    attackController.EnemieCircleAttack();
                    attacksDone++;
                    isDoingAction = true;
                    Debug.Log("Es la " + attacksDone + " que hace el circulo de enemigos");
                }
                else
                {
                    currentAction = BossActions.BRAKE_FLOOR;
                    attacksDone = 0;
                    Debug.Log("El circulo de ataques ha vuelto ");
                }

                break;
            case BossActions.BRAKE_FLOOR:
                if (attacksDone < timesToDo_BrakeFloor[0])
                {
                    attackController.BrakeFloorAttack();
                    attacksDone++;
                    isDoingAction = true;
                    Debug.Log("Es la " + attacksDone + " que hace la ulti de braum");
                }
                else
                {
                    currentAction = BossActions.BELOW_FLOOR;
                    attacksDone = 0;
                    Debug.Log("Ha acabado el ataque de la ulti de braum");

                }
                break;
            case BossActions.DEAD:
                break;
            default:
                break;
        }

    }

    private void Fase2()
    {
        //Durante esta fase el boss utilizara el ataque de subida de lava v2, el ataque aros de enemigos 2, aros de lava y topo truco
        //Resetea posici�n en el centro del coliseo.Hace una subida de lava v2 a los 2 segundos hace un topo truco a la posici�n relativa de Beleth. < -Y repetir� esto 2 veces m�s.
        //El jugador deber� huir/ esquivar durante estos ataques.
        //Despu�s se va al centro del coliseo y hace el ataque de aros de enemigos 2(ahora m�nimo 2 oleadas) + aros de lava.
        //Esto le sirve al jugador para hacerle da�o aplicando ir� a los enemigos 2 y de este modo da�ar al boss.
        //Si el jugador solo esquiva a los enemigos y no les aplica ira, el boss volver� a hacer el ataque.Hasta un l�mite de 2 o 3 veces.


    }

    void Fase3()
    {
        // har� el ataque de ca�da de piedras y topo truco. 
        //Durante esta fase el mundo entrara en ira y el �coliseo de pelea� tendr� la mec�nica del nivel.
        //El jugador deber� huir/ esquivar durante estos ataques.Pero podr� atacar atrayendo al boss a las rocas restantes de la ca�da de piedras y/ o a las subidas de lava.



    }
    // ExternActions
    public void GetDamage(float _damageDealt) {

        currentHP -= _damageDealt;
    }

    IEnumerator StartFight() 
    {


        yield return new WaitForSeconds(2f);
        currentAction = BossActions.BRAKE_FLOOR;
        currentFase = BossFase.FASE_1;
    
    }

}
