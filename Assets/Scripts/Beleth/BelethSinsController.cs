using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;



public class BelethSinsController : MonoBehaviour
{

    [SerializeField]
    private GameObject followCamera;

    [SerializeField]
    private GameObject audioSrc3D;
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

            CheckObjectType(wrathManager[uiController.wrathObjectIndex], wrathManager[uiController.wrathObjectIndex].transform.position);

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

                CheckObjectType(wrathManager[i], wrathManager[i].transform.position);

                wrathManager[i].WrathExplosion();
                SoundManager._SOUND_MANAGER.WrathExplosion(wrathManager[i].GetComponent<AudioSource>());
                wrathManager[i] = null;
                uiController.UpdateObjectList();

            }
        }

        uiController.UpdateObjectList();
        SoundManager._SOUND_MANAGER.WrathActivation(willSound);

    }


    private void CheckObjectType(WrathExplosionController _currentItem, Vector3 pos )
    {
        GameObject aud = Instantiate(audioSrc3D, _currentItem.transform.position, _currentItem.transform.rotation);

        switch (_currentItem.objectType)
        {
            

            case "Wall":
                aud.GetComponent<AudioSource>().PlayOneShot(SoundManager._SOUND_MANAGER.wallBreak);
                Destroy(aud, 3);
                break;
            case "Platform":
                
                aud.GetComponent<AudioSource>().PlayOneShot(SoundManager._SOUND_MANAGER.boundPlatform);
                Destroy(aud, 3);

               
               

                break;
            case "Rock":
                aud.GetComponent<AudioSource>().PlayOneShot(SoundManager._SOUND_MANAGER.rockWrath);
                Destroy(aud, 3);

                break;
            case "Activator":
                SoundManager._SOUND_MANAGER.audioSource2D.PlayOneShot(SoundManager._SOUND_MANAGER.chainActivation);

                break;
            case "RockLauncher":
                wrathManager[uiController.wrathObjectIndex].GetComponent<RockLauncherController>().LauchRock();
               // aud.GetComponent<AudioSource>().PlayOneShot(SoundManager._SOUND_MANAGER.cannon);
                SoundManager._SOUND_MANAGER.audioSource2D.PlayOneShot(SoundManager._SOUND_MANAGER.cannon);
                Destroy(aud, 3);
                break;
            case "RockImpulsor":
                aud.GetComponent<AudioSource>().PlayOneShot(SoundManager._SOUND_MANAGER.boundPlatform);
                Destroy(aud, 3);
                break;
            default:
                break;
        }
    }

    #endregion
}