using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivatorController : MonoBehaviour
{
    private enum ActivatorType { NONE, PRINCIPAL, LEFT, RIGHT }
    [SerializeField]
    private SerpiController serpiController;

    [SerializeField]
    private ActivatorType currentType = ActivatorType.NONE;

   public void ActivatorUsed() 
   {
        if (currentType == ActivatorType.PRINCIPAL)
        {
            serpiController.SerpiDead();
        }
        else if(currentType == ActivatorType.LEFT)
        {
            if (serpiController.currentSerpiState != SerpiController.SerpiStates.TRAPPED && serpiController.currentSerpiState != SerpiController.SerpiStates.DEAD)
            {
                serpiController.ActivateChain(0);
            }
        }
        else if(currentType == ActivatorType.RIGHT)
        {
            if (serpiController.currentSerpiState != SerpiController.SerpiStates.TRAPPED && serpiController.currentSerpiState != SerpiController.SerpiStates.DEAD)
            {
                serpiController.ActivateChain(1);
            }

        }

    }
}
