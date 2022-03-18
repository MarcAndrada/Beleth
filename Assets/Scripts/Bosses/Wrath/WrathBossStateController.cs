using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WrathBossStateController : MonoBehaviour
{
    public enum BossFase { NONE, FASE_1, FASE_2, FASE_3 };


    public enum BossActions { NONE, BELOW_FLOOR, LAVA_CIRCLE, ENEMIE_CIRCLE, BRAKE_FLOOR }

    [SerializeField]
    public BossActions currentAction = BossActions.NONE;


    [Header("Boss Stats")]
    [SerializeField]
    private float rotationSpeed;
    [SerializeField]
    private float maxHP;
    [SerializeField]
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
        //StartCoroutine(StartFight());

        player = GameObject.FindWithTag("Player");


    }


    private void Update()
    {
        CheckCurrentFase();
        DoCurrentFase();

        //transform.rotation = Quaternion.LookRotation(player.transform.position, Vector3.up);
        
        LookAtPlayer();

        if (Input.GetKeyDown(KeyCode.J))
        {
            StartCoroutine(StartFight());
        
        }
    }

    private void CheckCurrentFase() 
    {
        switch (currentFase)
        {
            case BossFase.NONE:
                break;
            case BossFase.FASE_1:
                if (currentHP <= fasesHealth[0] && !isDoingAction)
                {
                    Debug.Log("Entramos en fase 2");
                    currentFase = BossFase.FASE_2;
                    currentAction = BossActions.BRAKE_FLOOR;
                    if (!attackController.InCenter())
                    {
                        attackController.GoToStarterPos();
                        isDoingAction = true;
                    }
                    
                    attacksDone = 0;
                }
                break;
            case BossFase.FASE_2:
                if (currentHP <= fasesHealth[1])
                {
                    Debug.Log("Entramos en fase 3");
                    currentFase = BossFase.FASE_3;
                    currentAction = BossActions.BELOW_FLOOR;
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

    private void LookAtPlayer() 
    {
        Vector3 lookPos = player.transform.position - transform.position;
        lookPos.y = 0;
        Quaternion rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * rotationSpeed);
    }


    private void Fase1() 
    {
        //Hace 1 subida de lava, a los 2 segundos hará un topo truco a la posición relativa de Beleth. < -Y repetirá esto 1 vez más. El jugador deberá huir/ esquivar durante estos ataques.
        //Entonces hará el topo truco al centro del coliseo y hará el aro de enemigos 2.
        //Esto le sirve al jugador para hacerle daño aplicando irá a los enemigos 2 y de este modo dañar al boss.
        //Si el jugador solo esquiva a los enemigos y no les aplica ira, el boss volverá a hacer el ataque.Hasta un límite de 2 o 3 veces.

        switch (currentAction)
        {
            case BossActions.BELOW_FLOOR:
                if (attacksDone < timesToDo_BelowFloor[0])
                {
                    isDoingAction = true;
                    //Debug.Log("Esta haciendo la subida " + attacksDone);
                    attackController.GoBelowFloor();
                    attacksDone++;
                    //Debug.Log("Es la " + attacksDone + " que hace el topotruco");

                }
                else if (attacksDone < timesToDo_BelowFloor[0] + 1)
                {
                    //Hacer topo truco al centro
                    attackController.GoToStarterPos();
                    attacksDone++;
                    isDoingAction = true;
                    //Debug.Log("Resetea la posicion");
                }
                else
                {
                    //Al acabar cambiar el esado al ataque de aros de enemigos
                    currentAction = BossActions.ENEMIE_CIRCLE;
                    attacksDone = 0;
                    //Debug.Log("Ha acabado de moverse");
                }
                break;
            case BossActions.ENEMIE_CIRCLE:
                if (attacksDone < timesToDo_EnemieCircle[0])
                {
                    attackController.EnemieCircleAttack();
                    attacksDone++;
                    isDoingAction = true;
                    //Debug.Log("Es la " + attacksDone + " que hace el circulo de enemigos");
                }
                else
                {
                    currentAction = BossActions.BRAKE_FLOOR;
                    attacksDone = 0;
                    //Debug.Log("El circulo de ataques ha vuelto ");
                }

                break;
            case BossActions.BRAKE_FLOOR:
                if (attacksDone < timesToDo_BrakeFloor[0])
                {
                    attackController.BrakeFloorAttack();
                    attacksDone++;
                    isDoingAction = true;
                    //Debug.Log("Es la " + attacksDone + " que hace la ulti de braum");
                }
                else
                {
                    currentAction = BossActions.BELOW_FLOOR;
                    attacksDone = 0;
                    //Debug.Log("Ha acabado el ataque de la ulti de braum");

                }
                break;
            default:
                break;
        }

    }

    private void Fase2()
    {
        //Resetea posición en el centro del coliseo.Hace una subida de lava v2 a los 2 segundos hace un topo truco a la posición relativa de Beleth. < -Y repetirá esto 2 veces más.
        //El jugador deberá huir/ esquivar durante estos ataques.
        //Después se va al centro del coliseo y hace el ataque de aros de enemigos 2(ahora mínimo 2 oleadas) + aros de lava.
        //Esto le sirve al jugador para hacerle daño aplicando irá a los enemigos 2 y de este modo dañar al boss.
        //Si el jugador solo esquiva a los enemigos y no les aplica ira, el boss volverá a hacer el ataque.Hasta un límite de 2 o 3 veces.

        switch (currentAction)
        {
            case BossActions.BELOW_FLOOR:

                if (attacksDone < timesToDo_BelowFloor[1])
                {
                    isDoingAction = true;
                    //Debug.Log("Esta haciendo la subida " + attacksDone);
                    attackController.GoBelowFloor();
                    attacksDone++;
                    //Debug.Log("Es la " + attacksDone + " que hace el topotruco");

                }
                else if (attacksDone < timesToDo_BelowFloor[1] + 1)
                {
                    //Hacer topo truco al centro
                    attackController.GoToStarterPos();
                    attacksDone++;
                    isDoingAction = true;
                    //Debug.Log("Resetea la posicion");
                }
                else
                {
                    //Al acabar cambiar el esado al ataque de aros de enemigos
                    currentAction = BossActions.ENEMIE_CIRCLE;
                    attacksDone = 0;
                    //Debug.Log("Ha acabado de moverse");
                }

                break;
            case BossActions.LAVA_CIRCLE:
                if (attacksDone < timesToDo_LavaCircle[1])
                {
                    attackController.LavaCircleAttack();
                    attacksDone++;
                    isDoingAction = true;
                }
                else
                {
                    currentAction = BossActions.BRAKE_FLOOR;
                }
                    
                break;
            case BossActions.ENEMIE_CIRCLE:

                if (attacksDone < timesToDo_EnemieCircle[1])
                {
                    attackController.EnemieCircleAttack();
                    attacksDone++;
                    isDoingAction = true;
                    //Debug.Log("Es la " + attacksDone + " que hace el circulo de enemigos");
                }
                else
                {
                    currentAction = BossActions.LAVA_CIRCLE;
                    attacksDone = 0;
                    //Debug.Log("El circulo de ataques ha vuelto ");
                }

                break;
            case BossActions.BRAKE_FLOOR:

                if (attacksDone < timesToDo_BrakeFloor[1])
                {
                    attackController.BrakeFloorAttackV2();
                    attacksDone++;
                    isDoingAction = true;
                    //Debug.Log("Es la " + attacksDone + " que hace la ulti de braum");
                }
                else
                {
                    currentAction = BossActions.BELOW_FLOOR;
                    attacksDone = 0;
                    //Debug.Log("Ha acabado el ataque de la ulti de braum");

                }

                break;
            default:
                Debug.Log("Error");
                break;
        }


    }

    void Fase3()
    {
        // hará el ataque de caída de piedras y topo truco. 
        //Durante esta fase el mundo entrara en ira y el “coliseo de pelea” tendrá la mecánica del nivel.
        //El jugador deberá huir/ esquivar durante estos ataques.Pero podrá atacar atrayendo al boss a las rocas restantes de la caída de piedras y/ o a las subidas de lava.
        isDoingAction = true;
        //Debug.Log("Esta haciendo la subida " + attacksDone);
        attackController.GoBelowFloor();
        attacksDone++;


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
