using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{

    private void Awake()
    {

    }


    public void GoWrathLevel()
    {
        SceneManager.LoadScene("NuevoNivelTest");
    }

    public void GoMainMenuScene() 
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void GoSettingsScene()
    {
        SceneManager.LoadScene("Settings");
    }

    public void GoControlsScene() 
    {
        SceneManager.LoadScene("Controls");
    }


    public void GoCreditsScene()
    {
        SceneManager.LoadScene("Credits");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
