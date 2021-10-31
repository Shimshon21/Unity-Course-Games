using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using Mirror;
using System;

public class UnitSpawner : NetworkBehaviour, IPointerClickHandler,IPointerDownHandler,IPointerUpHandler
{
    [SerializeField] private Health health = null;
    [SerializeField] private GameObject unitPrefab = null;
    [SerializeField] private Transform spawnPoint = null;

    #region Server


    public override void OnStartServer()
    {
        health.ServerOnDie += ServerHandleDie;
    }

    public override void OnStopServer()
    {
        health.ServerOnDie -= ServerHandleDie;
    }


    [Server]
    private void ServerHandleDie()
    {
        //NetworkServer.Destroy(gameObject);
    }


    // Command on server to spawn a unit.
    [Command]
    private void CmdSpawnUnit()
    {
        // Create new unit in spawPoint position
        GameObject unitInstance = Instantiate(unitPrefab,
            spawnPoint.position,
            spawnPoint.rotation);

        // Spawn a unit in the server itself to recognize the unit , and set the owner of the unit.
        // connectionToClient - Give the data of client who sent the current command.
        NetworkServer.Spawn(unitInstance, connectionToClient);
    }

    #endregion

    #region Client
    // Call this function on each time we click on the current object.
    public void OnPointerDown(PointerEventData eventData)
    {

    }

    public void OnPointerUp(PointerEventData eventData)
    {

    }
    public void OnPointerClick(PointerEventData eventData)
    {


        // Check if left button clicked.
        if (eventData.button != PointerEventData.InputButton.Left) { return; }

        // Cheeck we are the owner.
        if (!hasAuthority) { return; }

        CmdSpawnUnit();
    }


}
    #endregion
