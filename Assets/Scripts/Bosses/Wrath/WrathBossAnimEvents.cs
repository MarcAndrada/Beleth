using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WrathBossAnimEvents : MonoBehaviour
{
    private WrathBossAttackController wrathBossAttackController;

    private void Start()
    {
        wrathBossAttackController = GetComponentInParent<WrathBossAttackController>();
    }

    public void CallBrakeFloor() 
    { 
        wrathBossAttackController.BrakeFloorAttakAction();
    }




    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<BelethHealthController>().GetDamage(1);
        }
    }
}
