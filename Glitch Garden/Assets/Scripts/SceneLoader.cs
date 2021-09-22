using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{

    [SerializeField] int timeToWait = 4;

    const int 
        SPLASH_SCREEN = 0,
        START_SCREEN = 1,
        GAME_SCREEN = 2;

    int currentSceneIndex = SPLASH_SCREEN;



    // Start is called before the first frame update
    void Start()
    {
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        if (currentSceneIndex == SPLASH_SCREEN)
        {
            StartCoroutine(WaitForTime());
        }
    
    }

    private IEnumerator WaitForTime()
    {
        yield return new WaitForSeconds(timeToWait);
        LoadNextScreen();
    }

    private void LoadNextScreen()
    {
        currentSceneIndex++;
        SceneManager.LoadScene(currentSceneIndex);
    }

    private void LoadStartScene()
    {
        SceneManager.LoadScene(START_SCREEN);
    }


    private void LoadGameScene()
    {
        SceneManager.LoadScene(GAME_SCREEN);
    }

}
