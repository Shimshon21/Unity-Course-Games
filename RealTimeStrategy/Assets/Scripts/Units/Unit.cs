using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.Events;
using System;

// A specific unit class with attributes of selection
// 
public class Unit : NetworkBehaviour
{
    [SerializeField] private Health health;
   

    [SerializeField] private UnitMovement unitMovement = null;
    [SerializeField] private Targeter targeter;

    [SerializeField] private UnityEvent onSelected = null;
    [SerializeField] private UnityEvent onDeselected = null;

    // Server
    public static event Action<Unit> ServerOnUnitSpawned;

    public static event Action<Unit> ServerOnUnitDeSpawned;

    //Client
    public static event Action<Unit> AuthortyOnUnitSpawned;

    public static event Action<Unit> AuthortyOnUnitDeSpawned;


    // Return the movment setting of this unit.
    public UnitMovement GetUnitMovement()
    {
        return unitMovement;
    }

    public Targeter GetTargeter()
    {
        return targeter;
    }

    #region Server
    public override void OnStartServer()
    {
        health.ServerOnDie += ServerHandleDie;

        ServerOnUnitSpawned?.Invoke(this);

    }


    public override void OnStopServer()
    {
        health.ServerOnDie -= ServerHandleDie;

        ServerOnUnitDeSpawned?.Invoke(this);

    }


    [Server]
    private void ServerHandleDie()
    {
        NetworkServer.Destroy(gameObject);
    }
    #endregion

    #region Client


    public override void OnStartAuthority()
    {
        if (!hasAuthority) { return; }
        AuthortyOnUnitSpawned?.Invoke(this);
    }

    
    public override void OnStopClient()
    {
        if (!isClientOnly || !hasAuthority) { return; }
        AuthortyOnUnitDeSpawned?.Invoke(this);
    }

    // Showing the token of the current unit.
    [Client]
    public void Select()
    {
        if (!hasAuthority) return;
        onSelected?.Invoke();
    }

    // Removing the token of the current unit.
    [Client]
    public void Deselect()
    {
        if (!hasAuthority) return;
        onDeselected?.Invoke();
    }

    #endregion

}
