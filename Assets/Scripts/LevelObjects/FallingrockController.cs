using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingrockController : MonoBehaviour
{
    Rigidbody rb;
    Transform transform;
    GameObject collisionMark;
    BrokenWallController breakeControll;

    [SerializeField]
    float fallingSpeed;
    [SerializeField]
    float rotationSpeed;
    [SerializeField]
    GameObject collisionMarkPrefab;

    [Header("VFX")]
    [SerializeField]
    ParticleSystem[] explosion;

    private void Awake()
    {
        breakeControll = GetComponent<BrokenWallController>();
        transform = GetComponent<Transform>();  
        rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        rb.velocity = Vector3.up * fallingSpeed * -1;
      
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.up * -1), out hit, /*Mathf.infinty*/ 100))
        {
            Vector3 pos = hit.point;
            collisionMark = Instantiate(collisionMarkPrefab, new Vector3(pos.x, pos.y + 0.1f, pos.z), Quaternion.identity);
        }
    }

    private void Update()
    {
        transform.Rotate(Vector3.forward, 100 * Time.deltaTime);
        transform.Rotate(Vector3.right, 50.3f * Time.deltaTime);
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

            foreach (var item in explosion)
            {
                Instantiate(item, transform.position, Quaternion.Euler(0, 0, 0));
            }

            Destroy(gameObject);
        }
    }
}
