using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BelethCollisionController : MonoBehaviour
{
    private BelethMovementController movementController;
    private BelethCheckPointManager checkPointManager;
    private BelethHealthController healthController;
    private CharacterController characterController;
    private void Start()
    {
        movementController = GetComponent<BelethMovementController>();
        checkPointManager = GetComponent<BelethCheckPointManager>();
        healthController = GetComponent<BelethHealthController>();
        characterController = GetComponent<CharacterController>();

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "CheckPoints")
        {
            checkPointManager.SetNewCheckPoint(other.transform.position, other.gameObject.GetComponent<Animation>());
        }

        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "BossAttack" || other.gameObject.tag == "Boss" || other.gameObject.tag == "Lava")
        {
            healthController.GetDamage(1);

        }

        if (other.gameObject.tag == "Boss")
        {
            healthController.GetDamage(1);

        }
    }

    private void OnParticleCollision(GameObject other)
    {
        if (other.gameObject.tag == "BossAttack")
        {
            healthController.GetDamage(1);

        }
    }

}
