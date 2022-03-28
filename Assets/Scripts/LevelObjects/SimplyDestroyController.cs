using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimplyDestroyController : MonoBehaviour
{
    [SerializeField]
    float timeToDie;
    float timer;

    // Start is called before the first frame update
    void Start()
    {
        timer = 0;    
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > timeToDie) Destroy(gameObject);
    }
}
