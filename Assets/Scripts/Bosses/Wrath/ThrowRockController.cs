using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowRockController : MonoBehaviour
{

    [SerializeField]
    private Transform target;
    [SerializeField]
    private float throwForce;
    [SerializeField]
    private float upForce;
    [SerializeField]
    private float upForceNull;
    private WrathBossStateController bossController;
    private Rigidbody rb;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }


    private void Start()
    {
        target.TryGetComponent<WrathBossStateController>(out bossController);

    }

    private void ThrowRock() 
    {

        Vector3 targetDir = target.position - transform.position;
        if (bossController != null)
        {
            if (bossController.currentAction != WrathBossStateController.BossActions.RESET_POS && bossController.currentAction != WrathBossStateController.BossActions.BELOW_FLOOR)
            {
                //Lanzar hacia el boss
                rb.AddForce(targetDir * throwForce + Vector3.up * upForce, ForceMode.Impulse);

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
}
