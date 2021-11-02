using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.Events;
using System;

// A specific unit class with attributes of selection
public class Unit : NetworkBehaviour
{

    [SerializeField] private Health health;
    [SerializeField] private int resourceCost;
   

    [SerializeField] private UnitMovement unitMovement = null;
    [SerializeField] private Targeter targeter;

    [SerializeField] private UnityEvent onSelected = null;
    [SerializeField] private UnityEvent onDeselected = null;

    // Server

    // Server Unit spawned event.
    public static event Action<Unit> ServerOnUnitSpawned;
    // Server Unit Despawned event.
    public static event Action<Unit> ServerOnUnitDeSpawned;

    // Authorty

    // Authorty Unit spawned event. 
    public static event Action<Unit> AuthortyOnUnitSpawned;
    // Authorty Unit despawned event. 
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


    public int GetResourceCost()
    {
        return resourceCost;
    }

    #region Server
    // Subscribe to event of the unit die
    // and invoke when spawned event.
    public override void OnStartServer()
    {
        health.ServerOnDie += ServerHandleDie;

        ServerOnUnitSpawned?.Invoke(this);

    }

    // Unsubscribe to event of the unit die
    // and invoke when despawned event.
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


    // Authority invoke unit spawned event.
    public override void OnStartAuthority()
    {
        if (!hasAuthority) { return; }
        AuthortyOnUnitSpawned?.Invoke(this);
    }

    // Client invoke unit despawned event.
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
