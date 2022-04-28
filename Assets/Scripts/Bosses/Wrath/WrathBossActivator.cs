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
    private GameObject door;
    private BelethHealthController playerHealth;
    private Vector3 doorStarterPoint;
    private Rigidbody doorRigidBody;
    private bool doorClosing = false;
    private float doorCloseState = 0;
    [SerializeField]
    private float doorCloseSpeed;

    private void Awake()
    {
        doorRigidBody = door.GetComponent<Rigidbody>();

    }

    private void Start()
    {
        playerHealth = boss.player.GetComponent<BelethHealthController>();


        BossCanvas.SetActive(false);
        doorStarterPoint = door.transform.position;

        boss.rocksManager.SetActive(false);
        doorClosing = false;
        doorRigidBody.MovePosition(new Vector3(doorStarterPoint.x, doorStarterPoint.y - 30, doorStarterPoint.z));
        doorCloseState = 0;

    }

    // Update is called once per frame
    void Update()
    {
        if (playerHealth.GetHealthPoints() <= 0)
        {
            PlayerExit();
        }

        if (doorClosing)
        {
            doorCloseState += doorCloseSpeed/100 * Time.deltaTime;
            doorRigidBody.MovePosition(Vector3.Lerp(door.transform.position, new Vector3(doorStarterPoint.x, doorStarterPoint.y, doorStarterPoint.z), doorCloseState));
            if (doorCloseState >= 1)
            {
                doorClosing = false;
            }
        }

    }

    public void PlayerExit()
    {
        doorClosing = false;
        StartCoroutine(boss.StopFight());
        doorRigidBody.MovePosition(new Vector3(doorStarterPoint.x, doorStarterPoint.y - 30, doorStarterPoint.z)); 
        Debug.Log("Sale");
        SoundManager._SOUND_MANAGER.ChangeMusicLevel();
        doorCloseState = 0;

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            StartCoroutine(boss.StartFight());
            BossCanvas.SetActive(true);
            doorRigidBody.MovePosition(doorStarterPoint);
            SoundManager._SOUND_MANAGER.ChangeMusicBoss();
            doorClosing = true;

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
