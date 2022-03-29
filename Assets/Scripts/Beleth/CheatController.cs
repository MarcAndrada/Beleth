using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheatController : MonoBehaviour
{
    GameObject[] checkPoints;
    CharacterController charCont;

    int index = 0;
    
    
    // Start is called before the first frame update
    void Awake()
    {
        checkPoints = GameObject.FindGameObjectsWithTag("CheckPoints");
        charCont = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            charCont.enabled = false;
            transform.position = checkPoints[index].transform.position;
            charCont.enabled = true;
            index++;
            if (index >= checkPoints.Length)
            {
                index = 0;
            }
        }
    }
}
