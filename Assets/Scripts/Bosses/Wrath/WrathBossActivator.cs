using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WrathBossActivator : MonoBehaviour
{
    [SerializeField]
    private WrathBossStateController boss;
    [SerializeField]
    GameObject BossCanvas;

    [Header("Door")]
    [SerializeField]
    private DoorController door;

    private BelethHealthController playerHealth;

    private void Start()
    {
        playerHealth = boss.player.GetComponent<BelethHealthController>();


        BossCanvas.SetActive(false);

        boss.rocksManager.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        if (playerHealth.GetHealthPoints() <= 0)
        {
            PlayerExit();
        }

        //if (doorClosing)
        //{
        //    doorCloseState += doorCloseSpeed/100 * Time.deltaTime;
        //    doorRigidBody.MovePosition(Vector3.Lerp(door.transform.position, new Vector3(doorStarterPoint.x, doorStarterPoint.y, doorStarterPoint.z), doorCloseState));
        //    if (doorCloseState >= 1)
        //    {
        //        doorClosing = false;
        //    }
        //}

    }

    private void PlayerEnter() 
    {
        boss.StartFight();
        BossCanvas.SetActive(true);
        SoundManager._SOUND_MANAGER.ChangeMusicBoss();
        door.CloseDoor();
    }

    public void PlayerExit()
    {
        boss.StopFight();
        //doorRigidBody.MovePosition(new Vector3(doorStarterPoint.x, doorStarterPoint.y - 30, doorStarterPoint.z)); 
        Debug.Log("Sale");
        BossCanvas.SetActive(false);
        SoundManager._SOUND_MANAGER.ChangeMusicLevel();
        door.OpenDoor();


    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            PlayerEnter();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            PlayerExit();
        }
    }
}
