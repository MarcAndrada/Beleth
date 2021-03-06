using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BelethCollisionController : MonoBehaviour
{
    private BelethCheckPointManager checkPointManager;
    private BelethHealthController healthController;
    private CoinController coinController;
    private BelethUIController uIController;
    BelethMovementController movementController;

    [SerializeField] GameObject BonesVFX;

    private void Start()
    {
        checkPointManager = GetComponent<BelethCheckPointManager>();
        healthController = GetComponent<BelethHealthController>();
        coinController = GetComponent<CoinController>();
        uIController = GetComponent<BelethUIController>();
        movementController = GetComponent<BelethMovementController>();

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "CheckPoints")
        {
            checkPointManager.SetNewCheckPoint(other.transform.position, other.gameObject.GetComponent<Animation>());
        }

        if (other.gameObject.tag == "DeadZone" || other.gameObject.tag == "Lava" || other.gameObject.tag == "Spike")
        {
            healthController.GetDamage(1);
            if (healthController.healthPoints > 0)
            {
                checkPointManager.GoLastRespawn();
            }
        }


        if (other.gameObject.tag == "Coin")
        {
            coinController.AddCoin();
            SoundManager._SOUND_MANAGER.SoulGet(SoundManager._SOUND_MANAGER.audioSource2D);
            Destroy(other.gameObject);
            Instantiate(BonesVFX, transform.position, Quaternion.identity);
        }

        if (other.gameObject.tag == "Collectable")
        {
            CollectableController currentCollectable = other.GetComponent<CollectableController>();
            uIController.ObtainedCollectable();
            currentCollectable.DisableCollectable();
        }

        if (other.gameObject.tag == "BossAttack" || other.gameObject.tag == "Boss")
        {
            healthController.GetDamage(1);
            if (other.gameObject.tag != "Boss")
            {
                Vector3 knockback = transform.position - other.transform.position;
                movementController.AddImpulse(knockback, 1);
            }
        }


    }

    private void OnTriggerStay(Collider other)
    {
        
    }

    private void OnParticleCollision(GameObject other)
    {
        if (other.gameObject.tag == "BossAttack")
        {
            healthController.GetDamage(1);
            Vector3 knockback = transform.position - other.transform.position;
            movementController.AddImpulse(knockback, 1);
        }
    }

}
