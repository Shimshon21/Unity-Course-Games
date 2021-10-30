using System;
using System.Collections.Generic;
using UnityEngine;
using Mirror;



public class RTSPlayer:NetworkBehaviour
{
    // Save all the units we have overall;
    [SerializeField] private List<Unit> myUnits = new List<Unit>();

    #region Server
    public override void OnStartServer()
    {

        Unit.ServerOnUnitSpawned += ServerHandleUnitSpawned;

        Unit.ServerOnUnitDeSpawned += ServerHandleUnitDeSpawned;
    }


    public override void OnStopServer()
    {
        Unit.ServerOnUnitSpawned -= ServerHandleUnitSpawned;

        Unit.ServerOnUnitDeSpawned -= ServerHandleUnitDeSpawned;
    }


    private void ServerHandleUnitSpawned(Unit unit)
    {
        Debug.Log("Add Unit To List");

        if (unit.connectionToClient.connectionId != connectionToClient.connectionId) { return; }

        Debug.Log("Add Unit To List");

        myUnits.Add(unit);
    }


    private void ServerHandleUnitDeSpawned(Unit unit)
    {
        if (unit.connectionToClient.connectionId != connectionToClient.connectionId) { return; }

        myUnits.Remove(unit);
    }

    #endregion


    #region Client
    public override void OnStartAuthority()
    {
        if (NetworkServer.active) return;

        Unit.AuthortyOnUnitSpawned += AuthortyHandleUnitSpawned;
        Unit.AuthortyOnUnitDeSpawned += AuthortyHandleUnitDeSpawned;
    }


    public override void OnStopClient()
    {
        if (!isClientOnly) return;
        Unit.AuthortyOnUnitSpawned -= AuthortyHandleUnitSpawned;
        Unit.AuthortyOnUnitDeSpawned -= AuthortyHandleUnitDeSpawned;
    }

    private void AuthortyHandleUnitDeSpawned(Unit unit)
    {
        if (!hasAuthority) return;
        myUnits.Add(unit);
    }

    private void AuthortyHandleUnitSpawned(Unit unit)
    {
        if (!hasAuthority) return;
        myUnits.Remove(unit);
    }


    #endregion


    public List<Unit> GetMyUnits()
    {
        return myUnits;
    }


}
