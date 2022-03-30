using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem;


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

    PlayerInput playerInput;
    InputAction pauseAction;

    BelethMovementController belethController;
    BelethHealthController belethHealthController;

    bool isPaused;

    private void Start()
    {
        pauseCanvas.SetActive(false);
        playerInput = GetComponent<PlayerInput>();
        pauseAction = playerInput.actions["Pause"];
        pauseAction.started += _ => PauseGame();
        belethController = GetComponentInParent<BelethMovementController>();
        belethHealthController = GetComponentInParent<BelethHealthController>();

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;

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
    public void ShowDeathUI(bool _canShow) {

        deathCanvas.SetActive(_canShow);
    
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

    private void ToGame()
    {
        isPaused = !isPaused;
        pauseCanvas.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1;

    }

    private void PauseGame()
    {
        if (!isPaused)
        {
            isPaused =! isPaused;
            pauseCanvas.SetActive(true);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
            Time.timeScale = 0;

        }
        else
        {
            ToGame();
        }
    }

    public void ContinuePause()
    {
        ToGame();
    }

    public void SettingsGame()
    {
        ToGame();
        SceneManager.LoadScene("Settings");
    }

    public void QuitGame()
    {
        ToGame();
        SceneManager.LoadScene("MainMenu");
    }

    #endregion

}
