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
    private float timeToWaitToGoUp;
    [SerializeField]
    private float timeToWaitForNextAttack;

    private bool isBelowFloor = false;
    private bool movingOnY = false;
    private float placeToGoState = 0;

    private Vector3 posToEmerge;

    [Header("Lava Circle")]
    [SerializeField]
    private float lavaCircle_Duration;

    [Header("Enemie Circle")]
    [SerializeField]
    private float enemieCircle_Duration;

    [Header("Brake Floor")]
    [SerializeField]
    private float brakeFloor_Duration;




    private float timeWatied = 0;
    private float timeToWait = 0;
    private bool isAttacking;
    private bool resetPosition = false;

    private Vector3 starterPos;
    private GameObject player;

    private WrathBossStateController stateController;

    // Start is called before the first frame update
    void Start()
    {
        stateController = GetComponent<WrathBossStateController>();
        player = stateController.player;

        starterPos = transform.position;
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
                if (placeToGoState >= 1)
                {
                    //Al llegar al suelo cambiar isBelowFloor a true y que el state sea 0 de nuevo y mover el boss a la posicion X y Z del target
                    placeToGoState = 0;
                    isBelowFloor = true;
                    transform.position = new Vector3(player.transform.position.x, starterPos.y - yOffset, player.transform.position.z);
                    //Debug.Log("Ha parado de bajar");

                }
                else
                {
                    //Mover a la posicion y X tiempo despues que suba
                    placeToGoState += Time.deltaTime;
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

                    if (timeToWaitToGoUp <= timeWatied)
                    {

                        placeToGoState += Time.deltaTime;

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

                    if (timeToWaitToGoUp <= timeWatied)
                    {

                        placeToGoState += Time.deltaTime;

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

                        }
                    }
                    else
                    {

                        if (timeWatied < timeToWaitToGoUp - 1)
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
        isAttacking = true;
        timeToWait = lavaCircle_Duration;
        timeWatied = 0;
        //Accion del ataque
    }

    public void EnemieCircleAttack()
    {

        isAttacking = true;
        timeToWait = enemieCircle_Duration;
        timeWatied = 0;
        //Accion del ataque

    }
   
    public void BrakeFloorAttack()
    {

        isAttacking = true;
        timeToWait = brakeFloor_Duration;
        timeWatied = 0;
        //Accion del ataque

    }

    public void BrakeFloorAttackV2()
    {

        isAttacking = true;
        timeToWait = brakeFloor_Duration;
        timeWatied = 0;
        //Accion del ataque

    }




}
