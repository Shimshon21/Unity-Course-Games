using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Targeter : NetworkBehaviour
{
    private Targetable target;


    #region Server

    public override void OnStartServer()
    {
        GameOverHandler.ServerOnGameOver += ServerHandleGameOver;
    }

    public override void OnStopServer()
    {
        GameOverHandler.ServerOnGameOver -= ServerHandleGameOver;
    }

    [Command]
    public void CmdSetTarget(GameObject targetObject)
    {
        if(!targetObject.TryGetComponent<Targetable>(out Targetable targetable)) { return; }

        this.target = targetable;
    }

    [Server]
    public void ClearTarget()
    {
        target = null;
    }
    #endregion


    public Targetable GetTargetable()
    {
        return target;
    }

    [Server]
    public void ServerHandleGameOver()
    {
        ClearTarget();
    }


}
