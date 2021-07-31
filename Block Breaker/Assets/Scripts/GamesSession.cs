using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class GamesSession : MonoBehaviour
{
    // config params
    [Range(0.1f,10f)][SerializeField] float gameSpeed = 1f;
    [SerializeField] uint scorePerBlockDestroyed = 85;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] bool autoPlayEnabled = false;

    // State variables
    [SerializeField] uint currentScore = 0;


    private void Awake()
    {
        int gameStauesCount = FindObjectsOfType<GamesSession>().Length;
        if(gameStauesCount > 1)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {

        scoreText = FindObjectOfType<TextMeshProUGUI>();

        scoreText.text = currentScore.ToString();
    }

    public bool IsAutoPlayEnabled()
    {
        return autoPlayEnabled;
    }


    // Update is called once per frame
    void Update()
    {
        Time.timeScale = gameSpeed;
    }

    
    //Add score when block destroyed.
    public void AddScore()
    {
        currentScore += scorePerBlockDestroyed;

        scoreText.text = currentScore.ToString();
    }


    public void ResetGame()
    {
        Destroy(gameObject);
    }
}
