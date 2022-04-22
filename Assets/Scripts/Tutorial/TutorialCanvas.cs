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

    [SerializeField]
    string text;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            tutorialCanvas.SetActive(true);
            videoPlayer.clip = videoToPlay;
            tutorialCanvas.GetComponent<TutorialCanvasController>().SetThings();
        }
    }

    public string GetText()
    {
        return text;
    }
}

