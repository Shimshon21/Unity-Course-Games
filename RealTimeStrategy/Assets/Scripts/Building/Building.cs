using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : NetworkBehaviour
{
    [SerializeField] private GameObject buildingPreview = null;
    [SerializeField] private Sprite icon = null;
    [SerializeField] private int id = -1;
    [SerializeField] private int price = 100;


    // Server
    // Server Unit spawned event.
    public static event Action<Building> ServerOnBuildingSpawned;
    // Server Unit Despawned event.
    public static event Action<Building> ServerOnBuildingDeSpawned;

    // Authorty
    // Authorty Unit spawned event. 
    public static event Action<Building> AuthortyOnBuildingSpawned;
    // Authorty Unit despawned event. 
    public static event Action<Building> AuthortyOnBuildingDeSpawned;

    public GameObject GetBuildingPreview()
    {
        return buildingPreview;
    }

    public Sprite GetIcon()
    {
        return icon;
    }

    public int GetId()
    {
        return id;
    }


    public int GetPrice()
    {
        return price;
    }

    #region Server

    //
    public override void OnStartServer()
    {
        ServerOnBuildingSpawned?.Invoke(this);
    }

    //
    public override void OnStopServer()
    {
        ServerOnBuildingDeSpawned?.Invoke(this);
    }

    #endregion


    #region Client

    public override void OnStartAuthority()
    {
        AuthortyOnBuildingSpawned?.Invoke(this);
    }


    public override void OnStopClient()
    {
        AuthortyOnBuildingDeSpawned?.Invoke(this);
    }

    #endregion
}
