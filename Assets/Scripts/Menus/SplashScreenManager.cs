using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashScreenManager : MonoBehaviour
{
    [SerializeField]
    int secondsToWait;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ToMainMenu());
    }

    IEnumerator ToMainMenu()
    {
        yield return new WaitForSeconds(secondsToWait);
        SceneManager.LoadScene("MainMenu");
    }
}
