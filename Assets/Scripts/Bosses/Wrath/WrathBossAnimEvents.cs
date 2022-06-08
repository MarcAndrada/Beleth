using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WrathBossAnimEvents : MonoBehaviour
{

    [SerializeField]
    private GameObject parent;
    private WrathBossStateController bossController;
    private WrathBossAttackController wrathBossAttackController;
    private void Awake()
    {
        bossController = GetComponentInParent<WrathBossStateController>();
        wrathBossAttackController = GetComponentInParent<WrathBossAttackController>();

    }

    public void CallBrakeFloor() 
    { 
        wrathBossAttackController.BrakeFloorAttakAction();
    }

    public void MeteorAnimEnd()
    {
        bossController.isDoingAction = false;

    }

    public void StopBeginDamaged() 
    {
        bossController.isDamaged = false;
    }

    public void BossDead() 
    {
        BelethUIController endGameControll = FindObjectOfType<BelethUIController>();

        endGameControll.QuitGame();
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<BelethHealthController>().GetDamage(1);
        }
    }
}
