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
    [SerializeField]
    double timesToPlay;
    double timeToStop;
    float timer;

    enum InputActions {JUMP, ATTACK, WRATH_ATTACK, ACTIVATE_WRATH , GLIDE };
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

    // Start is called before the first frame update
    void Start()
    {
        timeToStop = videoPlayer.length * timesToPlay;
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if(timer>= timeToStop)
        {
            timer = 0;
            gameObject.SetActive(false);
        }
    }

    public void SetThings()
    {
        timeToStop = videoPlayer.length * timesToPlay;

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
                default:
                    break;
            }
        }
        

    }
}
