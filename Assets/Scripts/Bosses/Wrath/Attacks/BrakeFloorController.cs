using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrakeFloorController : MonoBehaviour
{

    [SerializeField]
    private float speed;
    [SerializeField]
    private float attackDuration;

    private void Start()
    {
        StartCoroutine(DestroyObject());  
    }
    void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;


    }

    IEnumerator DestroyObject()
    {
        yield return new WaitForSeconds(attackDuration);

        Destroy(gameObject);
    }
}
