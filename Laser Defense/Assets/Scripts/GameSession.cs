using System;
using TMPro;
using UnityEngine;

public class GameSession : MonoBehaviour
{
    [SerializeField] int score = 0;
    [SerializeField] int scorePerEnemy = 0;

    private void Awake()
    {
        SetUpSingleton();

    }

    private void SetUpSingleton()
    {
        int numberOfSessions = FindObjectsOfType<GameSession>().Length;

        if (numberOfSessions > 1)
        {
            gameObject.SetActive(false);

            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    public void ResetGame()
    {
        Destroy(gameObject);
    }

    public void AddScore()
    {
        score += scorePerEnemy;
    }

    public int GetScore()
    {
        return score;
    }
}
