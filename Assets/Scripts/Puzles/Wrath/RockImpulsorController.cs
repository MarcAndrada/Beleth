using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockImpulsorController : MonoBehaviour
{
    [SerializeField]
    private float timeToWaitImpulsing;
    private float timeWaitedImpulsing = 0;
    [SerializeField]
    private GameObject particles;

    private BoxCollider triggerColl;

    private void Awake()
    {
        triggerColl = GetComponent<BoxCollider>();
    }

    private void Start()
    {
        triggerColl.enabled = false;
        particles.SetActive(false);
    }

    private void Update()
    {
        CheckIfImpulsing();

    }

    public void ActivateImpulsor() 
    {
        triggerColl.enabled = true;
        particles.SetActive(true);
    }

    private void CheckIfImpulsing() 
    {
        if (triggerColl.enabled)
        {
            timeWaitedImpulsing += Time.deltaTime;

            if (timeWaitedImpulsing >= timeToWaitImpulsing)
            {

                triggerColl.enabled = false;
                particles.SetActive(false);
                timeWaitedImpulsing = 0;

            }

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Movable")
        {
            CinematicsController._CINEMATICS_CONTROLLER.PlaySpecificCinematic("StatueDestroyed");
            Destroy(other.gameObject);
        }
    }


}
