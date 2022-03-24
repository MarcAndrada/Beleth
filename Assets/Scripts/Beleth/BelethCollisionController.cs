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
        checkPointManager = GetComponent<BelethCheckPointManager>();
        healthController = GetComponent<BelethHealthController>();
        characterController = GetComponent<CharacterController>();

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
            healthController.GetDamage(1);

        }
    }

}
