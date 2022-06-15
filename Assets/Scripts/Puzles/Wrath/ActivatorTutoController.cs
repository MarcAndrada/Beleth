using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivatorTutoController : MonoBehaviour
{

    [SerializeField]
    private TutorialBridgeController controller;


    public void ActivatorUsed() 
    {
        controller.ActivatorUsed();
    }
}
