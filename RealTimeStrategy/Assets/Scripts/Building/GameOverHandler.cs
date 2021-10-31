using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;

public class GameOverHandler : NetworkBehaviour
{
    public static event Action ServerOnGameOver;

    public static event Action<string> ClientOnGameOver;

    [SerializeField] private List<UnitBase> bases;


    #region Server
    public override void OnStartServer()
    {
        UnitBase.ServerOnBaseSpawned += ServerHandleBaseSpawned;
        UnitBase.ServerOnBaseDeSpawned += ServerHandleBaseDeSpawned;
    }


    public override void OnStopServer()
    {
        UnitBase.ServerOnBaseSpawned -= ServerHandleBaseSpawned;
        UnitBase.ServerOnBaseDeSpawned -= ServerHandleBaseDeSpawned;
    }


    private void ServerHandleBaseSpawned(UnitBase unitBase)
    {
        Debug.Log("Base Added ");
        bases.Add(unitBase);

    }


    private void ServerHandleBaseDeSpawned(UnitBase unitBase)
    {
        bases.Remove(unitBase);

        if(bases.Count != 1) { return; }

        int playerId = bases[0].connectionToClient.connectionId;

        RpcGameOver($"Player {playerId}");

        ServerOnGameOver?.Invoke();
    }
    #endregion


    #region Client

    // Notify all players the game is over.
    [ClientRpc]
    private void RpcGameOver(string winner)
    {
        ClientOnGameOver?.Invoke(winner);
    }

    #endregion
}
