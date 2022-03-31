using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WrathBossAnimEvents : MonoBehaviour
{

    [SerializeField]
    private GameObject parent;
    private WrathBossAttackController wrathBossAttackController;
    private AudioSource audiosource;
    private void Start()
    {
        wrathBossAttackController = GetComponentInParent<WrathBossAttackController>();
        audiosource = GetComponentInParent<AudioSource>();

    }

    public void CallBrakeFloor() 
    { 
        wrathBossAttackController.BrakeFloorAttakAction();
    }


    public void DestroyBoss() 
    {
        Destroy(parent);
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<BelethHealthController>().GetDamage(1);
        }
    }
}
