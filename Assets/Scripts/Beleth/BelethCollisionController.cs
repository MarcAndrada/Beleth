using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BelethCollisionController : MonoBehaviour
{
    private BelethMovementController movementController;
    private BelethCheckPointManager checkPointManager;
    private BelethHealthController healthController;

    private void Start()
    {
        checkPointManager = GetComponent<BelethCheckPointManager>();
        
    
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "CheckPoints")
        {
            checkPointManager.SetNewCheckPoint(other.transform.position);
        }
    }


}
