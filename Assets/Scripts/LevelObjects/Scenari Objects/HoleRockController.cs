using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoleRockController : MonoBehaviour
{
    [SerializeField]
    private float timeToWait;
    Rigidbody rb;
    BoxCollider boxcollider;
    float timer;

    private void Awake()
    {
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
        if (timer > timeToWait && rb == null && boxcollider == null)
        {
            transform.Translate(Vector3.down * 1 * Time.deltaTime, Space.World);

            if(timer > timeToWait + 3) Destroy(gameObject);
        }
        else if (timer > timeToWait)
        {
            rb.isKinematic = true;
            boxcollider.enabled = false;

            transform.Translate(Vector3.down * 1 * Time.deltaTime, Space.World);

            if (timer > timeToWait + 3) Destroy(gameObject);
        }
    }
}
