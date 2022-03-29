using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WrathBossActivator : MonoBehaviour
{
    [SerializeField]
    private WrathBossStateController boss;

    private BelethHealthController playerHealth;
    // Start is called before the first frame update
    void Awake()
    {
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

    private void PlayerExit()
    {
        StartCoroutine(boss.StopFight());
        Debug.Log("Sale");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            StartCoroutine(boss.StartFight());
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
