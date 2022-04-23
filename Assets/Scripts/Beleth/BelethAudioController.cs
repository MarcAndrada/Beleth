using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BelethAudioController : MonoBehaviour
{
    [SerializeField]
    private AudioSource feetAS;
    [SerializeField]
    private AudioSource wingAS;
    [SerializeField]
    private AudioSource attackAS;
    

    void Awake()
    {

    }


    public void FootstepSound() 
    {
        SoundManager._SOUND_MANAGER.SoundRandomFootstep(feetAS);
    }

    public void JumpSound() 
    {
        SoundManager._SOUND_MANAGER.Jump(attackAS);
    }

    public void WingSound() 
    {
        SoundManager._SOUND_MANAGER.WingsSound(wingAS);
    }

    public void AttackSound(int _currentAttack) 
    {
        switch (_currentAttack)
        {
            case 0:
                SoundManager._SOUND_MANAGER.NormalAttackSound(attackAS);
                break;
            case 1:
                SoundManager._SOUND_MANAGER.WrathAttackSound(attackAS);
                break;
            default:
                break;
        }
    }





}
