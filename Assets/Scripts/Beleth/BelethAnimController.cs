using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BelethAnimController : MonoBehaviour
{

    private Animator animator;
    private BelethMovementController movmentController;

    private bool touchedFloor = false;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        movmentController = GetComponent<BelethMovementController>();
        animator.SetBool("OnFloor", true);

    }

    // Update is called once per frame
    void Update()
    {
        SetOnAir(); 
    }


    #region Movement Animations
    public void SetMovmentInput(bool _movingInputPressed) {
        animator.SetBool("UsingInput", _movingInputPressed);
    }
    public void SetSpeedValue(float _playerSpeed) {
        animator.SetFloat("Speed", _playerSpeed);

    }
    #endregion

    #region Air Animations
    private void SetOnAir()
    {
        if ( movmentController.groundedPlayer && !touchedFloor)
        {
            animator.ResetTrigger("Jump");
            SetFirstJump(false);
            touchedFloor = true;
        }
        else if (!movmentController.groundedPlayer)
        {
            touchedFloor = true;

        }
        animator.SetBool("OnFloor", movmentController.groundedPlayer);
    }
    public void SetOnPlatform(bool _isOnPlatform)
    {
        animator.SetBool("OnPlatform", _isOnPlatform);
    }
    public void JumpTrigger() {

        animator.SetTrigger("Jump");
        StartCoroutine(WaitForJump());

    }
    public void SetFirstJump(bool _didFirstJump) 
    {
        animator.SetBool("FirstJump", _didFirstJump);
    }

    IEnumerator WaitForJump() {

        yield return new WaitForSeconds(0.1f);
        SetFirstJump(true);

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
