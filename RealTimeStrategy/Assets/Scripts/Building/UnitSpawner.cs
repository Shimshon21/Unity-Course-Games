using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using Mirror;
using System;
using UnityEngine.UI;
using TMPro;

public class UnitSpawner : NetworkBehaviour, IPointerClickHandler,IPointerDownHandler,IPointerUpHandler
{
    //
    [SerializeField] private Health health = null;
    [SerializeField] private Unit unitPrefab = null;
    [SerializeField] private Transform spawnPoint = null;

    // Animation canvas spawn time.
    [SerializeField] private TMP_Text remainingUnitsText = null;
    [SerializeField] private Image unitProgressImage = null;

    // Spawn unit data.
    // Maximum allowing to spawn at certain time.
    [SerializeField] private int maxUnitQueue = 5;

    // Move range from the spawn point.
    [SerializeField] private float spawnMoveRange = 7;

    // Time duration for each unit to spawn.
    [SerializeField] private float unitSpawnDuration = 5;

    [SyncVar(hook = nameof(ClientHandleQueuedUnitsUpdated))]
    private int queuedUnits;

    [SyncVar]
    private float unitTimer;

    private float progressImageVelocity;

    private void Update()
    {
        if(isServer)
        {
            ProduceUnits();
        }
        if(isClient)
        {
            UpdateTimeDisplay();
        }
    }

    [Server]
    private void ProduceUnits()
    {
        if(queuedUnits == 0) { return; }

        unitTimer += Time.deltaTime;

        if(unitTimer < unitSpawnDuration) { return; }

        GameObject unitInstance = Instantiate(unitPrefab.gameObject, spawnPoint.position, spawnPoint.rotation);

        // Spawn a unit in the server itself to recognize the unit , and set the owner of the unit.
        // connectionToClient - Give the data of client who sent the current command.
        NetworkServer.Spawn(unitInstance, connectionToClient);

        Vector3 spawnOffSet = UnityEngine.Random.insideUnitSphere * spawnMoveRange;

        spawnOffSet.y = spawnPoint.position.y;

        UnitMovement unitMovement = unitInstance.GetComponent<UnitMovement>();

        unitMovement.ServerMove(spawnPoint.position + spawnOffSet);

        unitTimer = 0;

        queuedUnits--;
    }


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
        NetworkServer.Destroy(gameObject);
    }


    // Command on server to spawn a unit.
    [Command]
    private void CmdSpawnUnit()
    {
        if(queuedUnits == maxUnitQueue) { return; }

        RTSPlayer player = connectionToClient.identity.GetComponent<RTSPlayer>();

        if(player.GetResources() < unitPrefab.GetResourceCost()) { return; }

        queuedUnits++;

        player.SetResources(player.GetResources() - unitPrefab.GetResourceCost());
    }

    #endregion

    #region Client

    private void UpdateTimeDisplay()
    {
        float newProgress = unitTimer / unitSpawnDuration;

        if(newProgress < unitProgressImage.fillAmount)
        {
            unitProgressImage.fillAmount = newProgress;
        }
        else
        {
            unitProgressImage.fillAmount = Mathf.SmoothDamp(unitProgressImage.fillAmount, newProgress, ref progressImageVelocity, 0.1f);
        }
    }


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

    private void ClientHandleQueuedUnitsUpdated(int oldQueuedUnit, int newQueuedUnit)
    {
        remainingUnitsText.text = newQueuedUnit.ToString(); 
    }


}
    #endregion
