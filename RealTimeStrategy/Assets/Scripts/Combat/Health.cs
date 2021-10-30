using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;

public class Health : NetworkBehaviour
{
    [SerializeField] private int maxHealth = 100;

    [SyncVar(hook = nameof(HadnleHealthUpdated))]
    private int currentHealth;

    public event Action ServerOnDie;

    // Pass current health and max health.
    public event Action<int, int> ClientOnHealthUpdated;

    #region Server

    public override void OnStartServer()
    {
        currentHealth = maxHealth;
    }

    [Server]
    public void DealDamage(int damageAmount)
    {
        if(currentHealth == 0) { return; }

        currentHealth = Mathf.Max(currentHealth - damageAmount, 0);

        if(currentHealth != 0) { return; }

        Debug.Log("I am dead");

        ServerOnDie?.Invoke();
    }


    #endregion


    #region Client

    // Called on each change happned to 'currentHealth'.
    private void HadnleHealthUpdated(int oldHealth,int newHealth)
    {
        ClientOnHealthUpdated?.Invoke(newHealth, maxHealth);
    }


    #endregion



}
