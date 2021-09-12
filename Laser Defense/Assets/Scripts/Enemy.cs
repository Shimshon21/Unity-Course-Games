using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    // Enemy configuration
    [Header("Enemy")]
    [SerializeField] float health = 100;
    [SerializeField] float shotCounter;
    [SerializeField] float minTimeBetweeenShots = 0.2f;
    [SerializeField] float maxTimeBetweenShots = 3f;
    [SerializeField][Range(0, 1)] float deathSoundVolume = 1f;
    [SerializeField] AudioClip deathSFX;


    // Laser configuration
    [Header("Projectile")]
    [SerializeField] float projectileFiringPeriod = 10f;
    [SerializeField] GameObject laserPrefab;
    [SerializeField][Range(0, 1)] float projectileSoundVolume = 0.8f;
    [SerializeField] AudioClip laserSFX;
    AudioSource audioSource;


    [Header("Explosion")]
    [SerializeField] GameObject deathVFX;
    [SerializeField] float durationOfExplosion = 1f;
    

    // Start is called before the first frame update
    void Start()
    {
        shotCounter = Random.Range
            (minTimeBetweeenShots, maxTimeBetweenShots);

    }


    // Update is called once per frame
    void Update()
    {
        CountDownAndShoot();
    }

    private void CountDownAndShoot()
    {
        shotCounter -= Time.deltaTime;
        if(shotCounter <= 0f)
        {
            Fire();

            shotCounter = Random.Range
                (minTimeBetweeenShots, maxTimeBetweenShots);
        }
    }

    private void Fire()
    {
        GameObject laser = Instantiate
            (laserPrefab, transform.position, Quaternion.identity) as GameObject;

        laser.GetComponent<Rigidbody2D>().velocity = 
            new Vector2(0,(-1* projectileFiringPeriod));

        AudioSource.PlayClipAtPoint
            (laserSFX,Camera.main.transform.position,projectileSoundVolume);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        DamageDealer damageDealer =
            collision.gameObject.GetComponent<DamageDealer>();
        if (!damageDealer) return;

        ProcessHit(damageDealer);

    }

    private void ProcessHit(DamageDealer damageDealer)
    {
        health -= damageDealer.GetDamage();

        damageDealer.hit();

        if (health <= 0)
        {
            AudioSource.PlayClipAtPoint
                (deathSFX, Camera.main.transform.position, deathSoundVolume);
           
            Die();
        }
    }

    private void Die()
    {
        FindObjectOfType<GameSession>().AddScore();

        Destroy(gameObject);

        GameObject explosion = Instantiate
            (deathVFX, transform.position, transform.rotation);
      
        Destroy(explosion, durationOfExplosion);
    }
}
