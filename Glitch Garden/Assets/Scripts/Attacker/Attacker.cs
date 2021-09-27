using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attacker : MonoBehaviour
{

    GameObject currentTarget;

    float currentSpeed = 0f;

    private void Awake()
    {
        FindObjectOfType<LevelController>().AttackerSpawned();
    }

    private void OnDestroy()
    {
        FindObjectOfType<LevelController>().AttackerKilled();
    }

    void Start()
    {

    }

    void Update()
    {
        transform.Translate(Vector2.left * Time.deltaTime * currentSpeed);

        UpdateAnimationState();
    
    }

    private void UpdateAnimationState()
    {
        if(!currentTarget)
        {
            GetComponent<Animator>().SetBool("IsAttacking", false);
        }
    }

    public void SetMovementSpeed(float movementSpeed)
    {
        currentSpeed = movementSpeed;
    }


    public void Attack(GameObject target)
    {
        GetComponent<Animator>().SetBool("IsAttacking", true);
        currentTarget = target;
    }


    public void StrikeCurrentTarget(float damage)
    {
        if (!currentTarget){ return; }

        Health health = currentTarget.GetComponent<Health>();
        if(health)
        {
            health.DealDamage(damage);
        }
    }


}
