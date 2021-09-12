using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] float delayInSeconds = 2;


    const int MENU = 0,GAME = 1,GAMEOVER = 2;


    public void LoadStartScene()
    {
        SceneManager.LoadScene(MENU);

        FindObjectOfType<GameSession>().ResetGame();
    }


    public void LoadGameScene()
    {
        FindObjectOfType<GameSession>().ResetGame();

        SceneManager.LoadScene(GAME);

    }


    public void LoadGameoverScene()
    {
        StartCoroutine(WaitAndLoad());
    }


    public void LoadNextScene()
    {
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(sceneIndex + 1);
    }

    public void ExitGame()
    {
        Application.Quit();
    }


    IEnumerator WaitAndLoad()
    {
        yield return new WaitForSeconds(delayInSeconds);

        SceneManager.LoadScene(GAMEOVER);
    }
}
