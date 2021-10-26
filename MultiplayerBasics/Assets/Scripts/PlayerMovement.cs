using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Mirror;

public class PlayerMovement : NetworkBehaviour
{
    [SerializeField] private NavMeshAgent agent = null;

    private Camera mainCamera;

    #region Server

    [Command]
    private void CmdMove(Vector3 position)
    {
        // Check if the requsted position is valid if not do nothing
        if (!NavMesh.SamplePosition(position,out NavMeshHit hit,1f,NavMesh.AllAreas)) { return; }
        

        agent.SetDestination(hit.position);
    }


    #endregion

    #region Client

    // Start method for object the client own/hold.
    public override void OnStartAuthority()
    {
        mainCamera = Camera.main;
    }


    // Client only runs this function.
    [ClientCallback]
    private void Update()
    {
        // Makes sure the update is belong to us(current client).
        if (!hasAuthority) { return; }

        // If we didnt clicked anything do nothing.
        if (!Input.GetMouseButtonDown(1)) { return; }

        // Get the position our crusor is according to the camera.
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        // Take the ray ,If we pressed outside our scene , do nothing.
        if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity)) { return; }

        CmdMove(hit.point);
    }

    #endregion

}
