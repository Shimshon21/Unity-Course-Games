using System;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    [SerializeField] GameObject projectilePrefab,gun;

    [SerializeField] float projectileSpeed = 2f;

    AttackerSpawner myLaneSpawner;

    Animator animator;

    const string PROJECTILE_PARENT_NAME = "Projectiles";

    GameObject projectileParent;

    private void Start()
    {
        SetLaneSpawner();

        animator = GetComponent<Animator>();

        CreateDefenderParent();
    }


    private void SetLaneSpawner()
    {
        AttackerSpawner[] spawners = FindObjectsOfType<AttackerSpawner>();

        foreach (AttackerSpawner spawner in spawners)
        {
            bool IsCloseEnough = Mathf.Abs(spawner.transform.position.y - transform.position.y) <= Mathf.Epsilon;

            if (IsCloseEnough)
            {
                myLaneSpawner = spawner;
            }
        }
    }


    private void Update()
    {
        if(IsAttackerInLane())
        { 
            animator.SetBool("IsAttacking",true);
        }
        else
        {
            animator.SetBool("IsAttacking", false);
        }
    }

    private bool IsAttackerInLane()
    {
        if (myLaneSpawner.transform.childCount <= 0)
        {
            return false;
        }

        return true;
    }

    public void Shoot()
    {
        var projectile = Instantiate(projectilePrefab, gun.transform.position, transform.rotation);

        projectile.transform.parent = projectileParent.transform;
    }


    private void CreateDefenderParent()
    {
        projectileParent = GameObject.Find(PROJECTILE_PARENT_NAME);
        if (!projectileParent)
        {
            projectileParent = new GameObject(PROJECTILE_PARENT_NAME);

        }
    }

}
