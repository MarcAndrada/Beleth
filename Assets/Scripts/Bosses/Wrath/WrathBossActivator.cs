using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WrathBossActivator : MonoBehaviour
{
    [SerializeField]
    private WrathBossStateController boss;
    [SerializeField]
    GameObject BossCanvas;
    [SerializeField]
    private GameObject barrier;
    [Header("Door")]
    [SerializeField]
    private DoorController door;

    [Header("Cinematics")]
    [SerializeField]
    private string cinematicsParameter;
    private bool cinematicPlayed = false;
    [SerializeField]
    private Collider cinematicCollider;

    private BelethHealthController playerHealth;

    private void Start()
    {
        playerHealth = boss.player.GetComponent<BelethHealthController>();


        BossCanvas.SetActive(false);

        boss.rocksManager.SetActive(false);
        barrier.SetActive(false);
        

    }

    // Update is called once per frame
    void Update()
    {
        if (playerHealth.GetHealthPoints() <= 0)
        {
            PlayerExit();
        }

    }

    void LateUpdate() 
    {
        if (playerHealth.GetHealthPoints() <= 0)
        {
            PlayerExit();
        }
    }
    private void PlayerEnter() 
    {
        boss.StartFight();
        BossCanvas.SetActive(true);
        SoundManager._SOUND_MANAGER.ChangeMusicBoss();
        door.CloseDoor();
        barrier.SetActive(true);
        
    }

    public void PlayerExit()
    {
        boss.StopFight();
        BossCanvas.SetActive(false);
        SoundManager._SOUND_MANAGER.ChangeMusicLevel();
        door.OpenDoor();
        barrier.SetActive(false);
        cinematicPlayed = false;
        cinematicCollider.enabled = true;

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && cinematicPlayed)
        {
            PlayerEnter();
        }
        else if (other.gameObject.tag == "Player" && !cinematicPlayed)
        {
            CinematicsController._CINEMATICS_CONTROLLER.PlaySpecificCinematic(cinematicsParameter);
            cinematicPlayed = true;
            cinematicCollider.enabled = false;
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
