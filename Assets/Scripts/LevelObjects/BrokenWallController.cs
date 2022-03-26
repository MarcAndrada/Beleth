using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenWallController : MonoBehaviour
{
    [SerializeField]
    GameObject hole;
    [SerializeField]
    GameObject brokenWall;
    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void OnDestroy()
    {
        Vector3 explosionPos = transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPos, 5);
        foreach (Collider hit in colliders)
        {
            Rigidbody fragmentrb = hit.GetComponent<Rigidbody>();

            if (fragmentrb != null)
                rb.AddExplosionForce(100f, explosionPos, 5f, 10f);
        }
    }

    public void Break()
    {   
        Instantiate(hole, new Vector3(transform.position.x, transform.position.y - 1.5f , transform.position.z), Quaternion.Euler(0,0,0));
        Instantiate(brokenWall, new Vector3(transform.position.x, transform.position.y + .6f, transform.position.z), transform.rotation);
        Destroy(gameObject);
    }
}
