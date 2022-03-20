using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingrockController : MonoBehaviour
{
    Rigidbody rb;
    GameObject collisionMark;
    BrokenWallController breakeControll;

    [SerializeField]
    float fallingSpeed;
    [SerializeField]
    float rotationSpeed;
    [SerializeField]
    GameObject collisionMarkPrefab;

    private void Awake()
    {
        breakeControll = GetComponent<BrokenWallController>();
        rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        rb.velocity = Vector3.up * fallingSpeed * -1;
      
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.up * -1), out hit, Mathf.Infinity))
        {
            Vector3 pos = hit.point;
            collisionMark = Instantiate(collisionMarkPrefab, pos, Quaternion.identity);
        }
    }

    private void OnDestroy()
    {
        Destroy(collisionMark);
        FallingrockManager.actualRocks--;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Scenario")
        {
            breakeControll.Break();
            Destroy(gameObject);
        }
    }
}
