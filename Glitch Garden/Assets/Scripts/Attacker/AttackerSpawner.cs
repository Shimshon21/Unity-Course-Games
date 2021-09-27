using System.Collections;
using UnityEngine;

public class AttackerSpawner : MonoBehaviour
{
    [SerializeField] float minSpawnTime = 1f;
    [SerializeField] float maxSpawnTime = 5f;
    [SerializeField] Attacker[] enemiesPrefab;
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
        int enemyType = Random.Range(0, enemiesPrefab.Length);

        Spawn(enemiesPrefab[enemyType]);

    }

    private void Spawn(Attacker attacker)
    {
        Attacker newAttacker = Instantiate
            (attacker, transform.position, transform.rotation) as Attacker;

        newAttacker.transform.parent = transform;
    }


    public void StopSpawning()
    {

        spawn = false;
    }

}
