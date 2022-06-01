using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlsMenuController : MonoBehaviour
{
    [SerializeField]
    private Image[] controlsImages;

    [SerializeField]
    private Sprite[] movementSprites;
    [SerializeField]
    private Sprite[] jumpSprites;
    [SerializeField]
    private Sprite[] walkSprites;
    [SerializeField]
    private Sprite[] pauseSprites;
    [SerializeField]
    private Sprite[] attackSprites;
    [SerializeField]
    private Sprite[] wrathAttackSprites;
    [SerializeField]
    private Sprite[] wrathActivationOneSprites;
    [SerializeField]
    private Sprite[] wrathActivationAllSprites;
    [SerializeField]
    private Sprite[] moveLeftWrathListSprites;
    [SerializeField]
    private Sprite[] moveRightWrathListSprites;
    // Start is called before the first frame update
    void Start()
    {
        RefreshValues();



    }

    private void RefreshValues()
    {
        int currentControllerType = -1;

        switch (SettingsController._SETTINGS_CONTROLLER.controllerType)
        {
            case SettingsController.ControllerType.KEYBOARD:
                currentControllerType = 0;
                break;
            case SettingsController.ControllerType.PLAYSTATION:
                currentControllerType = 1;
                break;
            case SettingsController.ControllerType.XBOX:
                currentControllerType = 2;
                break;
            default:
                break;
        }


        controlsImages[0].sprite = movementSprites[currentControllerType];
        controlsImages[1].sprite = jumpSprites[currentControllerType];
        controlsImages[2].sprite = walkSprites[currentControllerType];
        controlsImages[3].sprite = pauseSprites[currentControllerType];
        controlsImages[4].sprite = attackSprites[currentControllerType];
        controlsImages[5].sprite = wrathAttackSprites[currentControllerType];
        controlsImages[6].sprite = wrathActivationOneSprites[currentControllerType];
        controlsImages[7].sprite = wrathActivationAllSprites[currentControllerType];
        controlsImages[8].sprite = moveLeftWrathListSprites[currentControllerType];
        controlsImages[9].sprite = moveRightWrathListSprites[currentControllerType];



    }

    public void ChangeValue(int _value) 
    {
        SettingsController._SETTINGS_CONTROLLER.ChangeInputType(_value);
        RefreshValues();
    }
}
