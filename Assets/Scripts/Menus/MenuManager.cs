using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [Header("Play")]
    [SerializeField]
    string level;


    public void Play()
    {
        SceneManager.LoadScene(level);
    }

    public void Settings()
    {
        SceneManager.LoadScene("Credits");
    }

    public void Credits()
    {
        SceneManager.LoadScene("Settings");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
