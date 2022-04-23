using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheatController : MonoBehaviour
{
    GameObject[] checkPoints;
    int index = 0;
    
    
    // Start is called before the first frame update
    void Awake()
    {
        checkPoints = GameObject.FindGameObjectsWithTag("CheckPoints");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            transform.position = checkPoints[index].transform.position;
            index++;
            if (index >= checkPoints.Length)
            {
                index = 0;
            }
        }
    }
}
