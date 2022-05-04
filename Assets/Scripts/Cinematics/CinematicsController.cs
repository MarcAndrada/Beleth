using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;


public class CinematicsController : MonoBehaviour
{
    public static CinematicsController _CINEMATICS_CONTROLLER;

    private PlayableDirector director;
    public bool isPlayingCinematic = false;

    [SerializeField]
    private PlayableAsset[] timeLines;
    // Start is called before the first frame update
    void Awake()
    {
        if (CinematicsController._CINEMATICS_CONTROLLER != null)
        {
            Destroy(gameObject);
        }
        else
        {
            _CINEMATICS_CONTROLLER = this;
        }

        director = GetComponent<PlayableDirector>();
    }

    private void Update() 
    {

        if (isPlayingCinematic)
        {
            if (director.state != PlayState.Playing)
            {
                isPlayingCinematic = false;
            }
        }

    }
    public void PlaySpecificCinematic(string _currentCinematic) 
    {
        switch (_currentCinematic)
        {
            case "WrathPuzleSerpiDead":
                director.playableAsset = timeLines[0];
                
                break;
            case "WrathPuzleActivatorL":
                director.playableAsset = timeLines[1];
                break;
            case "WrathPuzleActivatorR":
                director.playableAsset = timeLines[2];
                break;
            case "WrathPuzleSerpiTrapped":
                director.playableAsset = timeLines[3];
                break;
            case "WrathBossOpenDoor":
                director.playableAsset = timeLines[4];
                break;
            case "WrathDungeonEnd":
                director.playableAsset = timeLines[5];
                break;

            default:
                break;
        }
        director.Play();
        isPlayingCinematic = true;
    }


}
