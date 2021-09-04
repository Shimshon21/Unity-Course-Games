using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{

    // configuration paramaters
    [Header("Player")]
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] int health = 200;
    [SerializeField][Range(0,1)] float deathSoundVolume = 1f;
    [SerializeField] AudioClip deathSFX;

    [Header("Projectile")]
    [SerializeField] float projectileSpeed = 10f;
    [SerializeField] float projectileFiringPeriod = 3f;
    [SerializeField] float projectileSoundVolume = 1f;
    [SerializeField] GameObject laserPrefab;
    [SerializeField] AudioClip laserSFX; 

    Coroutine firingCoroutine;


    //int count = 0;

    float padding = 1f;

    float xMax,xMin,yMax,yMin;


    // Start is called before the first frame update
    void Start()
    {
        SetUpMoveBoundries();
    }

    private void SetUpMoveBoundries()
    {
        Camera gameCamera = Camera.main;

        xMin = gameCamera.ViewportToWorldPoint
            (new Vector3(0, 0, 0)).x+ padding;
        xMax = gameCamera.ViewportToWorldPoint
            (new Vector3(1, 0, 0)).x - padding;

        yMin = gameCamera.ViewportToWorldPoint
            (new Vector3(0, 0, 0)).y + padding;
        yMax = gameCamera.ViewportToWorldPoint
            (new Vector3(0, 1, 0)).y - padding;
    }



    // Update is called once per frame
    void Update()
    {
        Move();
        FireLaser();
    }

    private void FireLaser()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            firingCoroutine = StartCoroutine(FireContinuously());
            AudioSource.PlayClipAtPoint
                (laserSFX, Camera.main.transform.position,projectileSoundVolume);
        }

        if(Input.GetButtonUp("Fire1"))
        {
            StopCoroutine(firingCoroutine);
        }
    }


    IEnumerator FireContinuously()
    {

        while (true)
        {
            GameObject laser = Instantiate
                (laserPrefab, transform.position, Quaternion.identity) as GameObject;

            laser.GetComponent<Rigidbody2D>().velocity =
                new Vector2(0, projectileSpeed);

            yield return new WaitForSeconds
                (projectileFiringPeriod);
        }
    }


    //Set the player movment
    private void Move()
    {
        //Movment direction speed.
        var deltaX = Input.GetAxis("Horizontal")* 
            Time.deltaTime * moveSpeed;
        var deltaY = Input.GetAxis("Vertical") * 
            Time.deltaTime * moveSpeed;

        //Set new movment positions.
        var newXPos = Mathf.Clamp
            (transform.position.x + deltaX,xMin,xMax);
        var newYPos = Mathf.Clamp
            (transform.position.y + deltaY, yMin, yMax);

        //Apply the new positions.
        transform.position = new Vector2(newXPos, newYPos);
        
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
                (deathSFX, Camera.main.transform.position,deathSoundVolume);
            Destroy(gameObject);
        }
    }
}
