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

    [SerializeField]
    bool autoMove;


    // Start is called before the first frame update
    void Start()
    {

        //Initiates the platform to move to the first point
        if(navDest.Length > 0)
        {
            nextDest = navDest[0];
            destIndex = 0;
         }
    }

    // Update is called once per frame
    void Update()
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
        transform.position += (headingTo / headingTo.magnitude) * speed * Time.deltaTime;

        if(headingTo.magnitude < tolerance)
        {
            transform.position = nextDest;
            delayStart = Time.time;
        }

    }

    void NextDestination()
    {
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
        if (other.gameObject.tag == "Player")
        {
            other.transform.parent = transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.transform.parent = null;
        }
       
    }
}
