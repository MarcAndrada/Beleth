using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoleRockController : MonoBehaviour
{
    Rigidbody rb;
    BoxCollider boxcollider;
    Transform transform;
    float timer;

    private void Awake()
    {
        transform = GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();
        boxcollider = GetComponent<BoxCollider>();
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
        if (timer > 2 && rb == null && boxcollider == null)
        {
            transform.Translate(Vector3.down * 1 * Time.deltaTime, Space.World);

            if(timer > 4) Destroy(gameObject);
        }
        else if (timer > 3)
        {
            rb.isKinematic = true;
            boxcollider.enabled = false;

            transform.Translate(Vector3.down * 1 * Time.deltaTime, Space.World);

            if (timer > 4) Destroy(gameObject);
        }
    }
}
