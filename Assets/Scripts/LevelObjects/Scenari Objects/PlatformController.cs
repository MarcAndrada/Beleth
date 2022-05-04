using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour
{
    public enum MovementType { WAIT_PLAYER, MOVE_ALLWAYS };

    [SerializeField]
    public MovementType currentMovement;
    [SerializeField]
    private Transform[] placeToGo;
    [SerializeField]
    private float speed;
    [SerializeField]
    private float timeToWaitAtPoint;
    [SerializeField]
    private bool restartWhenEnd;
    [SerializeField]
    [Tooltip("Si esta activado cuando llegue al ultimo punto volvera a empezar desde el 1 si no ira marcha atras \n Ej: El ultimo punto es el 8, en vez de ir al 1 va al 7 despues al 6 ...")]
    private bool backTrackRestart;

    [SerializeField]
    private int index = 0;
    [SerializeField]
    private float placeToGoState = 0;
    [SerializeField]
    private float minPlaceToGoState;
    private float timeWaitedAtPoint = 0;
    private bool ascending;
    private bool canMove = true;
    private bool playerAboard = false;
    private BelethMovementController playerCont;
    private BelethAnimController animController;
    private Rigidbody playerRb;
    private Rigidbody rb;
    private Vector3 offsetPlayer;
    // Start is called before the first frame update
    void Start()
    {
        ascending = true;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentMovement)
        {
            case MovementType.WAIT_PLAYER:
                if (playerAboard)
                {
                    if (canMove)
                    {
                        MoveNextPoint();
                    }
                    else
                    {
                        WaitToGoNextPoint();
                    }
                }
                else
                {
                    index = 0;
                    MoveNextPoint();

                }
                break;
            case MovementType.MOVE_ALLWAYS:
                if (canMove)
                {
                    MoveNextPoint();
                }
                else
                {
                    WaitToGoNextPoint();
                }
                break;
            default:
                break;
        }


    }

    private void MoveNextPoint() 
    {
        placeToGoState += (speed / 100) * Time.deltaTime;
        rb.MovePosition(Vector3.Lerp(transform.position, placeToGo[index].position, placeToGoState));
        if (placeToGoState >= minPlaceToGoState)
        {
            canMove = false;
        }

    }

    private void WaitToGoNextPoint() 
    {
        timeWaitedAtPoint += Time.deltaTime;

        if (timeWaitedAtPoint >= timeToWaitAtPoint) 
        {
            GoNextPoint();
            placeToGoState = 0;
            canMove = true;
            timeWaitedAtPoint = 0;

        }

    }

    private void GoNextPoint() {

        if (backTrackRestart)
        {
            index++;
            if (index > placeToGo.Length - 1)
            {
                if (restartWhenEnd)
                {
                    index = 0;
                }
                else
                {
                    index--;
                }
            }
        }
        else
        {

            if (ascending)
            {
                index++;

                if (index >= placeToGo.Length - 1)
                {
                    if (restartWhenEnd)
                    {
                        ascending = false;
                    }
                    else
                    {
                        index = placeToGo.Length - 1;
                    }
                }


            }
            else
            {
                index--;

                if (index == 0)
                {
                    ascending = true;
                }
            }
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player") {
            if (playerCont == null || animController == null)
            {
                playerCont = other.GetComponent<BelethMovementController>();
                animController = other.GetComponent<BelethAnimController>();
            }
            animController.SetOnPlatform(true);
            playerCont.onPlatform = true;
            
            offsetPlayer = other.transform.position - transform.position;
            playerRb = other.GetComponent<Rigidbody>();
            playerAboard = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            offsetPlayer = other.transform.position - transform.position;
            if (placeToGoState < minPlaceToGoState)
            {
                playerRb.MovePosition(Vector3.Lerp(transform.position, placeToGo[index].position, placeToGoState) + offsetPlayer);
            }
        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            playerCont.onPlatform = false;
            playerAboard = false;
            placeToGoState = 0;
        }
    }


}
