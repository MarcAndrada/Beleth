using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivatorsController : MonoBehaviour
{
    [SerializeField]
    private FourArmEnemieController fourArmsController;

    [SerializeField]
    private int armIndex;

    private float timeToWaitGoDown;

    private bool active = false;
    private float timeWaited = 0;


    private void Update()
    {
        WaitForArmGoDown();
    }

    private void WaitForArmGoDown() 
    {
        if (active)
        {
            timeWaited += Time.deltaTime;
            if (timeWaited >= timeToWaitGoDown)
            {
                active = false;
                timeWaited = 0;
                fourArmsController.DesactivateArm(armIndex);
            }

        }
    }

    public void ActivateCurrentArm() 
    {
        if (!active)
        {
            timeToWaitGoDown = fourArmsController.ActivateArm(armIndex);

            active = true;
        }
       
    }
}
