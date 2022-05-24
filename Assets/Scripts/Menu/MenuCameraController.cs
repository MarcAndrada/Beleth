using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCameraController : MonoBehaviour
{

    [SerializeField]
    private GameObject mainMenuCanvas;
    [SerializeField]
    private GameObject settingsCanvas;
    [SerializeField]
    private GameObject creditsCanvas;
    [SerializeField]
    private GameObject controlsCanvas;

    private int cameraIndex;


    private Animator cameraAnimator;

    private void Awake()
    {
        cameraAnimator = GetComponent<Animator>();
    }


    // Start is called before the first frame update
    void Start()
    {
        cameraIndex = 0;

        mainMenuCanvas.SetActive(true);
        settingsCanvas.SetActive(false);
        creditsCanvas.SetActive(false);
        controlsCanvas.SetActive(false);
    }

    private void Update()
    {
        cameraAnimator.SetInteger("Index", cameraIndex);
    }
    public void GoMainMenuAngle()
    {
        cameraIndex = 0;

        mainMenuCanvas.SetActive(false);
        settingsCanvas.SetActive(false);
        creditsCanvas.SetActive(false);
        controlsCanvas.SetActive(false);
    }

    public void GoSettingsAngle()
    {
        cameraIndex = 1;

        mainMenuCanvas.SetActive(false);
        settingsCanvas.SetActive(false);
        creditsCanvas.SetActive(false);
        controlsCanvas.SetActive(false);
    }

    public void GoCreditsAngle()
    {
        cameraIndex = 2;

        mainMenuCanvas.SetActive(false);
        settingsCanvas.SetActive(false);
        creditsCanvas.SetActive(false);
        controlsCanvas.SetActive(false);
    }

    public void GoControllsAngle()
    {
        cameraIndex = 3;

        mainMenuCanvas.SetActive(false);
        settingsCanvas.SetActive(false);
        creditsCanvas.SetActive(false);
        controlsCanvas.SetActive(false);

    }

    public void ChangeCurrentIndex(int _currentIndex) 
    {
        cameraAnimator.SetInteger("CurrentIndex", _currentIndex);

    }

    public void ActivateCanvas(int _canvasToActive) 
    {
        switch (_canvasToActive)
        {
            case 0:
                mainMenuCanvas.SetActive(true);
                break;
            case 1:
                settingsCanvas.SetActive(true);
                break;
            case 2:
                creditsCanvas.SetActive(true);
                break;
            case 3:
                controlsCanvas.SetActive(true);
                break;
            default:
                break;
        }

    }



}
