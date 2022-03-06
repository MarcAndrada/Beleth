using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IraManager : MonoBehaviour
{
    [SerializeField]
    bool wrathActive;

    [SerializeField]
    float timeToExplode;

    [SerializeField]
    float radius;

    [SerializeField]
    float strenght;

    [SerializeField]
    float upStrenght;

    void Start()
    {
        wrathActive = false;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("g"))
        {
            if (!wrathActive)
            {

                wrathActive= true;
            }

            else
            {
                WrathExplosion();
                wrathActive = false;
            }
        }
    }


    void WrathExplosion()
    {
        Vector3 explosionPosition = gameObject.transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPosition, radius);

        foreach(Collider hit in colliders)
        {

            Rigidbody rb = hit.GetComponent<Rigidbody>();

            if (rb != null)
            {
                rb.AddExplosionForce(strenght, explosionPosition, radius, upStrenght, ForceMode.Impulse);
            }
        }
        

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
