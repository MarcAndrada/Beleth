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
    GameObject hudCanvas;

    [Header("Stamina Bar")]
    [SerializeField]
    Slider slider;
    [SerializeField]
    Animator staminaAnimator;

    [Header("HUD")]
    [SerializeField]
    GameObject[] hearts;
    [SerializeField]
    private GameObject[] collectables;
    [SerializeField]
    private TextMeshProUGUI coinText;
    

    PlayerInput playerInput;
    InputAction pauseAction;

    BelethMovementController belethController;
    BelethHealthController belethHealthController;

    bool isPaused;

    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        pauseAction = playerInput.actions["Pause"];
        pauseAction.started += _ => CheckIfPaused();
        belethController = GetComponent<BelethMovementController>();
        belethHealthController = GetComponent<BelethHealthController>();
        

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        pauseCanvas.SetActive(false);


    }

    private void Update()
    {
        CheckStaminaSlider();
        SetHealthUI(CheckHealth());
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
                break;
            case 1:
                hearts[0].SetActive(true);
                hearts[1].SetActive(false);
                hearts[2].SetActive(false);
                hearts[3].SetActive(false);
                hearts[4].SetActive(false);
                hearts[5].SetActive(false);
                break;
            case 2:
                hearts[1].SetActive(true);
                hearts[2].SetActive(false);
                hearts[3].SetActive(false);
                hearts[4].SetActive(false);
                hearts[5].SetActive(false);
                break;
            case 3:
                hearts[2].SetActive(true);
                hearts[3].SetActive(false);
                hearts[4].SetActive(false);
                hearts[5].SetActive(false);
                break;
            case 4:
                hearts[3].SetActive(true);
                hearts[4].SetActive(false);
                hearts[5].SetActive(false);
                break;
            case 5:
                hearts[4].SetActive(true);
                hearts[5].SetActive(false);
                break;
            case 6:
                foreach (var item in hearts)
                {
                    item.SetActive(true);
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

    public void ObtainedCollectable(int _collectableID) 
    {
        collectables[_collectableID].SetActive(true);
    }

    #endregion

    #region Stamina slider
    private void CheckStaminaSlider()
    {
        if (belethController.GetGliding())
        {
            if (staminaAnimator.GetCurrentAnimatorStateInfo(0).IsName("GoingUp"))
            {
                PlayDown();
            }

            staminaAnimator.SetBool("start", true);
        }
        else
        {
            if (staminaAnimator.GetCurrentAnimatorStateInfo(0).IsName("GoingDown"))
            {
                PlayUp();
            }

            staminaAnimator.SetBool("start", false);
        }
    }
    public void PlayDown()
    {
        staminaAnimator.Play("Base Layer.GoingDown", 0, 1 - slider.value);
    }
    public void PlayUp()
    {
        staminaAnimator.Play("Base Layer.GoingUp", 0, slider.value);
    }
    public void EmptyAnim()
    {
        staminaAnimator.SetBool("start", false);

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
        
    }

    public void UnPause() 
    {
        isPaused = false;
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        pauseCanvas.SetActive(false);


    }

    public void SettingsGame()
    {
        UnPause();
        SceneManager.LoadScene("Settings");
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


}
