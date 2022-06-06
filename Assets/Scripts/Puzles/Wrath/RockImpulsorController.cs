using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockImpulsorController : MonoBehaviour
{
    [SerializeField]
    private float timeToWaitImpulsing;
    private float timeWaitedImpulsing = 0;

    private BoxCollider triggerColl;

    private void Awake()
    {
        triggerColl = GetComponent<BoxCollider>();
    }

    private void Start()
    {
        triggerColl.enabled = false;
    }

    private void Update()
    {
        CheckIfImpulsing();    
    }

    public void ActivateImpulsor() 
    {
        triggerColl.enabled = true;
    }

    private void CheckIfImpulsing() 
    {
        if (triggerColl.enabled)
        {
            timeWaitedImpulsing += Time.deltaTime;

            if (timeWaitedImpulsing >= timeToWaitImpulsing)
            {

                triggerColl.enabled = false;
                timeWaitedImpulsing = 0;
            }

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Movable")
        {
            Debug.Log("OJOOO AQUI IRIA CINEMATICA");
            Destroy(other.gameObject);
        }
    }


}
