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

    [Header("Stamina Bar")]
    [SerializeField]
    Slider slider;
    [SerializeField]
    Animator staminaAnimator;

    PlayerInput playerInput;
    InputAction pauseAction;

    BelethMovementController belethController;

    bool isPaused;

    private void Start()
    {
        pauseCanvas.SetActive(false);
        playerInput = GetComponent<PlayerInput>();
        pauseAction = playerInput.actions["Pause"];
        pauseAction.started += _ => PauseGame();
        belethController = GetComponentInParent<BelethMovementController>();
    }

    private void Update()
    {
        CheckStaminaSlider();
    }

    #region Health UI
    public void SetHealthUI(int _toatLive)
    {
        switch (_toatLive)
        {
            case 0:

                break;
            case 1:

                break;
            case 2:

                break;
            case 3:

                break;
            case 4:

                break;
            case 5:

                break;
            case 6:

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
        Time.timeScale = 1;
    }

    private void PauseGame()
    {
        if (!isPaused)
        {
            isPaused =! isPaused;
            pauseCanvas.SetActive(true);
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
        SceneManager.LoadScene("SettingsScene");
    }

    public void QuitGame()
    {
        ToGame();
        SceneManager.LoadScene("MainMenu");
    }

    #endregion

}
