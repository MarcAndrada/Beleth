using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class BelethSinsController : MonoBehaviour
{

    [SerializeField]
    private GameObject followCamera;


    private BelethAnimController animController;
    private BelethMovementController movementController;
    private BelethAttackController attackController;
    private BelethUIController uiController;
    private PlayerInput playerInput;
    private InputAction wrathAttackAction;
    private InputAction wrathExplisionAction;
    //[HideInInspector]
    public WrathExplosionController[] wrathManager = new WrathExplosionController[5];

    // Start is called before the first frame update
    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        animController = GetComponent<BelethAnimController>();
        movementController = GetComponent<BelethMovementController>();
        attackController = GetComponent<BelethAttackController>();
        uiController = GetComponent<BelethUIController>();

        // Wrath Events
        wrathAttackAction = playerInput.actions["WrathAttack"];
        wrathAttackAction.started += _ => attackController.AttackAction_started(1);
        wrathExplisionAction = playerInput.actions["WrathActiveAll"];
        wrathExplisionAction.started += _ => ExplodeAllObjects();
        wrathExplisionAction = playerInput.actions["WrathActiveOne"];
        wrathExplisionAction.started += _ => ExplodeOneObject();

    }

    #region Wrath
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
            for (int i = 0; i < 5; i++)
            {
                if (wrathManager[i] == null) 
                {
                    wrathManager[i] = _objComponent;
                    _objComponent.SetWrath();
                    uiController.UpdateObjectList();
                    break;
                }

            }
           
            
        }

    }


    private void ExplodeOneObject() 
    {
        if (wrathManager.Length > 0 && wrathManager[uiController.wrathObjectIndex] != null)
        {

            CheckObjectType(wrathManager[uiController.wrathObjectIndex].objectType, wrathManager[uiController.wrathObjectIndex].transform.position);

            wrathManager[uiController.wrathObjectIndex].WrathExplosion();
            SoundManager._SOUND_MANAGER.WrathExplosion(wrathManager[uiController.wrathObjectIndex].gameObject.GetComponent<AudioSource>());
            wrathManager[uiController.wrathObjectIndex] = null;
            uiController.UpdateObjectList();
            

        }

        uiController.UpdateObjectList();


    }

    private void ExplodeAllObjects() {

        bool willSound = false;
        
        for (int i = 0; i < wrathManager.Length; i++)
        {
            if (wrathManager[i] != null)
            {

                CheckObjectType(wrathManager[i].objectType, wrathManager[i].transform.position);

                wrathManager[i].WrathExplosion();
                SoundManager._SOUND_MANAGER.WrathExplosion(wrathManager[i].GetComponent<AudioSource>());
                wrathManager[i] = null;
                uiController.UpdateObjectList();

            }
        }

        uiController.UpdateObjectList();
        SoundManager._SOUND_MANAGER.WrathActivation(willSound);

    }


    private void CheckObjectType(string _currentType, Vector3 pos )
    {
        switch (_currentType)
        {

            case "Wall":
                AudioSource.PlayClipAtPoint(SoundManager._SOUND_MANAGER.wallBreak, pos);

                break;
            case "Platform":
                break;
            case "Rock":
                break;
            case "Activator":
                break;
            case "RockLauncher":
                wrathManager[uiController.wrathObjectIndex].GetComponent<RockLauncherController>().LauchRock();
                break;
            case "RockImpulsor":
                break;

            default:
                break;
        }
    }

    #endregion
}