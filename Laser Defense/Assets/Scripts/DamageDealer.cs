using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Damage manager
public class DamageDealer : MonoBehaviour
{
    [SerializeField] int damage = 100;

     public int GetDamage()
    {
       
        return damage;
    }

    public void hit()
    {
        Destroy(gameObject);
    }


}
