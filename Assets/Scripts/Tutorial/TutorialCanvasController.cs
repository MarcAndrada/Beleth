using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class TutorialCanvasController : MonoBehaviour
{
    [SerializeField]
    TutorialCanvas m_Canvas;
    [SerializeField]
    GameObject Player;
    [SerializeField]
    VideoPlayer videoPlayer;
    [SerializeField]
    Text text;
    [SerializeField]
    double timesToPlay;
    double timeToStop;
    float timer;

    // Start is called before the first frame update
    void Start()
    {
        timeToStop = videoPlayer.length * timesToPlay;
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if(timer>= timeToStop)
        {
            timer = 0;
            gameObject.SetActive(false);
        }
    }

    public void SetThings()
    {
        timeToStop = videoPlayer.length * timesToPlay;
        text.text = m_Canvas.GetText();
    }
}
