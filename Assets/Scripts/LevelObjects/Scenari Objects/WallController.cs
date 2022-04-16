using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallController : MonoBehaviour
{
    [SerializeField]
    GameObject BrokeWall;
    [SerializeField]
    GameObject existingBrokeWall;

    bool can;

    public void Break()
    {
        if (!can)
        {
            if (existingBrokeWall != null)
            {
                existingBrokeWall.SetActive(true);
            }
            else
            {
                Instantiate(BrokeWall);
            }            
            can = true;
            Destroy(gameObject);
        }
    }
}
