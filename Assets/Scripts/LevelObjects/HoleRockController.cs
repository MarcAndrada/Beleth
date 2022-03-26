using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoleRockController : MonoBehaviour
{
    Transform transform;
    float timer;

    private void Awake()
    {
        transform = GetComponent<Transform>();
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
            transform.Translate(Vector3.down * 1 * Time.deltaTime, Space.World);

            if(timer > 4) Destroy(gameObject);
        }
    }
}
