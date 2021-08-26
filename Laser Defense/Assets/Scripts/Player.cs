using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float projectileSpeed = 10f;
    [SerializeField] float projectileFiringPeriod = 3f;
    [SerializeField] GameObject laserPrefab;


    Coroutine firingCoroutine;


    int count = 0;

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

        xMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x+ padding;
        xMax = gameCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - padding;

        yMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y + padding;
        yMax = gameCamera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y - padding;
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
            GameObject laser = Instantiate(laserPrefab, transform.position, Quaternion.identity) as GameObject;

            laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, projectileSpeed);

            yield return new WaitForSeconds(projectileFiringPeriod);
        }
    }


    //Set the player movment
    private void Move()
    {
        //Movment direction speed.
        var deltaX = Input.GetAxis("Horizontal")* Time.deltaTime * moveSpeed;
        var deltaY = Input.GetAxis("Vertical") * Time.deltaTime * moveSpeed;

        //Set new movment positions.
        var newXPos = Mathf.Clamp(transform.position.x + deltaX,xMin,xMax);
        var newYPos = Mathf.Clamp(transform.position.y + deltaY, yMin, yMax);

        //Apply the new positions.
        transform.position = new Vector2(newXPos, newYPos);
        
    }
}
