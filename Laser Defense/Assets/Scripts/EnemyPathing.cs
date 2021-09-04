using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPathing : MonoBehaviour
{
    // Wave configuration
    [SerializeField] WaveConfig waveConfig;

    //Type transform to store the positions itself of the paths.
    [SerializeField] List<Transform> waypoints;

    //Index of the waypoint.
    int waypointIndex = 0;



    // Start is called before the first frame update
    void Start()
    {
        waypoints = waveConfig.GetWaypoints();

        transform.position = waypoints[waypointIndex].transform.position;

        foreach(var waypoint in waypoints)
            Debug.Log(waypoint.position);
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    //
    public void setWaveConfig(WaveConfig waveConfig)
    {
        this.waveConfig = waveConfig;
    }
   
    // Move the enemy according a specific path.
    private void Move()
    {
        if (waypointIndex <= waypoints.Count - 1)
        {
            // The next target position wanted
            var targetPosition = waypoints[waypointIndex].transform.position;

            // Movment speed
            var movementThisFrame = waveConfig.GetMoveSpeed() * Time.deltaTime;

            // Update new position by 'MoveTowards'
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, movementThisFrame);

            if (transform.position == targetPosition)
            {
                waypointIndex++;

            }


        }
        else
        {
            Destroy(gameObject);
        }


    }
}

