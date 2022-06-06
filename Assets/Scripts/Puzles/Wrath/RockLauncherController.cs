using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockLauncherController : MonoBehaviour
{
    [SerializeField]
    private Transform rockStarterPos;

    [SerializeField]
    private GameObject rockPrefab;


    public void LauchRock() 
    {
        GameObject thisRock;
        thisRock = Instantiate(rockPrefab, rockStarterPos);
        Destroy(thisRock, 6);
    
    }


   


}
