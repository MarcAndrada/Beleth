using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class TutorialCanvasController : MonoBehaviour
{
    [SerializeField]
    TutorialCanvas m_Canvas;
    [SerializeField]
    VideoPlayer videoPlayer;
    [SerializeField]
    Text text;


    enum InputActions {JUMP, ATTACK, WRATH_ATTACK, ACTIVATE_WRATH , GLIDE, ACTIVATE_ONE_WRATH};
    [SerializeField]
    InputActions[] inputActions;

    [SerializeField]
    private Image[] controllsImages;

    [SerializeField]
    private Sprite[] jumpSprites;
    [SerializeField]
    private Sprite[] attackSprites;
    [SerializeField]
    private Sprite[] wrathAttackSprites;
    [SerializeField]
    private Sprite[] activateWrathSprites;
    [SerializeField]
    private Sprite[] activateOneWrathSprites;


    public void SetThings()
    {
        for (int i = 0; i < inputActions.Length; i++)
        {
            switch (inputActions[i])
            {
                case InputActions.JUMP:
                    controllsImages[i].sprite = jumpSprites[SettingsController._SETTINGS_CONTROLLER.GetControlsType()]; 
                    break;
                case InputActions.ATTACK:
                    controllsImages[i].sprite = attackSprites[SettingsController._SETTINGS_CONTROLLER.GetControlsType()];
                    break;
                case InputActions.WRATH_ATTACK:
                    controllsImages[i].sprite = wrathAttackSprites[SettingsController._SETTINGS_CONTROLLER.GetControlsType()];
                    break;
                case InputActions.ACTIVATE_WRATH:
                    controllsImages[i].sprite = activateWrathSprites[SettingsController._SETTINGS_CONTROLLER.GetControlsType()];
                    break;
                case InputActions.ACTIVATE_ONE_WRATH:
                    controllsImages[i].sprite = activateOneWrathSprites[SettingsController._SETTINGS_CONTROLLER.GetControlsType()];
                    break;
                default:
                    break;
            }
        }
        

    }
}
