using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;


public class UnitProjectile : NetworkBehaviour
{
    [SerializeField] private Rigidbody rb = null;
    [SerializeField] private int damageToDeal = 20;
    [SerializeField] private float destroyAfterSeconds = 5;
    [SerializeField] private float lauchForce = 10f;



    // Start is called before the first frame update
    void Start()
    {
        rb.velocity = transform.forward * lauchForce;
        
    }

    public override void OnStartServer()
    {
        // Call a function after destroyAfterSeconds
        Invoke(nameof(DestroySelf),destroyAfterSeconds);
    }

    [ServerCallback]
    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<NetworkIdentity>(out NetworkIdentity networkIdentity))
        {
            if(networkIdentity.connectionToClient == connectionToClient) { return; }


            if(other.TryGetComponent<Health>(out Health health))
            {
                health.DealDamage(damageToDeal);
            }
        }

        DestroySelf();
        
    }

    [Server]
    private void DestroySelf()
    {
        NetworkServer.Destroy(gameObject);
    }
}
