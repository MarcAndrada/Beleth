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
    private float maxPointDist;
    [SerializeField]
    private float timeToWaitAtPoint;
    [SerializeField]
    [Tooltip("Si esta activado cuando llegue al ultimo punto volvera a empezar desde el 1 si no ira marcha atras \n Ej: El ultimo punto es el 8, en vez de ir al 1 va al 7 despues al 6 ...")]
    private bool restartWhenEnd;

    private int index = 0;
    private Rigidbody rb;
    private float[] directions = new float[3];
    private bool[] reachedPoints = new bool[3];
    private bool ascending;
    private bool canMove = true;
    private CharacterController cc;
    private BelethMovementController playerCont;
    private BelethAnimController animController;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        ascending = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove)
        { 
            SetPlatformVelocities();
            StartCoroutine(CheckIfReachedPoints());
        }

        SetRBVelocities();
    }

    private void SetPlatformVelocities() {

        if (transform.position.x - placeToGo[index].transform.position.x > maxPointDist)
        {
            directions[0] = -speed;
        }
        else if (transform.position.x - placeToGo[index].transform.position.x < -maxPointDist)
        {
            directions[0] = speed;
        }
        else
        {
            directions[0] = 0;
            reachedPoints[0] = true;
        }

        if (transform.position.y - placeToGo[index].transform.position.y > maxPointDist)
        {
           
            directions[1] = -speed;
        }
        else if (transform.position.y - placeToGo[index].transform.position.y < -maxPointDist)
        {
            directions[1] = speed;
        }
        else
        {
            directions[1] = 0;
            reachedPoints[1] = true;
        }

        if (transform.position.z - placeToGo[index].transform.position.z > maxPointDist)
        {
            directions[2] = -speed;
        }
        else if (transform.position.z - placeToGo[index].transform.position.z < -maxPointDist)
        {
            directions[2] = speed;
        }
        else
        {
            directions[2] = 0;
            reachedPoints[2] = true;
        }

    }

    private void SetRBVelocities() {

        rb.velocity = new Vector3(directions[0], directions[1], directions[2]);

    }

    private IEnumerator CheckIfReachedPoints() {
        
        if (reachedPoints[0] && reachedPoints[1] && reachedPoints[2])
        {
            canMove = false;
            yield return new WaitForSeconds(timeToWaitAtPoint);
            GoNextPoint();
            for (int i = 0; i < reachedPoints.Length; i++)
            {
                reachedPoints[i] = false;
            }
            canMove = true;

        }
        else
        {
            yield return null;
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
            cc = other.GetComponent<CharacterController>();
            playerCont = other.GetComponent<BelethMovementController>();
            animController = other.GetComponent<BelethAnimController>();
            animController.SetOnPlatform(true);
            other.gameObject.GetComponent<BelethMovementController>().onPlatform = true;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player" & cc)
        {
            //if (rb.velocity.y <= 0)
            //{
            //    cc.Move(new Vector3(rb.velocity.x, rb.velocity.y, rb.velocity.z) * Time.deltaTime);
            //}
            //else
            //{
            //}
            cc.Move(new Vector3(rb.velocity.x, -0.0001f, rb.velocity.z) * Time.deltaTime);

        }
    }
    private void OnTriggerExit(Collider other)
    {

        if (other.tag == "Player")
        {
            animController.SetOnPlatform(false);
            other.gameObject.GetComponent<BelethMovementController>().onPlatform = false;
        }

    }


}
