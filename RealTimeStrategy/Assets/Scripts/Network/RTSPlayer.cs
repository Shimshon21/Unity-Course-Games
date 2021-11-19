using System;
using System.Collections.Generic;
using UnityEngine;
using Mirror;


// In charge of the player actions.
public class RTSPlayer:NetworkBehaviour
{
    // Following camera of the player.
    [SerializeField] private Transform cameraTransfornm = null;

    // Layers we want to show the player.
    [SerializeField] private LayerMask buildingBlockLayer = new LayerMask();
    
    // Builidngs player can instatiate in the game.
    [SerializeField] private Building[] buildings = new Building[0];
    
    // Distance range player can built from the base.
    [SerializeField] private float buildingRangeLimit = 5f;

    // The player resource.
    [SerializeField][SyncVar(hook = nameof(ClientHandleResourcesUpdated))] 
    private int resources = 500;

    [SyncVar(hook = nameof(AuthortyHandlePartyOwnerStateUpdated))] 
    private bool isPartyOwner = false;

    [SyncVar(hook = nameof(ClientHandleDisplayNameUpdated))]
    private string displayName;

    private Color teamColor = new Color();

    [SerializeField] private List<Unit> myUnits = new List<Unit>();

    private List<Building> myBuildings = new List<Building>();

    // Resource value changed.
    // (Not static because it is relvent to specific client)
    public event Action<int> ClientOnResourcesUpdated;

    public static event Action ClientOnInfoUpdated;
    public static event Action<bool> AuthortyOnPartyOwnerStateUpdated;

    // Getters.
    public string GetDisplayName(){return displayName;}

    public bool GetIsPartyOwner(){return isPartyOwner;}

    public Transform GetCameraTransform(){return cameraTransfornm;}

    public Color GetTeamColor(){return teamColor;}

    // Get client/server units list
    public List<Unit> GetMyUnits(){return myUnits;}

    public List<Building> GetMyBuildings(){return myBuildings;}

    public int GetResources(){return resources;}

    // Setters
    public void SetPartyOwner(bool state){isPartyOwner = state;}

    [Server]
    public void SetDisplayName(string displayName){this.displayName = displayName;}

    [Server]
    public void SetResources(int newResources){resources = newResources;}

    [Server]
    public void SetTeamColor(Color newTeamColor){teamColor = newTeamColor;}


    // Command the server to start the game.
    [Command]
    public void CmdStartGame()
    {
        if(!isPartyOwner) { return; }

        ((RTSNetworkManager)NetworkManager.singleton).StartGame();
    }

    // Command the server to put building if possible.
    [Command]
    public void CmdTryPlaceBuilding(int buildingId, Vector3 point)
    {
        Building buildingToPlace = getBuilidingById(buildingId);

        // Id is not existed.
        if (!buildingToPlace) return; 

        // Not enough resources.
        if (resources < buildingToPlace.GetPrice()) return; 

        BoxCollider buildingCollider = buildingToPlace.GetComponent<BoxCollider>();
        
        //
        if (!CanPlaceBuilding(buildingCollider, point))  return; 

        GameObject buildingInstance = Instantiate(buildingToPlace.gameObject, point, buildingToPlace.transform.rotation);

        NetworkServer.Spawn(buildingInstance, connectionToClient);

        SetResources(resources - buildingToPlace.GetPrice());
    }

    // Return the wanted building by given Id.
    private Building getBuilidingById(int buildingId)
    {
        // Set the building the id is match to.
        foreach (Building building in buildings)
        {
            if (buildingId == building.GetId())
                return building;
        }
        return null;
    }

    // Check if we allowed to put the wanted building.
    public bool CanPlaceBuilding(BoxCollider buildingCollider, Vector3 point)
    {

        // Check if we collide existed object collider.
        if (Physics.CheckBox(point + buildingCollider.center, buildingCollider.size / 2, Quaternion.identity, buildingBlockLayer))
        {
            return false;
        }

        // Check building around existed building area.
        foreach (Building building in myBuildings)
        {
            if ((point - building.transform.position).sqrMagnitude
                <= buildingRangeLimit * buildingRangeLimit)
            {
                return true;
            }
        }

        return false;
    }

    #region Server

    public override void OnStartServer()
    {
        // Subscribe to Unit
        Unit.ServerOnUnitSpawned += ServerHandleUnitSpawned;
        Unit.ServerOnUnitDeSpawned += ServerHandleUnitDeSpawned;

        // Subscribe to Building
        Building.ServerOnBuildingSpawned += ServerHandleBuildingSpawn;
        Building.ServerOnBuildingDeSpawned += ServerHandleBuildingDeSpawn;

        // Dont delete player between scenes switch.
        DontDestroyOnLoad(gameObject);
    }


    public override void OnStopServer()
    {
        Unit.ServerOnUnitSpawned -= ServerHandleUnitSpawned;
        Unit.ServerOnUnitDeSpawned -= ServerHandleUnitDeSpawned;

        Building.ServerOnBuildingSpawned -= ServerHandleBuildingSpawn;
        Building.ServerOnBuildingDeSpawned -= ServerHandleBuildingDeSpawn;
    }


    // Add new unit to the server 'myUnits' list.
    private void ServerHandleUnitSpawned(Unit unit)
    {
        if (isUnitBelongToClient(unit))
            myUnits.Add(unit);
    }


    // Remove unit from the server 'myUnits' list
    private void ServerHandleUnitDeSpawned(Unit unit)
    {
        if (isUnitBelongToClient(unit))
            myUnits.Remove(unit);
    }


    private void ServerHandleBuildingSpawn(Building building)
    {
        if (isBuildingBelongToClient(building))
            myBuildings.Add(building);
    }


    private void ServerHandleBuildingDeSpawn(Building building)
    {
        if (isBuildingBelongToClient(building))
            myBuildings.Remove(building);
    }

    private bool isUnitBelongToClient(Unit unit)
    {
        return unit.connectionToClient.connectionId ==
            connectionToClient.connectionId;
    }

    private bool isBuildingBelongToClient(Building building)
    {
        return (building.connectionToClient.connectionId ==
            connectionToClient.connectionId);
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

    public override void OnStartClient()
    {
        if(NetworkServer.active) return;

        // When we switch from menu to map scene,
        // the object are destroyed unless we say
        // otherwise.
        DontDestroyOnLoad(gameObject);

        ((RTSNetworkManager)NetworkManager.singleton).players.Add(this);

    }

    // Unsubscribe client and client that is also a server(Authorty) to unit spawn event.
    public override void OnStopClient()
    {
        ClientOnInfoUpdated?.Invoke();

        if (!isClientOnly) return;

        ((RTSNetworkManager)NetworkManager.singleton).players.Remove(this);

        if(!hasAuthority) return;

        Unit.AuthortyOnUnitSpawned -= AuthortyHandleUnitSpawned;
        Unit.AuthortyOnUnitDeSpawned -= AuthortyHandleUnitDeSpawned;

        Building.AuthortyOnBuildingSpawned -= AuthortyHandleBuildingSpawn;
        Building.AuthortyOnBuildingDeSpawned -= AuthortyHandleBuildingDeSpawn;
    }

    private void AuthortyHandlePartyOwnerStateUpdated(bool oldState, bool newState)
    {
        if (!hasAuthority) { return; }

        AuthortyOnPartyOwnerStateUpdated?.Invoke(newState);
    }

    // Add unit to authorty 'list'.
    private void AuthortyHandleUnitDeSpawned(Unit unit)
    {
        if (!hasAuthority) return;
        myUnits.Remove(unit);
    }


    // Remove unit to authorty 'list'.
    private void AuthortyHandleUnitSpawned(Unit unit)
    {
        if (!hasAuthority) return;
        myUnits.Add(unit);
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


    private void ClientHandleResourcesUpdated(int oldResource,int newResource)
    {
        Debug.Log("Current Resource" + newResource);
        ClientOnResourcesUpdated?.Invoke(newResource);
    }

    private void ClientHandleDisplayNameUpdated(string oldName,string newName)
    {
        ClientOnInfoUpdated?.Invoke();
    }
    #endregion

}
