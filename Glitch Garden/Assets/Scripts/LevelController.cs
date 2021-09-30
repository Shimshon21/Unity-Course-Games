using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    [SerializeField] GameObject winLabel;

    [SerializeField] GameObject loseLabel;

    [SerializeField] int AttackersAlive = 0;

    AttackerSpawner[] attackerSpawners;

    [SerializeField] float waitToLoad;

    bool timerEnded = false,
        gameWon = false;

    private void Awake()
    {
        //SetUpSingleton();

        AttackersAlive = 0;
    }

    //
    private void SetUpSingleton()
    {
        if (GetComponents<LevelController>().Length > 1)
        {
            gameObject.SetActive(false);

            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    //
    private void Start(){
        if(winLabel && loseLabel)
        {
            loseLabel.SetActive(false);

            winLabel.SetActive(false);
        }
    
    }

    //
    private void Update(){}

    public void HandleLoseCondition()
    {
        loseLabel.SetActive(true);

        Time.timeScale = 0;
    }


    public void AttackerSpawned(){ AttackersAlive++; }


    public void AttackerKilled() {

        bool isPlayerLost = FindObjectOfType<PlayerHealthDisplay>().IsPlayerLost();

        AttackersAlive--; 

        if(AttackersAlive <= 0 && timerEnded && !isPlayerLost)
        {
            StartCoroutine(HandleWinCondition());
        }
    }


    public void LevelTimerEnded()
    {
        timerEnded = true;

        StopSpawners();
    }


    private void StopSpawners()
    {
        var spawners = FindObjectsOfType<AttackerSpawner>();

        foreach(var spawner in spawners)
        {
            spawner.StopSpawning();
        }
    }


    IEnumerator HandleWinCondition()
    {

        winLabel.SetActive(true);

        GetComponent<AudioSource>().Play();

        yield return new WaitForSeconds(waitToLoad);

        FindObjectOfType<SceneLoader>().LoadNextScene();
    }

}
