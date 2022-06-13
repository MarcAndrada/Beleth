using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneWallController : MonoBehaviour
{

    [SerializeField]
    private Rigidbody[] rocks;
    [SerializeField]
    private float moveForce;

    private bool goInFront = true;


    public void ShakeWall() 
    {
        foreach (var item in rocks)
        {
            item.isKinematic = false;
            item.gameObject.transform.parent = null;
            if (goInFront)
            {
                item.AddForce(transform.forward * moveForce, ForceMode.Impulse);
                goInFront = false;
            }
            else
            {
                item.AddForce(-transform.forward * moveForce, ForceMode.Impulse);
                goInFront = true;
            }

            item.GetComponent<SimplyDestroyController>().enabled = true;
        }

        Destroy(gameObject);
    }
}
