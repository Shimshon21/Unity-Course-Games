using UnityEngine;

public class Shooter : MonoBehaviour
{
    [SerializeField] GameObject projectilePrefab,gun;

    [SerializeField] float projectileSpeed = 2f;
    public void Shoot()
    {


        var projectile = Instantiate(projectilePrefab, gun.transform.position, transform.rotation);
        return;
    }


}
