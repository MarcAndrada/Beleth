using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathCanvasController : MonoBehaviour
{
    [SerializeField] BelethHealthController healthController;
    [SerializeField] Animator BossAnimator;
    [SerializeField] Animator FillAnimator;

    // Update is called once per frame
    void Update()
    {
        if (healthController.GetHealthPoints() == 0)
        {
            ActivateBossAnimation();
        }
    }

    public void ActivateFillAnimation()
    {
        FillAnimator.SetBool("canMove", true);
        FillAnimator.SetBool("canIdle", false);
    }

    public void ActivateBossAnimation()
    {
        BossAnimator.SetBool("canMove", true);
        BossAnimator.SetBool("canIdle", false);
    }

    public void GoIdleAgain()
    {
        BossAnimator.SetBool("canMove", false);
        BossAnimator.SetBool("canIdle", true);

        FillAnimator.SetBool("canMove", false);
        FillAnimator.SetBool("canIdle", true);
    }
}
