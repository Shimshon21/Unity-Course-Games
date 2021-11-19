using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;


public class UnitFiring : NetworkBehaviour
{
    [SerializeField] private Targeter targeter = null;
    [SerializeField] private GameObject projectilePrefab = null;
    [SerializeField] private Transform projectileSpawnPoint = null;

    [SerializeField] private float fireRange = 5f;
    [SerializeField] private float fireRate = 1f;
    [SerializeField] private float rotationSpeed = 20f;

    private float laserFireTime;

    [ServerCallback]
    private void Update()
    {

        if(targeter.GetTargetable() == null) { return; }

        if (!CanFireAtTarget()) { return; }

        Quaternion targetRotation =
            Quaternion.LookRotation(targeter.GetTargetable().transform.position - transform.position);

        transform.rotation = Quaternion.RotateTowards(transform.rotation,targetRotation,rotationSpeed*Time.deltaTime);

        // Shoot over 
        if(Time.time > (1/fireRate) + laserFireTime)
        {
            Quaternion projectileRotation = Quaternion.LookRotation
                (targeter.GetTargetable().GetAimAtPoint().position -  projectileSpawnPoint.position);

            GameObject projectileInstance = Instantiate(projectilePrefab, projectileSpawnPoint.position,projectileRotation);

            NetworkServer.Spawn(projectileInstance,connectionToClient);

            laserFireTime = Time.time;

            Debug.Log("Shooting At: " + targeter);

        }



        
    }

    [Server]
    private bool CanFireAtTarget()
    {

        return ((targeter.GetTargetable().transform.position - transform.position).sqrMagnitude <= fireRange * fireRange);
    }

}
