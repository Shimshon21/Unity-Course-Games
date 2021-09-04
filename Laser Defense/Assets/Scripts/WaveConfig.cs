using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy wave Config ")]



// Enemy Waves configuration class
public class WaveConfig : ScriptableObject
{
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] GameObject pathPrefab;
    [SerializeField] float timeBetweenSpawns = 1f;
    [SerializeField] float spawnRandomFactor = 0.3f;
    [SerializeField] int numberOfEnemies = 20;
    [SerializeField] float moveSpeed = 2f;

    public List<Transform> GetWaypoints()
    {
        var waveWaypoints = new List<Transform>();

        foreach(Transform child in pathPrefab.transform)
        {
            waveWaypoints.Add(child);
        }
       
        return waveWaypoints;
    }

    public GameObject GetEnemyPrefab() { return enemyPrefab; }

    public GameObject GetPathPrefab() { return pathPrefab; }

    public float GetTimeBetweenSpawns() { return timeBetweenSpawns; }
    public float GetSpawnsFactor() { return spawnRandomFactor; }

    public int GetNumberOfEnemies() { return numberOfEnemies; }

    public float GetMoveSpeed() { return moveSpeed; }
}
