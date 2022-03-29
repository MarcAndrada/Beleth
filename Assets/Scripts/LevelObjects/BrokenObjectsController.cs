using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenObjectsController : MonoBehaviour
{
    Rigidbody rb;
    BoxCollider boxColl;

    float timer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        boxColl = GetComponent<BoxCollider>();
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
            Destroy(rb);
            Destroy(boxColl);

            if(timer > 4) Destroy(gameObject);
        }  
    }
}
