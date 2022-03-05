using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionController : MonoBehaviour
{
    private SphereCollider explosionColl;
    // Start is called before the first frame update
    void Start()
    {
        explosionColl = GetComponent<SphereCollider>();

        StartCoroutine(StopCollision());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator StopCollision() { 

        yield return new WaitForSeconds(0.4f);
        explosionColl.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("Toma daño puto");
            other.GetComponent<BelethHealthController>().GetDamage(1);
        }
    }
}
