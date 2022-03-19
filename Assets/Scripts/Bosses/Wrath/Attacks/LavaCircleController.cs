using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaCircleController : MonoBehaviour
{
    [SerializeField]
    private int playerDamage;

    public void DestroyObject() {
        Destroy(gameObject);
    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "player")
        {
            other.gameObject.GetComponent<BelethHealthController>().GetDamage(playerDamage);
        }
    }


}

