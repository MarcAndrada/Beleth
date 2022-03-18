using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenWallController : MonoBehaviour
{

    [SerializeField]
    GameObject brokenWall;
    [SerializeField]
    float timeToDestroy;
    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    private void OnDestroy()
    {
        Vector3 explosionPos = transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPos, 5);
        foreach (Collider hit in colliders)
        {
            Rigidbody fragmentrb = hit.GetComponent<Rigidbody>();
            if (fragmentrb != null)
                rb.AddExplosionForce(10, explosionPos, 5, 2.0f);
        }


    }

    public void Break()
    {
     GameObject broken= Instantiate(brokenWall, transform.position, transform.rotation);
        Destroy(broken, timeToDestroy);
      Destroy(gameObject);

    }
}
