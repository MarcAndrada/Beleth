using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowRockController : MonoBehaviour
{

    [SerializeField]
    public Transform target;
    [SerializeField]
    private float throwForce;
    [SerializeField]
    private float upForce;
    [SerializeField]
    private float upForceNull;
    private WrathBossStateController bossController;
    private Rigidbody rb;
    private bool canExplodeOnBoss = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }


    private void Start()
    {
        if (target) 
        {
            target.TryGetComponent<WrathBossStateController>(out bossController);
        }

    }

    private void ThrowRock() 
    {
        if (target)
        {


            Vector3 targetDir = target.position - transform.position;
            if (bossController != null)
            {
                if (bossController.currentAction != WrathBossStateController.BossActions.RESET_POS && bossController.currentAction != WrathBossStateController.BossActions.BELOW_FLOOR)
                {
                    //Lanzar hacia el boss
                    rb.AddForce(targetDir * throwForce + Vector3.up * upForce, ForceMode.Impulse);
                    canExplodeOnBoss = true;
                }
                else
                {
                    //Tirar hacia arriba
                    rb.AddForce(Vector3.up * upForceNull, ForceMode.Impulse);

                }
            }
            else
            {
                //Lanzar hacia el objetivo
                rb.AddForce(targetDir * throwForce + Vector3.up * upForce, ForceMode.Impulse);

            }
        }
    }

    public void SetTarget(Transform _target) 
    {
        target = _target;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Trident")
        {
            ThrowRock();
        }

  

    }


    private void OnCollisionEnter(Collision collision)
    {
        if (canExplodeOnBoss && collision.gameObject.tag == "Boss")
        {
            WrathExplosionController controller = GetComponent<WrathExplosionController>();
            if (controller.isOnWrath) 
            {
                controller.WrathExplosion();
                BelethSinsController sins = bossController.player.GetComponent<BelethSinsController>();
                for (int i = 0; i < sins.wrathManager.Length; i++)
                {
                    if (sins.wrathManager[i] == controller)
                    {
                        sins.wrathManager[i] = null;
                        bossController.player.GetComponent<BelethUIController>().UpdateObjectList();
                        break;
                    }

                }
            }
            



        }
    }
}
