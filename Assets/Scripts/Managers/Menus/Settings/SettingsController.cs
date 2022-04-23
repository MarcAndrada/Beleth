using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsController : MonoBehaviour
{
    public static SettingsController _SETTINGS_CONTROLLER;

    [Header("Mouse Speed")]
    [SerializeField]
    private string mouseSpeedXParameter;
    [SerializeField]
    private string mouseSpeedYParameter;
    [SerializeField]
    private Slider mouseXSlider;
    [SerializeField]
    private Slider mouseYSlider;
    [SerializeField]
    private Text mouseXText;
    [SerializeField]
    private Text mouseYText;
    
    public float mouseSpeedX;
    public float mouseSpeedY;



    void Awake()
    {
        if (SettingsController._SETTINGS_CONTROLLER != null)
        {
            Destroy(gameObject);

        }
        else
        {
            _SETTINGS_CONTROLLER = this;

        }

        mouseSpeedX = PlayerPrefs.GetFloat(mouseSpeedXParameter);
        mouseSpeedY = PlayerPrefs.GetFloat(mouseSpeedYParameter);

    }

    #region Mouse Speed

    private void HandleSliderValueChangedXSpeed(float _value)
    {
        PlayerPrefs.SetFloat(mouseSpeedXParameter, mouseXSlider.value);
        PlayerPrefs.Save();

        mouseSpeedX = mouseXSlider.value;
        mouseXText.text = mouseXSlider.value.ToString();
    }
    private void HandleSliderValueChangedYSpeed(float _value)
    {
        PlayerPrefs.SetFloat(mouseSpeedYParameter, mouseYSlider.value);
        PlayerPrefs.Save();

        mouseSpeedY = mouseYSlider.value;
        mouseYText.text = mouseYSlider.value.ToString(); ;

    }

    public void ChangeMouseSpeedSliders(Slider _xSpeedSlider, Slider _ySpeedSlider, Text _xText, Text _yText) 
    {
        mouseXSlider = _xSpeedSlider;
        mouseYSlider = _ySpeedSlider;

        mouseXText = _xText;
        mouseYText = _yText;

        mouseXSlider.onValueChanged.AddListener(HandleSliderValueChangedXSpeed);
        mouseYSlider.onValueChanged.AddListener(HandleSliderValueChangedYSpeed);

        mouseXSlider.value = PlayerPrefs.GetFloat(mouseSpeedXParameter, mouseXSlider.value);
        mouseYSlider.value = PlayerPrefs.GetFloat(mouseSpeedYParameter, mouseYSlider.value);

        

        mouseXText.text = mouseXSlider.value.ToString();
        mouseYText.text = mouseYSlider.value.ToString();

    }


    #endregion


}
