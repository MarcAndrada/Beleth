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
        healthController = GetComponent<BelethHealthController>();


    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "CheckPoints")
        {
            checkPointManager.SetNewCheckPoint(other.transform.position);
        }
    }



    private void OnParticleCollision(GameObject other)
    {
        if (other.gameObject.tag == "BossAttack")
        {
            Debug.Log("Tonto x2");
            healthController.GetDamage(1);

        }
    }

}
