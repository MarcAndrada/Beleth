using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialBridgeController : MonoBehaviour
{

    [SerializeField]
    private GameObject bridge;

    private int activatorsUsed = 0;
    private bool bridgeDown = false;


    private void LateUpdate() 
    {

        if (activatorsUsed >= 2 && !bridgeDown)
        {
            BothActivatorsActivated();
        }
        else
        {
            activatorsUsed = 0;
        }


    }

    public void ActivatorUsed() 
    {
        activatorsUsed++;
    }

    private void BothActivatorsActivated() 
    {
        bridge.GetComponent<Animation>().Play();
        bridgeDown = true;
    }


}
