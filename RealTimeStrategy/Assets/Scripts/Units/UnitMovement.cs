using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Mirror;
using System;

public class UnitMovement : NetworkBehaviour
{
    [SerializeField] private NavMeshAgent agent = null;
    [SerializeField] private Targeter targeter = null;
    [SerializeField] private float chaseRange = 10f;


    #region Server

    public override void OnStartServer()
    {
        GameOverHandler.ServerOnGameOver += ServerHandleGameOver;
    }

    [Server]
    public override void OnStopServer()
    {
        GameOverHandler.ServerOnGameOver -= ServerHandleGameOver;
    }

    private void ServerHandleGameOver()
    {
        agent.ResetPath();
    }

    [ServerCallback]
    private void Update()
    {
        PathFindingTweak();
    }

    private void PathFindingTweak()
    {
        Targetable target = targeter.GetTargetable();

        if(target != null)
        {

            ChaseTarget(target);

            return;
        }

        // Prevent from reseting path
        if (!agent.hasPath) { return; }

        if (agent.remainingDistance > agent.stoppingDistance) { return; }

        agent.ResetPath();
    }

    private void ChaseTarget(Targetable target)
    {
        // We can call aswell Vector3.distance() but it is uses square root calculations which is slow.
        // caclulation of d = sqrt((X1 - X2)^2,(Y1-Y2)^2) =>^2=> d^2 = (X1-X2),(Y1-Y2).
        if ((target.transform.position - transform.position).sqrMagnitude > chaseRange * chaseRange)
        {
            agent.SetDestination(target.transform.position);
        }
        else if (agent.hasPath)
        {
            agent.ResetPath();
        }
    }

    [Command]
    public void CmdMove(Vector3 position)
    {
        targeter.ClearTarget();

        // Check if the requsted position is valid if not do nothing
        if (!NavMesh.SamplePosition(position,out NavMeshHit hit,1f,NavMesh.AllAreas)) { return; }
        

        agent.SetDestination(hit.position);
    }


    #endregion

}
