using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoleRockController : MonoBehaviour
{
    [SerializeField]
    private float timeToWait;
    [SerializeField]
    private float timeOffset;
    Rigidbody rb;
    MeshCollider coll;
    float timer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        coll = GetComponent<MeshCollider>();
    }

    // Start is called before the first frame update
    void Start()
    {
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > timeToWait && rb == null && coll == null)
        {
            transform.Translate(Vector3.down * 1 * Time.deltaTime, Space.World);

            if(timer > timeToWait + timeOffset) Destroy(gameObject);
        }
        else if (timer > timeToWait)
        {
            rb.isKinematic = true;
            coll.enabled = false;

            transform.Translate(Vector3.down * 3 * Time.deltaTime, Space.World);

            if (timer > timeToWait + timeOffset) Destroy(gameObject);
        }
    }
}
