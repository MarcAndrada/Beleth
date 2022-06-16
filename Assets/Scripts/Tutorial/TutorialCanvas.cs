using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;



public class TutorialCanvas : MonoBehaviour

{
    [SerializeField]
    GameObject tutorialCanvas;

    [SerializeField]
    VideoPlayer videoPlayer;

    [SerializeField]
    VideoClip videoToPlay;


    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            tutorialCanvas.SetActive(true);
            videoPlayer.clip = videoToPlay;
            tutorialCanvas.GetComponent<TutorialCanvasController>().SetThings();
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            tutorialCanvas.SetActive(false);
        }

    }
}

