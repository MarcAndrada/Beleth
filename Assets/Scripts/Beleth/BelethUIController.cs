using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BelethUIController : MonoBehaviour
{
    [SerializeField]
    private GameObject deathCanvas;

    [Header("Stamina Bar")]
    [SerializeField]
    Slider slider;
    [SerializeField]
    Animator staminaAnimator;

    

    BelethMovementController belethController;

    private void Start()
    {
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

}
