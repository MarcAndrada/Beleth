using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallController : MonoBehaviour
{
    [SerializeField]
    GameObject BrokeWall;

    bool can;

    public void Break()
    {
        if (!can)
        {
            Instantiate(BrokeWall);
            Destroy(gameObject);
            can = true;
        }
    }
}
