using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    public static SoundManager _SOUND_MANAGER;


    public AudioSource audioSource2D;
    [Header("Music")]
    [SerializeField]
    private AudioSource musicAudioSource;
    [SerializeField]
    private AudioClip musicClip;
    [SerializeField]
    private AudioClip bossMusicClip;


    [Header("BelethSounds")]
    [SerializeField]
    private float[] actionsPitch;
    [SerializeField]
    private float[] voicePitch;
    [SerializeField]
    private AudioClip[] beleth_Footsteps;
    [SerializeField]
    private AudioClip[] beleth_WingFlap;
    [SerializeField]
    private AudioClip beleth_NormalAttack;
    [SerializeField]
    private AudioClip beleth_WrathAttack;
    [SerializeField]
    private AudioClip beleth_WrathActivation;
    [SerializeField]
    private AudioClip beleth_Jump;
    [SerializeField]
    private AudioClip[] beleth_Damage;
    [SerializeField]
    private AudioClip beleth_Death;
    [SerializeField]
    private AudioClip beleth_Revive;
    [SerializeField]
    private AudioClip checkPoint;
    

    [Header("WrathBoss Sounds")]
    [SerializeField]
    private AudioClip wrathBoss_GoBelowFloor;
    [SerializeField]
    private AudioClip wrathBoss_ReturnBelowFloor;
    [SerializeField]
    private AudioClip wrathBoss_LavaCircle;
    [SerializeField]
    private AudioClip wrathBoss_EnemieCircle;
    [SerializeField]
    private AudioClip wrathBoss_Damaged;
    [SerializeField]
    private AudioClip wrathBoss_Death;

    [Header("Pimpollo Sounds")]
    [SerializeField]
    private AudioClip pimpollo_UnHide;
    [SerializeField]
    private AudioClip pimpollo_Chase;
    [SerializeField]
    private AudioClip pimpollo_Jump;
    [SerializeField]
    private AudioClip pimpollo_Explosion;
    [SerializeField]
    private AudioClip pimpollo_Death;

    [Header("Menu Sounds")]
    [SerializeField]
    private AudioClip clickSound;
    [SerializeField]
    private AudioClip hoverSound;

    [Header("Ambience Sounds")]
    [SerializeField]
    private AudioClip lavaFlow;

    [Header("Snake Sounds")]
    [SerializeField]
    private AudioClip snakeAttack;
    [SerializeField]
    private AudioClip chainActivation;

    [Header("Vessel")]
    [SerializeField]
    private AudioClip vesselBreak;

    [Header("Collectable")]
    [SerializeField]
    private AudioClip getSoul;
    [SerializeField]
    private AudioClip getFavor;

    [Header("Phantom")]
    [SerializeField]
    private AudioClip phantomAppears;


    [Header("WallBreak")]
    [SerializeField]
    public AudioClip wallBreak;




    private void Awake()
    {
        if (SoundManager._SOUND_MANAGER != null)
        {
            Destroy(gameObject);

        }
        else
        {
            _SOUND_MANAGER = this;

        }
        audioSource2D = GetComponent<AudioSource>();




    }

    private void Start()
    {
        musicAudioSource.clip = musicClip;
    }

    #region Beleth Sounds

    public void SoundRandomFootstep(AudioSource _currentAS)
    {
        _currentAS.pitch = Random.Range(actionsPitch[0], actionsPitch[1]);
        _currentAS.PlayOneShot(beleth_Footsteps[Random.Range(0, beleth_Footsteps.Length - 1)]);
    }

    public void Jump(AudioSource _currentAS)
    {
        _currentAS.pitch = Random.Range(voicePitch[0], voicePitch[1]);
        _currentAS.PlayOneShot(beleth_Jump);
    }

    public void WingsSound(AudioSource _currentAS)
    {
        _currentAS.pitch = Random.Range(actionsPitch[0], actionsPitch[1]);
        _currentAS.PlayOneShot(beleth_WingFlap[Random.Range(0, beleth_WingFlap.Length - 1)]);
    }

    public void NormalAttackSound(AudioSource _currentAS)
    {
        _currentAS.pitch = Random.Range(voicePitch[0], voicePitch[1]);
        _currentAS.PlayOneShot(beleth_NormalAttack);
    }

    public void WrathAttackSound(AudioSource _currentAS)
    {
        _currentAS.pitch = Random.Range(voicePitch[0], voicePitch[1]);
        _currentAS.PlayOneShot(beleth_WrathAttack);

    }

    public void WrathActivation(bool _doSound)
    {
        if (_doSound)
        {
            audioSource2D.PlayOneShot(beleth_WrathActivation);
        }
        else
        {
            //Audio de error

        }
    }

    public void BelethDamaged()
    {
        audioSource2D.PlayOneShot(beleth_Damage[Random.Range(0, beleth_Damage.Length)]);
    }

    public void BelethDeath()
    {
        audioSource2D.PlayOneShot(beleth_Death);
    }

    public void ReviveSound()
    {
        audioSource2D.PlayOneShot(beleth_Revive);
    }

    public void CheckPointSound()
    {
        audioSource2D.PlayOneShot(checkPoint);

    }

    #endregion

    #region Boss Sounds
    public void WrathBossGoBelowFloorSound(AudioSource _currentAS)
    {
        _currentAS.PlayOneShot(wrathBoss_GoBelowFloor);
    }
    public void WrathBossReturnBelowFloorSound(AudioSource _currentAS)
    {
        if (!_currentAS.isPlaying)
        {
            _currentAS.PlayOneShot(wrathBoss_ReturnBelowFloor);
        }
    }
    public void WrathBossLavaCircleSound(AudioSource _currentAS)
    {
        _currentAS.PlayOneShot(wrathBoss_LavaCircle);
    }
    public void WrathBossEnemieCircleSound(AudioSource _currentAS)
    {
        _currentAS.PlayOneShot(wrathBoss_EnemieCircle);
    }
    public void WrathBossDamagedSound(AudioSource _currentAS)
    {
        _currentAS.PlayOneShot(wrathBoss_Damaged);
    }
    public void WrathBossDeadSound(AudioSource _currentAS)
    {
        _currentAS.PlayOneShot(wrathBoss_Death);
    }

    #endregion

    #region Pimpollo Sounds
    public void PimpolloUnHideSound(AudioSource _currentAS)
    {
        _currentAS.PlayOneShot(pimpollo_UnHide);
    }
    public void PimpolloChaseSound(AudioSource _currentAS)
    {
        _currentAS.PlayOneShot(pimpollo_Chase);
    }
    public void PimpolloJumpSound(AudioSource _currentAS)
    {
        _currentAS.PlayOneShot(pimpollo_Jump);
    }
    public void PimpolloExplosionSound(AudioSource _currentAS)
    {
        _currentAS.PlayOneShot(pimpollo_Explosion);
    }
    public void PimpolloDeathSound(AudioSource _currentAS)
    {
        _currentAS.PlayOneShot(pimpollo_Death);
    }

    #endregion

    #region Music
    public void ChangeMusicBoss() 
    {
        musicAudioSource.clip = bossMusicClip;
        musicAudioSource.Play();

    }
    public void ChangeMusicLevel()
    {
        musicAudioSource.clip = musicClip;
        musicAudioSource.Play();
    }

    #endregion

    #region Wrath

    public void WrathExplosion(AudioSource _currentAS) 
    {
        if (_currentAS != null)
        {
            _currentAS.pitch = Random.Range(actionsPitch[0], actionsPitch[1]);
            _currentAS.PlayOneShot(beleth_WrathActivation);
        }
        
    }

    #endregion

    #region Vessel
    public void VesselBreak(AudioSource _currentAS)
    {
        if (_currentAS != null)
        {

            _currentAS.PlayOneShot(vesselBreak);
        }

    }

    #endregion

    #region Colletcables
    public void FavorGet(AudioSource _currentAS)
    {
  

        if (_currentAS != null)
        {

            _currentAS.PlayOneShot(getFavor);
        }

    }
    public void SoulGet(AudioSource _currentAS) { 
          if (_currentAS != null)
          {

            _currentAS.PlayOneShot(getSoul);
          }
    }
    #endregion

    #region Phantom
    public void PhantomApears(AudioSource _currentAS)
    {


        if (_currentAS != null)
        {

            _currentAS.PlayOneShot(phantomAppears);
        }
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

    #region WallBreak

    public void WallBreak(AudioSource _currentAS)
    {
        if(_currentAS)
        _currentAS.PlayOneShot(wallBreak);
    }


    #endregion
}
