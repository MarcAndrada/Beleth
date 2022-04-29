using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BelethAnimController : MonoBehaviour
{

    private Animator animator;
    private BelethMovementController movmentController;


    // Start is called before the first frame update
    void Awake() 
    {
        animator = GetComponentInChildren<Animator>();
        movmentController = GetComponent<BelethMovementController>();
    }

    #region Movement Animations
    public void SetMovmentInput(bool _movingInputPressed) {
        animator.SetBool("UsingInput", _movingInputPressed);
    }
    public void SetRunning(bool _running) {
        animator.SetBool("Running", _running);

    }
    #endregion

    #region Air Animations
    public void SetOnAir()
    {
    
        animator.SetBool("OnFloor", movmentController.groundedPlayer);
    
    }
    public void SetOnPlatform(bool _isOnPlatform)
    {
        animator.SetBool("OnPlatform", _isOnPlatform);
    }
    public void JumpTrigger() {

        animator.SetTrigger("Jump");

    }
    public void ResetJumpTrigger() 
    {
        animator.ResetTrigger("Jump");

    }
    public void SetFirstJump(bool _didFirstJump) 
    {
        animator.SetBool("FirstJump", _didFirstJump);
    }

    public void SetGliding(bool _isGliding) {
        animator.SetBool("Gliding", _isGliding);
    }

    #endregion

    #region Health Anims
    public void DamageTrigger() {
        animator.SetTrigger("Damaged");
        
    }
    public void SetHealthValue(int _currentHP) {
        animator.SetInteger("Health", _currentHP);

    }

    #endregion

    #region Attack Anims
    
    public void SetIsAttacking(bool _isAttacking) 
    {
        animator.SetBool("isAttacking", _isAttacking);
    }
    public void AttackTrigger() {

        animator.SetTrigger("Attack");

    }
    public void WrathAttackTrigger()
    {

        animator.SetTrigger("WrathAttack");

    }

    public void ResetAttackTrigger()
    {
        animator.ResetTrigger("Attack");
        animator.ResetTrigger("WrathAttack");
    }

    #endregion


}
