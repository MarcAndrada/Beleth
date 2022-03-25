using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BelethResistanceUIController : MonoBehaviour
{
    Slider slider;
    Animator animator;
    BelethMovementController belethController;

    private void Awake()
    {
        slider = GetComponent<Slider>();
        animator = GetComponent<Animator>();
        belethController = GameObject.FindGameObjectWithTag("Player").GetComponent<BelethMovementController>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(slider.value);
        
        if (belethController.GetGliding())
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("GoingUp"))
            {
                PlayDown();
            }

            animator.SetBool("start", true);
        }
        else
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("GoingDown"))
            {
                PlayUp();
            }

            animator.SetBool("start", false);
        }
    }

    public void PlayDown()
    {
        animator.Play("Base Layer.GoingDown", 0, 1 - slider.value);
    }

    public void PlayUp()
    {
        animator.Play("Base Layer.GoingUp", 0, slider.value);
    }

    public void EmptyAnim()
    {
        animator.SetBool("start", false);

        //Deja de planear aqui
    }
}
