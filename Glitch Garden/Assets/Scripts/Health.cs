using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] float health = 100f;


    [SerializeField] GameObject deathVFXPrefab;
    float DESTROY_VFX_DELAY = 1f;

    public void DealDamage(float damage)
    {
        health -= damage;

        if (health < 0)
        {

            Destroy(gameObject);

            TriggerDeathVFX();

        }

    }

    private void TriggerDeathVFX()
    {
        if (deathVFXPrefab)
        {
            var deathVFX = Instantiate(deathVFXPrefab,
                transform.position, transform.rotation);

            Destroy(deathVFX, DESTROY_VFX_DELAY);
        }
    }
}
