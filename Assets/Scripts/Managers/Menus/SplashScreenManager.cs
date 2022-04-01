using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashScreenManager : MonoBehaviour
{
    [SerializeField]
    private int secondsToWait;
    [SerializeField]
    private GameObject managers;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ToMainMenu());
        DontDestroyOnLoad(managers);
    }

    IEnumerator ToMainMenu()
    {
        yield return new WaitForSeconds(secondsToWait);
        SceneManager.LoadScene("MainMenu");
    }
}
