using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BelethCollisionController : MonoBehaviour
{
    private BelethCheckPointManager checkPointManager;
    private BelethHealthController healthController;
    private CoinController coinController;
    private BelethUIController uIController;

    private void Start()
    {
        checkPointManager = GetComponent<BelethCheckPointManager>();
        healthController = GetComponent<BelethHealthController>();
        coinController = GetComponent<CoinController>();
        uIController = GetComponent<BelethUIController>();


    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "CheckPoints")
        {
            checkPointManager.SetNewCheckPoint(other.transform.position, other.gameObject.GetComponent<Animation>());
        }

        if (other.gameObject.tag == "DeadZone")
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
            //Hacer algo con la moneda que encontremos 
            other.gameObject.SetActive(false);
        }

        if (other.gameObject.tag == "Collectable")
        {
            CollectableController currentCollectable = other.GetComponent<CollectableController>();
            uIController.ObtainedCollectable(currentCollectable.collectableID);
            currentCollectable.DisableCollectable();
        }


    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "BossAttack" || other.gameObject.tag == "Boss" || other.gameObject.tag == "Lava")
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
