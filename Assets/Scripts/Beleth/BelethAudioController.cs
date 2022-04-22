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
    
    [HideInInspector]
    public SoundManager soundCont;

    void Awake()
    {
        soundCont = GameObject.FindGameObjectWithTag("SoundManager").GetComponent<SoundManager>();

    }


    public void FootstepSound() 
    {
        soundCont.SoundRandomFootstep(feetAS);
    }

    public void JumpSound() 
    {
        soundCont.Jump(attackAS);
    }

    public void WingSound() 
    {
        soundCont.WingsSound(wingAS);
    }

    public void AttackSound(int _currentAttack) 
    {
        switch (_currentAttack)
        {
            case 0:
                soundCont.NormalAttackSound(attackAS);
                break;
            case 1:
                soundCont.WrathAttackSound(attackAS);
                break;
            default:
                break;
        }
    }





}
