using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class GameOverHandler : NetworkBehaviour
{
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
        bases.Add(unitBase);

    }


    private void ServerHandleBaseDeSpawned(UnitBase unitBase)
    {
        bases.Remove(unitBase);

        if(bases.Count != 1) { return; }

        Debug.Log("Game Over!");
    }
    #endregion


    #region Client
    #endregion
}
