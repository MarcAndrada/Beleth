using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour
{

    public static VolumeController _VOLUME_CONTROLLER;

    [SerializeField]
    string masterVolumeParameter;
    [SerializeField]
    string musicVolumeParameter;
    [SerializeField]
    string sfxVolumeParameter;
    [SerializeField]
    private AudioMixer mixer;
    [SerializeField]
    private Slider masterSlider;
    [SerializeField]
    private Slider musicSlider;
    [SerializeField]
    private Slider sfxSlider;

    private float multiplier = 30;

    // Start is called before the first frame update
    private void Awake()
    {

        if (VolumeController._VOLUME_CONTROLLER != null)
        {
            Destroy(gameObject);

        }
        else
        {
            _VOLUME_CONTROLLER = this;

        }


       



    }

    private void Start()
    {
        mixer.SetFloat(masterVolumeParameter, Mathf.Log10(PlayerPrefs.GetFloat(masterVolumeParameter)) * multiplier);
        mixer.SetFloat(musicVolumeParameter, Mathf.Log10(PlayerPrefs.GetFloat(musicVolumeParameter)) * multiplier);
        mixer.SetFloat(sfxVolumeParameter, Mathf.Log10(PlayerPrefs.GetFloat(sfxVolumeParameter)) * multiplier);
    }


    private void HandleSliderValueChangedMaster(float _value)
    {
        
        mixer.SetFloat(masterVolumeParameter, Mathf.Log10(_value) * multiplier);
        PlayerPrefs.SetFloat(masterVolumeParameter, masterSlider.value);
        PlayerPrefs.Save();
    }
    private void HandleSliderValueChangedMusic(float _value)
    {
        mixer.SetFloat(musicVolumeParameter, Mathf.Log10(_value) * multiplier);
        PlayerPrefs.SetFloat(musicVolumeParameter, musicSlider.value);
        PlayerPrefs.Save();
    }
    private void HandleSliderValueChangedSFX(float _value)
    {
        mixer.SetFloat(sfxVolumeParameter, Mathf.Log10(_value) * multiplier);
        PlayerPrefs.SetFloat(sfxVolumeParameter, sfxSlider.value);
        PlayerPrefs.Save();
    }

    public void ChangeSlider(Slider _masterSlider, Slider _musicSlider, Slider _sfxSlider) 
    {
        masterSlider = _masterSlider;
        musicSlider = _musicSlider;
        sfxSlider = _sfxSlider;

        masterSlider.onValueChanged.AddListener(HandleSliderValueChangedMaster);
        musicSlider.onValueChanged.AddListener(HandleSliderValueChangedMusic);
        sfxSlider.onValueChanged.AddListener(HandleSliderValueChangedSFX);


        masterSlider.value = PlayerPrefs.GetFloat(masterVolumeParameter, masterSlider.value);
        musicSlider.value = PlayerPrefs.GetFloat(musicVolumeParameter, musicSlider.value);
        sfxSlider.value = PlayerPrefs.GetFloat(sfxVolumeParameter, sfxSlider.value);    
    }

}
