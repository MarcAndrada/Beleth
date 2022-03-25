using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BelethStaminaUIController : MonoBehaviour
{

    private BelethUIController uiController;

    private void Start()
    {
        uiController = GetComponentInParent<BelethUIController>();
    }

    public void PlayDown() 
    {
        uiController.PlayDown();
    
    }

    public void EmptyAnim() 
    {
        uiController.EmptyAnim();
    }
}
