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
        OPTIONS_SCREEN = 2,
        GAME_SCREEN = 3,
        GAMEOVER_SCREEN = 4;

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


    private void Update(){}

    private IEnumerator WaitForTime()
    {
        yield return new WaitForSeconds(timeToWait);
        LoadNextScene();
    }


    public void LoadNextScene()
    {
        currentSceneIndex++;
        SceneManager.LoadScene(currentSceneIndex);
    }


    public void LoadMenuScene()
    {
        SceneManager.LoadScene(START_SCREEN);

        Time.timeScale = 1;
    }


    public void LoadLevel1Scene()
    {
        SceneManager.LoadScene(GAME_SCREEN);
    }


    public void LoadGameOverScene()
    {
        SceneManager.LoadScene(GAMEOVER_SCREEN);
    }


    public void RestartLevel()
    {
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        SceneManager.LoadScene(currentSceneIndex);

        Time.timeScale = 1;
    }


    public void LoadOptionsScene()
    {
       SceneManager.LoadScene(OPTIONS_SCREEN);
    }


    public void ExitGame()
    {
        Application.Quit();
    }
}
