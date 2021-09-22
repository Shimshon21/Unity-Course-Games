using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attacker : MonoBehaviour
{

    [SerializeField] GameObject deathVFX;

    float currentSpeed = 0f;


    void Start()
    {

    }

    void Update()
    {
        transform.Translate(Vector2.left * Time.deltaTime * currentSpeed);
    }


    public void SetMovementSpeed(float movementSpeed)
    {
        currentSpeed = movementSpeed;
    }

}
