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
            serpiController.ActivateChain(0);
        }
        else if(currentType == ActivatorType.RIGHT)
        {
            serpiController.ActivateChain(1);

        }

    }
}
