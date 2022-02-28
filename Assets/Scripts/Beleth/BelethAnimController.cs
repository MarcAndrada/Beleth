using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BelethAnimController : MonoBehaviour
{

    private Animator animator;
    private CharacterController charController;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        charController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        SetOnAir(); 
    }

    private void SetOnAir() {

        animator.SetBool("OnAir", charController.isGrounded);
    }

    public void SetMovmentInput(bool _movingInputPressed) {
        animator.SetBool("UsingInput", _movingInputPressed);
    }
    public void SetSpeedValue(float _playerSpeed) {
        animator.SetFloat("Speed", _playerSpeed);

    }
    public void JumpTrigger() {

        animator.SetTrigger("Jump");
    
    }
    public void SetGliding(bool _isGliding) {
        animator.SetBool("Gliding", _isGliding);
    }
    public void DamageTrigger() {
        animator.SetTrigger("Damaged");
        
    }
    

}
