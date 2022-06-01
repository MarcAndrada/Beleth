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
    private Slider mouseXSlider;
    private Slider mouseYSlider;
    private Text mouseXText;
    private Text mouseYText;
    private float mouseSpeedXDefault = 75;
    private float mouseSpeedYDefault = 75;
    public float mouseSpeedX;
    public float mouseSpeedY;


    public enum ControllerType { KEYBOARD, PLAYSTATION, XBOX };
    public ControllerType controllerType;
    [SerializeField]
    private string controllerTypeParameter;
    private int controllerTypeDefault = 0;

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

        if (!PlayerPrefs.HasKey(mouseSpeedXParameter)) {
            PlayerPrefs.SetFloat(mouseSpeedXParameter, mouseSpeedXDefault);
        }

        if (!PlayerPrefs.HasKey(mouseSpeedYParameter))
        {
            PlayerPrefs.SetFloat(mouseSpeedYParameter, mouseSpeedYDefault);
        }


        mouseSpeedX = PlayerPrefs.GetFloat(mouseSpeedXParameter);
        mouseSpeedY = PlayerPrefs.GetFloat(mouseSpeedYParameter);

        if (!PlayerPrefs.HasKey(controllerTypeParameter))
        {
            PlayerPrefs.SetInt(controllerTypeParameter, controllerTypeDefault);
        }

        switch (PlayerPrefs.GetInt(controllerTypeParameter))
        {
            case 0:
                controllerType = ControllerType.KEYBOARD;
                break;
            case 1:
                controllerType = ControllerType.PLAYSTATION;
                break;
            case 2:
                controllerType = ControllerType.XBOX;

                break;
            default:
                break;
        }

    }

    #region Mouse Speed

    private void HandleSliderValueChangedXSpeed(float _value)
    {
        PlayerPrefs.SetFloat(mouseSpeedXParameter, mouseXSlider.value);
        PlayerPrefs.Save();

        mouseSpeedX = mouseXSlider.value;
        mouseXText.text = mouseSpeedX.ToString();
    }
    private void HandleSliderValueChangedYSpeed(float _value)
    {
        PlayerPrefs.SetFloat(mouseSpeedYParameter, mouseYSlider.value);
        PlayerPrefs.Save();

        mouseSpeedY = mouseYSlider.value;
        mouseYText.text = mouseSpeedY.ToString();

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

    #region Controller Type

    public void ChangeInputType(int _selectedType) 
    {
        switch (_selectedType)
        {
            case 0:
                controllerType = ControllerType.KEYBOARD;
                break;
            case 1:
                controllerType = ControllerType.PLAYSTATION;
                break;
            case 2:
                controllerType = ControllerType.XBOX;

                break;
            default:
                break;
        }

        PlayerPrefs.SetInt(controllerTypeParameter, _selectedType);

    }

    public int GetControlsType() 
    {
        int controlType = 0;

        switch (controllerType)
        {
            case ControllerType.KEYBOARD:
                controlType = 0;
                break;
            case ControllerType.PLAYSTATION:
                controlType = 1;
                break;
            case ControllerType.XBOX:
                controlType = 2;
                break;
            default:
                break;
        }


        return controlType;
    }

    #endregion
}
