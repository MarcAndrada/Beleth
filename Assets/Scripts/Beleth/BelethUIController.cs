using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public class BelethUIController : MonoBehaviour
{
    [Header("Canvas")]
    [SerializeField]
    GameObject deathCanvas;
    [SerializeField]
    GameObject pauseCanvas;
    [SerializeField]
    GameObject settingsCanvas;
    [SerializeField]
    private GameObject controlsCanvas;
    [SerializeField]
    private CameraSpeedController cameraSpeedController;

    [Header("Stamina Bar")]
    [SerializeField]
    Slider slider;
    [SerializeField]
    Animator staminaAnimator;

    [Header("HUD")]
    [SerializeField]
    GameObject[] hearts;
    [SerializeField]
    private GameObject[] emptyHearts;
    [SerializeField]
    private GameObject[] collectables;
    [SerializeField]
    private TextMeshProUGUI coinText;
    private int collectableIndex;


    [SerializeField]
    private GameObject wrathBar;
    [SerializeField]
    private float timeToWaitWrathBar;
    private float timeWaitedWrathBar = 0;
    [SerializeField]
    private GameObject[] wrathUiPointers;
    [SerializeField]
    private Sprite[] wrathUISprites;
    [SerializeField]
    private Image[] wrathUIImages;
    public int wrathObjectIndex = 0;


    private PlayerInput playerInput;
    private InputAction pauseAction;
    private InputAction leftWrathListAction;
    private InputAction rightWrathListAction;
    
    private BelethMovementController belethController;
    private BelethHealthController belethHealthController;
    private BelethSinsController sinsController;

    bool isPaused;

    private void Awake()
    {

        playerInput = GetComponent<PlayerInput>();
        sinsController = GetComponent<BelethSinsController>();
        belethController = GetComponent<BelethMovementController>();
        belethHealthController = GetComponent<BelethHealthController>();

        pauseAction = playerInput.actions["Pause"];
        pauseAction.started += _ => CheckIfPaused();


        leftWrathListAction = playerInput.actions["WrathListLeft"];
        leftWrathListAction.started += _ => MoveWratPointerLeft();

        rightWrathListAction = playerInput.actions["WrathListRight"];
        rightWrathListAction.started += _ => MoveWratPointerRight();
    }

    private void Start()
    {
       
        

        cameraSpeedController.SetSpeedOnCamera();

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        pauseCanvas.SetActive(false);


        wrathUiPointers[0].SetActive(true);
        wrathUiPointers[1].SetActive(false);
        wrathUiPointers[2].SetActive(false);
        wrathUiPointers[3].SetActive(false);
        wrathUiPointers[4].SetActive(false);

        wrathObjectIndex = 0;



    }

    private void Update()
    {
        CheckStaminaSlider();
        SetHealthUI(CheckHealth());
        WaitForDesapearWrathUI();
    }

    #region Health UI

    private int CheckHealth()
    {
        return belethHealthController.GetHealthPoints();
    }
    public void SetHealthUI(int _toatLive)
    {
        switch (_toatLive)
        {
            case 0:
                foreach (var item in hearts)
                {
                    item.SetActive(false);
                }

                foreach (var item in emptyHearts)
                {
                    item.SetActive(true);
                }

                break;
            case 1:
                hearts[0].SetActive(true);
                hearts[1].SetActive(false);
                hearts[2].SetActive(false);
                hearts[3].SetActive(false);
                hearts[4].SetActive(false);
                hearts[5].SetActive(false);

                emptyHearts[0].SetActive(false);
                emptyHearts[1].SetActive(true);
                emptyHearts[2].SetActive(true);
                emptyHearts[3].SetActive(true);
                emptyHearts[4].SetActive(true);
                emptyHearts[5].SetActive(true);

                break;
            case 2:
                hearts[0].SetActive(true);
                hearts[1].SetActive(true);
                hearts[2].SetActive(false);
                hearts[3].SetActive(false);
                hearts[4].SetActive(false);
                hearts[5].SetActive(false);

                emptyHearts[0].SetActive(false);
                emptyHearts[1].SetActive(false);
                emptyHearts[2].SetActive(true);
                emptyHearts[3].SetActive(true);
                emptyHearts[4].SetActive(true);
                emptyHearts[5].SetActive(true);
                break;
            case 3:
                hearts[0].SetActive(true);
                hearts[1].SetActive(true);
                hearts[2].SetActive(true);
                hearts[3].SetActive(false);
                hearts[4].SetActive(false);
                hearts[5].SetActive(false);

                emptyHearts[0].SetActive(false);
                emptyHearts[1].SetActive(false);
                emptyHearts[2].SetActive(false);
                emptyHearts[3].SetActive(true);
                emptyHearts[4].SetActive(true);
                emptyHearts[5].SetActive(true);
                break;
            case 4:
                hearts[3].SetActive(true);
                hearts[4].SetActive(false);
                hearts[5].SetActive(false);


                emptyHearts[0].SetActive(false);
                emptyHearts[1].SetActive(false);
                emptyHearts[2].SetActive(false);
                emptyHearts[3].SetActive(false);
                emptyHearts[4].SetActive(true);
                emptyHearts[5].SetActive(true);
                break;
            case 5:
                hearts[4].SetActive(true);
                hearts[5].SetActive(false);

                emptyHearts[0].SetActive(false);
                emptyHearts[1].SetActive(false);
                emptyHearts[2].SetActive(false);
                emptyHearts[3].SetActive(false);
                emptyHearts[4].SetActive(false);
                emptyHearts[5].SetActive(true);
                break;
            case 6:
                foreach (var item in hearts)
                {
                    item.SetActive(true);
                }

                foreach (var item in emptyHearts)
                {
                    item.SetActive(false);
                }
                break;
            default:
                break;
        }
    }
    public void ShowDeathUI(bool _canShow)
    {

        deathCanvas.SetActive(_canShow);

    }

    #endregion

    #region Collectables UI

    public void ObtainedCollectable()
    {
        collectables[collectableIndex].SetActive(true);

        collectableIndex++;
    }

    #endregion

    #region Stamina slider
    private void CheckStaminaSlider()
    {
        //if (belethController.GetGliding())
        //{
        //    if (staminaAnimator.GetCurrentAnimatorStateInfo(0).IsName("GoingUp"))
        //    {
        //        PlayDown();
        //    }

        //    staminaAnimator.SetBool("start", true);
        //}
        //else
        //{
        //    if (staminaAnimator.GetCurrentAnimatorStateInfo(0).IsName("GoingDown"))
        //    {
        //        PlayUp();
        //    }

        //    staminaAnimator.SetBool("start", false);
        //}
    }
    public void PlayDown()
    {
        //staminaAnimator.Play("Base Layer.GoingDown", 0, 1 - slider.value);
    }
    public void PlayUp()
    {
        staminaAnimator.Play("Base Layer.GoingUp", 0, slider.value);
    }
    public void EmptyAnim()
    {
        //staminaAnimator.SetBool("start", false);

        //Deja de planear aqui
    }
    public float GetStaminaValue()
    {
        return slider.value;
    }

    #endregion

    #region Pause UI

    private void CheckIfPaused()
    {
        if (!isPaused)
        {
            Pause();

        }
        else
        {
            UnPause();
        }
    }

    public void Pause()
    {
        isPaused = true;
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        pauseCanvas.SetActive(true);
        cameraSpeedController.SetSpeedOnCamera();
    }

    public void UnPause()
    {
        isPaused = false;
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        pauseCanvas.SetActive(false);
        settingsCanvas.SetActive(false);
        controlsCanvas.SetActive(false);
        cameraSpeedController.enabled = false;
        cameraSpeedController.SetSpeedOnCamera();

    }

    public void ShowSettings()
    {
        pauseCanvas.SetActive(false);
        settingsCanvas.SetActive(true);
        cameraSpeedController.enabled = true;
    }

    public void HideSettings() 
    {
        settingsCanvas.SetActive(false);
        cameraSpeedController.enabled = false;
        pauseCanvas.SetActive(true);

    }

    public void ShowControls() 
    {
        pauseCanvas.SetActive(false);
        controlsCanvas.SetActive(true);
    }

    public void HideControls()
    {
        controlsCanvas.SetActive(false);
        pauseCanvas.SetActive(true);

    }

    public void QuitGame()
    {
        
        UnPause();
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        SceneManager.LoadScene("MainMenu");
    }

    #endregion

    #region Coins UI

    public void UpdateCoinUI(int _currentCoins) 
    {
        //Hacer que las monedas se cambien en el canvas
        coinText.text = _currentCoins.ToString();
    }

    #endregion

    #region WrathBarUI

    public void UpdateObjectList() 
    {

        for (int i = 0; i < 5; i++)
        {
            if (sinsController.wrathManager[i] != null)
            {
                switch (sinsController.wrathManager[i].objectType)
                {
                    case "Wall":
                        wrathUIImages[i].sprite = wrathUISprites[0];
                        wrathUIImages[i].color = new Color(1, 1, 1, 1);

                        break;
                    case "Platform":
                        wrathUIImages[i].sprite = wrathUISprites[1];
                        wrathUIImages[i].color = new Color(1, 1, 1, 1);
                        break;
                    case "Rock":
                        wrathUIImages[i].sprite = wrathUISprites[2];
                        wrathUIImages[i].color = new Color(1, 1, 1, 1);
                        break;
                    case "Activator":
                        wrathUIImages[i].sprite = wrathUISprites[3];
                        wrathUIImages[i].color = new Color(1, 1, 1, 1);
                        break;

                    default:
                        break;
                }
            }
            else
            {
                wrathUIImages[i].color = new Color(1, 1, 1, 0);
            }
        }
       
    }

    private void MoveWratPointerLeft() 
    {
        if (wrathObjectIndex < 4) 
        {
            wrathObjectIndex++;

        }
        else
        {
            wrathObjectIndex = 0;
        }

        UpdatePointerPos();
    }

    private void MoveWratPointerRight()
    {
        if (wrathObjectIndex > 0)
        {
            wrathObjectIndex--;

        }
        else
        {
            wrathObjectIndex = 4;
        }
        
        UpdatePointerPos();
    }

    private void UpdatePointerPos() 
    {

        wrathUiPointers[0].SetActive(false);
        wrathUiPointers[1].SetActive(false);
        wrathUiPointers[2].SetActive(false);
        wrathUiPointers[3].SetActive(false);
        wrathUiPointers[4].SetActive(false);
 

        wrathUiPointers[wrathObjectIndex].SetActive(true);

    }

    private void WaitForDesapearWrathUI() 
    {
        bool isEmpty = true;
        for (int i = 0; i < 5; i++)
        {
            if (sinsController.wrathManager[i] != null)
            {
                isEmpty = false;
            }
        }

        if (!isEmpty)
        {
            //Hacerlo aparecer
            wrathBar.SetActive(true);
            timeWaitedWrathBar = 0;
        }
        else
        {
            //Empezar a contar para hacerlo desaparecer    
            timeWaitedWrathBar += Time.deltaTime;

            if (timeToWaitWrathBar <= timeWaitedWrathBar)
            {
                wrathBar.SetActive(false);
            }
        }
    }


    #endregion


}
