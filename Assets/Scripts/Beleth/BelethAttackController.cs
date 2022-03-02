using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BelethAttackController : MonoBehaviour
{

    [Header("Attack")]
    [SerializeField]
    private float attackCD;
    [SerializeField]
    private GameObject followCamera;

    private BelethAnimController animController;
    private PlayerInput playerInput;
    private InputAction attackAction;
    // Start is called before the first frame update
    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        animController = GetComponent<BelethAnimController>();

        //Attack Events
        attackAction = playerInput.actions["Attack"];
        attackAction.started += AttackAction_started;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void AttackAction_started(InputAction.CallbackContext obj)
    {
        //Hacer la animacion
        animController.AttackTrigger();


    }


}
