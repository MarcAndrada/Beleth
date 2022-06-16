using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuAudioController : MonoBehaviour
{

    [SerializeField]
    GameObject audioSrc;
    // Start is called before the first frame update    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayHoverSound()
    {

        audioSrc.GetComponent<AudioSource>().PlayOneShot(SoundManager._SOUND_MANAGER.hoverSound);
    }

    public void PlayClickSound()
    {

        audioSrc.GetComponent<AudioSource>().PlayOneShot(SoundManager._SOUND_MANAGER.clickSound);
    }
}
