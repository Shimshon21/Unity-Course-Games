using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Mirror;

public class UnitMovement : NetworkBehaviour
{
    [SerializeField] private NavMeshAgent agent = null;

    #region Server

    [Command]
    public void CmdMove(Vector3 position)
    {
        // Check if the requsted position is valid if not do nothing
        if (!NavMesh.SamplePosition(position,out NavMeshHit hit,1f,NavMesh.AllAreas)) { return; }
        

        agent.SetDestination(hit.position);
    }


    #endregion

}
