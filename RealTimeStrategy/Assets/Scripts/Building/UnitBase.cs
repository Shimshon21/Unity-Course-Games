using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;

public class UnitBase : NetworkBehaviour
{
    [SerializeField] private Health health = null;


    public static event Action<int> ServerOnPlayerDie;
    public static event Action<UnitBase> ServerOnBaseSpawned;
    public static event Action<UnitBase> ServerOnBaseDeSpawned;



    #region Server

    public override void OnStartServer()
    {
        health.ServerOnDie += HandleBaseDestroyed;

        ServerOnBaseSpawned?.Invoke(this);
    }

    public override void OnStopServer()
    {
        ServerOnBaseDeSpawned?.Invoke(this);

        health.ServerOnDie -= HandleBaseDestroyed;
    }

    [Server]
    private void HandleBaseDestroyed()
    {
        ServerOnPlayerDie?.Invoke(connectionToClient.connectionId);

        NetworkServer.Destroy(gameObject);
    }

    #endregion

    #region Client

    #endregion
}
