using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenObjectsController : MonoBehaviour
{
    Rigidbody rigidbody;
    BoxCollider collider;

    float timer;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        collider = GetComponent<BoxCollider>();
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
        if (timer > 2)
        {
            Destroy(rigidbody);
            Destroy(collider);

            if(timer > 4) Destroy(gameObject);
        }  
    }
}
