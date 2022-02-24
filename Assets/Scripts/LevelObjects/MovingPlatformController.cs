using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformController : MonoBehaviour
{
    //Navigation Points of the moving platform
    [SerializeField]
    Vector3[] navDest;

    Vector3 nextDest;
    int destIndex;


    
    [SerializeField]
    float tolerance; // Help platform snap correctly
    [SerializeField]
    float speed;    //How quickly the platform moves between points
    [SerializeField]
    float delay;    //Time the platform stays in a navDest before it starts to move again

    private float delayStart;
    Rigidbody rb;
    [SerializeField]
    bool autoMove;
    CharacterController cc;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {

        //Initiates the platform to move to the first point
        if (navDest.Length > 0)
        {
            nextDest = navDest[0];
            destIndex = 0;
         }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(transform.position != nextDest)
        {
            MovePlatform();
        }
        else
        {
            NextDestination();
        }
    }

    void MovePlatform()
    {
        Vector3 headingTo = nextDest - transform.position;
        rb.velocity = (headingTo / headingTo.magnitude) * speed;
        

        if(headingTo.magnitude < tolerance)
        {
            rb.position = nextDest;
            delayStart = Time.time;
        }

    }

    void NextDestination()
    {
        rb.velocity = new Vector3(0,0,0);
        if (autoMove)
        {
            if(Time.time - delayStart > delay)
            {
                destIndex++;
                if(destIndex >= navDest.Length)
                {
                    destIndex = 0;
                }
                nextDest = navDest[destIndex];
            }
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
            cc = other.GetComponent<CharacterController>();
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player" & cc)
            cc.Move(rb.velocity * Time.deltaTime);
    }
    private void OnTriggerExit(Collider other)
    {

        if (other.tag == "Player")
        {
            other.transform.SetParent(null);
        }

    }
}
