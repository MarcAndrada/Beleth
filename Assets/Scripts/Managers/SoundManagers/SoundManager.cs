using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    private AudioSource audioSource2D;
    [SerializeField]
    private float[] maxAndMinPitch;
    [SerializeField]
    private float[] voicePitch;
    [SerializeField]
    private AudioClip[] footsteps;
    [SerializeField]
    private AudioClip[] wingFlap;
    [SerializeField]
    private AudioClip normalAttack;
    [SerializeField]
    private AudioClip wrathAttack;
    [SerializeField]
    private AudioClip wrathActivation;
    [SerializeField]
    private AudioClip jump;
    [SerializeField]
    private AudioClip belethDamage;
    [SerializeField]
    private AudioClip belethDeath;
    [SerializeField]
    private AudioClip revive;
    [SerializeField]
    private AudioClip checkPoint;

    [SerializeField]
    private AudioClip wrathExplosion; 

    [SerializeField]
    private AudioClip clickSound;
    [SerializeField]
    private AudioClip hoverSound;


    private void Start()
    {
        audioSource2D = GetComponent<AudioSource>();
    }

    #region Beleth Sounds

    public void SoundRandomFootstep(AudioSource _currentAS)
    {
        _currentAS.pitch = Random.Range(maxAndMinPitch[0], maxAndMinPitch[1]);
        _currentAS.PlayOneShot(footsteps[Random.Range(0, footsteps.Length - 1)]);
    }

    public void Jump(AudioSource _currentAS)
    {
        _currentAS.pitch = Random.Range(voicePitch[0], voicePitch[1]);
        _currentAS.PlayOneShot(jump);
    }

    public void WingsSound(AudioSource _currentAS)
    {
        _currentAS.pitch = Random.Range(maxAndMinPitch[0], maxAndMinPitch[1]);
        _currentAS.PlayOneShot(wingFlap[Random.Range(0, wingFlap.Length - 1)]);
    }

    public void NormalAttackSound(AudioSource _currentAS)
    {
        _currentAS.pitch = Random.Range(voicePitch[0], voicePitch[1]);
        _currentAS.PlayOneShot(normalAttack);
    }

    public void WrathAttackSound(AudioSource _currentAS)
    {
        _currentAS.pitch = Random.Range(voicePitch[0], voicePitch[1]);
        _currentAS.PlayOneShot(wrathAttack);

    }

    public void WrathActivation(int _numList)
    {
        if (_numList > 0)
        {
            audioSource2D.PlayOneShot(wrathActivation);
        }
        else
        {
            //Audio de error

        }
    }

    public void BelethDamaged()
    {
        audioSource2D.PlayOneShot(belethDamage);
    }

    public void BelethDeath()
    {
        audioSource2D.PlayOneShot(belethDeath);
    }

    public void ReviveSound()
    {
        audioSource2D.PlayOneShot(revive);
    }

    public void CheckPointSound()
    {
        audioSource2D.PlayOneShot(checkPoint);

    }

    #endregion


    #region Wrath

    public void WrathExplosion(AudioSource _currentAS) 
    {
        _currentAS.pitch = Random.Range(maxAndMinPitch[0], maxAndMinPitch[1]);
        _currentAS.PlayOneShot(wrathActivation);
    }

    #endregion

    #region Menu Sounds

    public void HoverMenu() {
        audioSource2D.PlayOneShot(hoverSound);
    }

    public void ClickMenu() {
        audioSource2D.PlayOneShot(clickSound);
    }

    #endregion


}
