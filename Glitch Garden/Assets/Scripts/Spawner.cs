using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] float minSpawnTime = 1f;
    [SerializeField] float maxSpawnTime = 5f;
    [SerializeField] GameObject[] enemiesPrefab;
    bool spawn = true;

    IEnumerator Start()
    {
        while (spawn)
        {
            yield return new WaitForSeconds(Random.Range(minSpawnTime, maxSpawnTime));

            SpawnAttacker();
        }
    }

    private void SpawnAttacker()
    {
        Instantiate(enemiesPrefab[0], transform.position, transform.rotation);
    }
}
