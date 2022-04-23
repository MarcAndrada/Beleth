using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SliderVolumeController : MonoBehaviour
{
    [SerializeField]
    private Slider masterSlider;
    [SerializeField]
    private Slider musicSlider;
    [SerializeField]
    private Slider sfxSlider;

    // Start is called before the first frame update
    void Start()
    {
        VolumeController._VOLUME_CONTROLLER.ChangeSlider(masterSlider, musicSlider, sfxSlider);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
