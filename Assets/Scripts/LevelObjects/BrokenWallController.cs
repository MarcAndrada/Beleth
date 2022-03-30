using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenWallController : MonoBehaviour
{
    [Header("Explosion Variables")]
    [SerializeField]
    float radius;
    [SerializeField]
    float force;

    [Header("GameObjects")]
    [SerializeField]
    GameObject hole;
    [SerializeField]
    GameObject brokenObject;
    [SerializeField]
    GameObject wrathRock;
    
    public void Break()
    {   
        Instantiate(hole, new Vector3(transform.position.x, transform.position.y - 1.7f , transform.position.z), Quaternion.Euler(0,0,0));

        Instantiate(brokenObject, new Vector3(transform.position.x + 2.7f, transform.position.y + .3f, transform.position.z), Quaternion.Euler(0, 0, 0));

        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
        foreach (Collider hit in colliders)
        {
            Rigidbody fragmentrb = hit.GetComponent<Rigidbody>();

            if (fragmentrb != null)
                fragmentrb.AddExplosionForce(force, transform.position, radius);
        }

        int i = 1;
        if (Random.Range(0, 5) == i) Instantiate(wrathRock, new Vector3(transform.position.x, transform.position.y, transform.position.z), transform.rotation);

        Destroy(gameObject);
    }
}
