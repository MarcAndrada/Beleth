using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MouseSpeedConfigurationController : MonoBehaviour
{
    [SerializeField]
    private Slider mouseXSlider;
    [SerializeField]
    private Slider mouseYSlider;
    [SerializeField]
    private Text mouseXText;
    [SerializeField]
    private Text mouseYText;
        

    // Start is called before the first frame update
    void Start()
    {
        SettingsController._SETTINGS_CONTROLLER.ChangeMouseSpeedSliders(mouseXSlider, mouseYSlider, mouseXText, mouseYText);
    }


}
