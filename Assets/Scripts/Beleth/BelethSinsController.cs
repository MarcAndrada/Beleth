using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class BelethSinsController : MonoBehaviour
{
    /* Variables en caso de que el ataque con la ira tenga diferentes stats que el ataque normal
     * 
     * [SerializeField]
     * private float wrathAttackCD;
     * [SerializeField]
     * private float wrathAttackDuration;
     * [SerializeField]
     * private float wrathAttackDecelSpeed;
     * 
    */

    [SerializeField]
    private GameObject followCamera;


    private BelethAnimController animController;
    private BelethMovementController movementController;
    private BelethAttackController attackController;
    private PlayerInput playerInput;
    private InputAction wrathAttackAction;
    private InputAction wrathExplisionAction;
    private List<WrathExplosionController> wrathManager = new List<WrathExplosionController>();


    // Start is called before the first frame update
    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        animController = GetComponent<BelethAnimController>();
        movementController = GetComponent<BelethMovementController>();
        attackController = GetComponent<BelethAttackController>();

        // Wrath Events
        wrathAttackAction = playerInput.actions["WrathAttack"];
        wrathAttackAction.started += _ => attackController.AttackAction_started(1);
        wrathExplisionAction = playerInput.actions["WrathActive"];
        wrathExplisionAction.started += _ => ExplodeAllObjects();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddWrathObject(WrathExplosionController _objComponent) {
        bool _same = false;

        foreach (var item in wrathManager)
        {
            if (item == _objComponent)
            {
                _same = true;
            }
        }

        if (!_same && _objComponent.canBeTriggered) 
        {
            wrathManager.Add(_objComponent);
        }

    }

    private void ExplodeAllObjects() {
       foreach (var item in wrathManager)
       {
           if (item != null)
           {
                item.WrathExplosion();
                SoundManager._SOUND_MANAGER.WrathExplosion(item.GetComponent<AudioSource>());

           }

       }

        SoundManager._SOUND_MANAGER.WrathActivation(wrathManager.Count);

       wrathManager.Clear();

    }


}
