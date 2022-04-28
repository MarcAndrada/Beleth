using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingrockController : MonoBehaviour
{
    Rigidbody rb;
    GameObject collisionMark;
    MeteorFragmentController breakeControll;

    [SerializeField]
    float fallingSpeed;
    [SerializeField]
    float rotationSpeed;
    [SerializeField]
    GameObject collisionMarkPrefab;
    [SerializeField]
    private LayerMask floorLayer;
    [Header("VFX")]
    [SerializeField]
    ParticleSystem[] explosion;

    RaycastHit startRay;

    private void Awake()
    {
        breakeControll = GetComponent<MeteorFragmentController>();
        rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        rb.velocity = Vector3.up * fallingSpeed * -1;
      

        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.up * -1), out startRay, /*Mathf.infinty*/ 100, floorLayer))
        {
            Vector3 pos = startRay.point;
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
        if (collision.gameObject.layer == 3)
        {
            breakeControll.Break(startRay.point);

            foreach (var item in explosion)
            {
                Instantiate(item, transform.position, Quaternion.Euler(0, 0, 0));
            }

            Destroy(gameObject);
        }

        if (collision.gameObject.tag == "Player")
        {
            breakeControll.BreakOnPlayerHead();

            foreach (var item in explosion)
            {
                Instantiate(item, transform.position, Quaternion.Euler(0, 0, 0));
            }

            collision.gameObject.GetComponent<BelethHealthController>().GetDamage(1);
            Destroy(gameObject);
        }
    }
}
