using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WrathBossAttackController : MonoBehaviour
{

    [Header("Below Floor")]
    [SerializeField]
    private float belowSpeed;
    [SerializeField]
    private float yOffset;
    [SerializeField]
    private float timeToWaitUnderPlayer;
    [SerializeField]
    [Tooltip("El tiempo que espera mientras esta quieto debajo del player (esta variable va en funcion de la de timeToWaitToGoUp, esa ha de ser mas grande que esta)")]
    private float timeWaitingToEmerge;
    [SerializeField]
    private float timeToWaitForNextAttack;
    [SerializeField]
    private GameObject belowFloor_Particles;
    private bool isBelowFloor = false;
    private bool movingOnY = false;
    private float placeToGoState = 0;

    private Vector3 posToEmerge;

    [Header("Lava Circle")]
    [SerializeField]
    private GameObject lavaCircle_Attack;
    [SerializeField]
    private float lavaCircle_Duration;

    [Header("Enemie Circle")]
    [SerializeField]
    private float enemieCircle_Duration;
    [SerializeField]
    private float enemieCircle_Offset;


    [Header("Brake Floor")]
    [SerializeField]
    private GameObject brakeFloor_Attack;
    [SerializeField]
    private float brakeFloor_Duration;
    [SerializeField]
    private float brakeFloorV2_Angle;


    private float timeWatied = 0;
    private float timeToWait = 0;
    private bool isAttacking;
    private bool resetPosition = false;

    private Vector3 starterPos;
    private GameObject player;

    private WrathBossStateController stateController;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        stateController = GetComponent<WrathBossStateController>();
        player = stateController.player;

        starterPos = transform.position;
        animator = GetComponentInChildren<Animator>();

        belowFloor_Particles.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        
        CheckIfAttacking();

        CheckIfMovingY();


    }


    private void CheckIfAttacking() 
    {

        if (isAttacking)
        {
            timeWatied += Time.deltaTime;
            if (timeWatied >= timeToWait)
            {

                stateController.isDoingAction = false;
                isAttacking = false;
                timeWatied = 0;
            }
        }
    }
    private void CheckIfMovingY() 
    {
        if (movingOnY)
        {
            if (!isBelowFloor)
            {
                //Bajada

                animator.SetBool("BelowFloor", true);


                if (placeToGoState >= 1)
                {
                    //Al llegar al suelo cambiar isBelowFloor a true y que el state sea 0 de nuevo y mover el boss a la posicion X y Z del target
                    placeToGoState = 0;
                    isBelowFloor = true;
                    transform.position = new Vector3(player.transform.position.x, starterPos.y - yOffset, player.transform.position.z);
                    belowFloor_Particles.SetActive(true);
                }
                else
                {
                    //Mover a la posicion y X tiempo despues que suba
                    placeToGoState += Time.deltaTime * belowSpeed;
                    Vector3 targetPos = new Vector3(transform.position.x, starterPos.y - yOffset, transform.position.z);
                    transform.position = Vector3.Lerp(transform.position, targetPos, placeToGoState);
                }



            }
            else
            {

                //Subida
                if (resetPosition)
                {
                                        
                    //Esperar el tiempo suficiente para que emerja el boss del suelo 
                    timeWatied += Time.deltaTime;

                    if (timeToWaitUnderPlayer <= timeWatied)
                    {
                        animator.SetBool("BelowFloor", false);

                        placeToGoState += Time.deltaTime * belowSpeed;

                        if (placeToGoState >= 1)
                        {
                            //Al acabar poner isAttacking en true y hacer que time to wait sea timeToWaitGoUp
                            isAttacking = true;
                            timeToWait = timeToWaitForNextAttack;
                            movingOnY = false;
                            isBelowFloor = false;
                            placeToGoState = 0;
                            timeWatied = 0;
                            resetPosition = false;

                        }
                        else
                        {
                            //Mover a la posicion y X tiempo despues que suba
                            transform.position = Vector3.Lerp(transform.position, starterPos, placeToGoState);
                            belowFloor_Particles.SetActive(false);

                        }
                    }
                    else
                    {
                        transform.position = new Vector3(starterPos.x, starterPos.y - yOffset, starterPos.z);
                    }
                }
                else
                {


                    //Esperar el tiempo suficiente para que emerja el boss del suelo 
                    timeWatied += Time.deltaTime;

                    if (timeToWaitUnderPlayer <= timeWatied)
                    {
                        animator.SetBool("BelowFloor", false);


                        placeToGoState += Time.deltaTime * belowSpeed;

                        if (placeToGoState >= 1)
                        {

                            //Al acabar poner isAttacking en true y hacer que time to wait sea timeToWaitGoUp
                            isAttacking = true;
                            timeToWait = timeToWaitForNextAttack;
                            movingOnY = false;
                            isBelowFloor = false;
                            placeToGoState = 0;
                            timeWatied = 0;
                        }
                        else
                        {
                            //Mover a la posicion y X tiempo despues que suba
                            Vector3 targetPos = new Vector3(posToEmerge.x, starterPos.y, posToEmerge.z);
                            transform.position = Vector3.Lerp(transform.position, targetPos, placeToGoState);
                            belowFloor_Particles.SetActive(false);

                        }
                    }
                    else
                    {

                        if (timeWatied < timeToWaitUnderPlayer - timeWaitingToEmerge)
                        {
                            posToEmerge = player.transform.position;
                            transform.position = new Vector3(player.transform.position.x, starterPos.y - yOffset, player.transform.position.z);
                        }


                    }
                }



            }
        }
    }
    
    public void GoBelowFloor() {
        movingOnY = true;
    }
    public void GoToStarterPos() {
        resetPosition = true;
        GoBelowFloor();
    }
    public bool InCenter() 
    {
        return transform.position == starterPos;
    }


    public void LavaCircleAttack() 
    {
        animator.SetTrigger("LavaCircle");
        isAttacking = true;
        timeToWait = lavaCircle_Duration;
        timeWatied = 0;
        //Accion del ataque
        Instantiate(lavaCircle_Attack, transform.position, Quaternion.Euler(-90,0,0));
    }

    public void EnemieCircleAttack()
    {
        animator.SetTrigger("LavaCircle");
        isAttacking = true;
        timeToWait = enemieCircle_Duration;
        timeWatied = 0;
        //Accion del ataque



    }
   
    public void BrakeFloorAttack()
    {
        animator.SetTrigger("BrakeFloor");
        isAttacking = true;
        timeToWait = brakeFloor_Duration;
        timeWatied = 0;
        

    }

    public void BrakeFloorAttackV2()
    {
        animator.SetTrigger("BrakeFloor");
        isAttacking = true;
        timeToWait = brakeFloor_Duration;
        timeWatied = 0;
        
    }

    public void BrakeFloorAttakAction() 
    {
        //Accion del ataque
        switch (stateController.currentFase)
        {
            case WrathBossStateController.BossFase.FASE_1:
                Instantiate(brakeFloor_Attack, transform.position, transform.rotation);
                break;
            case WrathBossStateController.BossFase.FASE_2:
            case WrathBossStateController.BossFase.FASE_3:
                Instantiate(brakeFloor_Attack, transform.position, transform.rotation);
                Instantiate(brakeFloor_Attack, transform.position, transform.rotation * Quaternion.Euler(0, brakeFloorV2_Angle, 0));
                Instantiate(brakeFloor_Attack, transform.position, transform.rotation * Quaternion.Euler(0, -brakeFloorV2_Angle, 0));
                break;
            default:
                break;
        }



    }

}
