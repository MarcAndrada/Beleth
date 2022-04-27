using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour
{
    

    [SerializeField]
    private Transform[] placeToGo;
    [SerializeField]
    private float speed;
    [SerializeField]
    private float timeToWaitAtPoint;
    [SerializeField]
    [Tooltip("Si esta activado cuando llegue al ultimo punto volvera a empezar desde el 1 si no ira marcha atras \n Ej: El ultimo punto es el 8, en vez de ir al 1 va al 7 despues al 6 ...")]
    private bool restartWhenEnd;

    [SerializeField]
    private int index = 0;
    [SerializeField]
    private float placeToGoState = 0;
    private Transform playerParent;
    private float timeWaitedAtPoint = 0;
    private bool ascending;
    private bool canMove = true;
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
        if (canMove)
        {
            MoveNextPoint();
        }
        else
        {
            WaitToGoNextPoint();
        }


    }

    private void MoveNextPoint() 
    {
        placeToGoState += (speed / 100) * Time.deltaTime;
        //transform.position = Vector3.Lerp(transform.position, placeToGo[index].position, placeToGoState);
        rb.MovePosition(Vector3.Lerp(transform.position, placeToGo[index].position, placeToGoState));
        if (placeToGoState >= 0.03f)
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

        if (restartWhenEnd)
        {
            index++;
            if (index > placeToGo.Length - 1)
            {
                index = 0;
            }
        }
        else
        {

            if (ascending)
            {
                index++;

                if (index == placeToGo.Length - 1)
                {
                    ascending = false;
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
            //playerParent = other.gameObject.transform.parent;
            //playerParent.SetParent(gameObject.transform);
            offsetPlayer = other.transform.position - transform.position;
            playerRb =other.GetComponent<Rigidbody>();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            offsetPlayer = other.transform.position - transform.position;
            if (placeToGoState < 0.03f)
            {
                playerRb.MovePosition(Vector3.Lerp(transform.position, placeToGo[index].position, placeToGoState) + offsetPlayer);
            }
        }

    }

    private void OnTriggerExit(Collider other)
    {

        if (other.tag == "Player")
        {
            //animController.SetOnPlatform(false);
            //playerCont.onPlatform = false;
            //playerParent.SetParent(null);
        }

    }


}
