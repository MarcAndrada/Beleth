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
    private GameObject door;
    private BelethHealthController playerHealth;

    private void Start()
    {
        BossCanvas.SetActive(false);
        playerHealth = boss.player.GetComponent<BelethHealthController>();

    }

    // Update is called once per frame
    void Update()
    {
        if (playerHealth.GetHealthPoints() <= 0)
        {
            PlayerExit();
        }
    }

    public void PlayerExit()
    {
        StartCoroutine(boss.StopFight());
        door.transform.position = new Vector3(door.transform.position.x, -30 , door.transform.position.z);
        Debug.Log("Sale");
        boss.player.GetComponentInChildren<BelethAudioController>().soundCont.ChangeMusicLevel();

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            StartCoroutine(boss.StartFight());
            BossCanvas.SetActive(true);
            door.transform.position = new Vector3(door.transform.position.x, 0, door.transform.position.z);
            boss.player.GetComponentInChildren<BelethAudioController>().soundCont.ChangeMusicBoss();

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            PlayerExit();
            BossCanvas.SetActive(false);
        }
    }
}
