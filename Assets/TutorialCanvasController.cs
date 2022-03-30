using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class TutorialCanvasController : MonoBehaviour
{
    [SerializeField]
    GameObject Player;
    [SerializeField]
    VideoPlayer videoPlayer;
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

    public void SetLenght()
    {
        
        timeToStop = videoPlayer.length * timesToPlay;
    }
}
