using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

// Set colors for the different player connecting to the game.
public class TeamColorSetter : NetworkBehaviour
{
    // Store the renderes which need to be colored.
    [SerializeField] private Renderer[] colorRendereres = new Renderer[0];

    // Update the team color.
    [SyncVar(hook =nameof(HandleTeamColorUpdated))]
    private Color teamColor = new Color();


    #region Server 
    public override void OnStartServer()
    {
        RTSPlayer player = connectionToClient.identity.GetComponent<RTSPlayer>();

        teamColor = player.GetTeamColor();
    }


    #endregion


    #region Client
    // Update the color.
    private void HandleTeamColorUpdated(Color oldColor, Color newColor)
    {
        foreach(Renderer renderer in colorRendereres)
        {
            renderer.material.SetColor("_BaseColor", newColor);
        }

    }

    #endregion
}
