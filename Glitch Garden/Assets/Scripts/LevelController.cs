using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    [SerializeField] GameObject winLabel;

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
    private void Start(){ }

    //
    private void Update(){}

    public void AttackerSpawned(){ AttackersAlive++; }

    public void AttackerKilled() {
        AttackersAlive--; 

        if(AttackersAlive <= 0 && timerEnded)
        {
            StartCoroutine(HandleWindCondition());
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


    IEnumerator HandleWindCondition()
    {

        winLabel.SetActive(true);

        GetComponent<AudioSource>().Play();

        yield return new WaitForSeconds(waitToLoad);

        FindObjectOfType<SceneLoader>().LoadNextScreen();
    }
}
