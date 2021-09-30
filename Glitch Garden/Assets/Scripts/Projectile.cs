using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField]
    float
        speed = 1f,
        speedOfSpin = 1f,
        damage = 100f;


    // Update is called once per frame
    void Update()
    {
        ShootMovment();
    }


    private void ShootMovment()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime, Space.World);

        transform.Rotate(0, 0, -speedOfSpin);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        var health = collision.GetComponent<Health>();
        var attacker = collision.GetComponent<Attacker>();

        if (health && attacker)
        {
            health.DealDamage(damage);

            Destroy(gameObject);
        }
    }


}
