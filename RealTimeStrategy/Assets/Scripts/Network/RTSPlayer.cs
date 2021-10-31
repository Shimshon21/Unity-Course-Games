using System;
using System.Collections.Generic;
using UnityEngine;
using Mirror;


// In charge of the player actions.
public class RTSPlayer:NetworkBehaviour
{
    [SerializeField] private Building[] buildings = new Building[0];

    // Save all the units we have overall;
    private List<Unit> myUnits = new List<Unit>();

    private List<Building> myBuildings = new List<Building>();


    // Get client/server units list
    public List<Unit> GetMyUnits()
    {
        return myUnits;
    }


    public List<Building> GetMyBuildings()
    {
        return myBuildings;
    }

    #region Server


    public override void OnStartServer()
    {
        // Subscribe to unit
        Unit.ServerOnUnitSpawned += ServerHandleUnitSpawned;
        Unit.ServerOnUnitDeSpawned += ServerHandleUnitDeSpawned;

        // Subscribe to building
        Building.ServerOnBuildingSpawned += ServerHandleBuildingSpawn;
        Building.ServerOnBuildingDeSpawned += ServerHandleBuildingDeSpawn;
    }


    public override void OnStopServer()
    {
        // Unsubscribe to building
        Unit.ServerOnUnitSpawned -= ServerHandleUnitSpawned;
        Unit.ServerOnUnitDeSpawned -= ServerHandleUnitDeSpawned;

        Building.ServerOnBuildingSpawned -= ServerHandleBuildingSpawn;
        Building.ServerOnBuildingDeSpawned -= ServerHandleBuildingDeSpawn;
    }


    // Add new unit to the server 'units' list.
    private void ServerHandleUnitSpawned(Unit unit)
    {
        if (unit.connectionToClient.connectionId != connectionToClient.connectionId) { return; }

        myUnits.Add(unit);
    }


    // Remove unit from the server 'units' list
    private void ServerHandleUnitDeSpawned(Unit unit)
    {
        if (unit.connectionToClient.connectionId != connectionToClient.connectionId) { return; }

        myUnits.Remove(unit);
    }


    private void ServerHandleBuildingSpawn(Building building)
    {
        if (building.connectionToClient.connectionId != connectionToClient.connectionId) { return; }

        myBuildings.Add(building);
    }


    private void ServerHandleBuildingDeSpawn(Building building)
    {
        if (building.connectionToClient.connectionId != connectionToClient.connectionId) { return; }

        myBuildings.Remove(building);
    } 


    [Command]
    public void CmdTryPlaceBuilding(int buildingId,Vector3 transform)
    {
        Building buildingToPlace = null;

        foreach(Building building in buildings)
        {
            if(buildingId == building.GetId())
            {
                buildingToPlace = building;
                break;
            }
        }

        if (!buildingToPlace) { return; }

        GameObject buildingInstance = Instantiate(buildingToPlace.gameObject, transform, buildingToPlace.transform.rotation);

        NetworkServer.Spawn(buildingInstance,connectionToClient);
    }
    #endregion


    #region Client

    // Subscribe client and client that is also a server((Authorty)) to unit spawn event.
    public override void OnStartAuthority()
    {
        if (NetworkServer.active) return;

        Unit.AuthortyOnUnitSpawned += AuthortyHandleUnitSpawned;
        Unit.AuthortyOnUnitDeSpawned += AuthortyHandleUnitDeSpawned;

        Building.AuthortyOnBuildingSpawned += AuthortyHandleBuildingSpawn;
        Building.AuthortyOnBuildingDeSpawned += AuthortyHandleBuildingDeSpawn;

    }


    // Unsubscribe client and client that is also a server(Authorty) to unit spawn event.
    public override void OnStopClient()
    {
        if (!isClientOnly) return;
        Unit.AuthortyOnUnitSpawned -= AuthortyHandleUnitSpawned;
        Unit.AuthortyOnUnitDeSpawned -= AuthortyHandleUnitDeSpawned;

        Building.AuthortyOnBuildingSpawned -= AuthortyHandleBuildingSpawn;
        Building.AuthortyOnBuildingDeSpawned -= AuthortyHandleBuildingDeSpawn;
    }

    // Add unit to authorty 'list'.
    private void AuthortyHandleUnitDeSpawned(Unit unit)
    {
        if (!hasAuthority) return;
        myUnits.Add(unit);
    }


    // Remove unit to authorty 'list'.
    private void AuthortyHandleUnitSpawned(Unit unit)
    {
        if (!hasAuthority) return;
        myUnits.Remove(unit);
    }

    private void AuthortyHandleBuildingSpawn(Building building)
    {
        if (!hasAuthority) return;
        myBuildings.Add(building);
    }

    private void AuthortyHandleBuildingDeSpawn(Building building)
    {
        if (!hasAuthority) return;
        myBuildings.Remove(building);
    }
    #endregion



}
