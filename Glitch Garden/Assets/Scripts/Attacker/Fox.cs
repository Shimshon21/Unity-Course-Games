using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fox : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject otherObject = collision.gameObject;

        // Colliding with gravestoen.
        if (otherObject.GetComponent<Gravestone>())
        {
            Jump();
        }

        else if (otherObject.GetComponent<Defender>())
        {
            GetComponent<Attacker>().Attack(otherObject);
        }

    }

    private void Jump()
    {
        GetComponent<Animator>().SetTrigger("JumpTrigger");
    }
}
