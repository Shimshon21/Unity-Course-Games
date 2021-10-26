using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using TMPro;

public class MyNetworkPlayer : NetworkBehaviour
{
    // SyncVariable which is updated on server for each changed.
    [SyncVar(hook =nameof(HandleDisplayNameUpdated))] [SerializeField]private string displayName = "Missing Name";

    [SyncVar(hook =nameof(HandleDisplayColorUpdated))][SerializeField] private Color color = Color.black;


    [SerializeField] private TMP_Text displayNameText = null;
    [SerializeField] private Renderer displayColorRenderer;

    # region Server
    // Warns the client to access to that function.
    [Server]
    public void SetDisplayName(string newDisplayName)
    {
        displayName = newDisplayName;
    }
   

    public void SetDisplayColor(Color newDisplaycolor)
    {
        color = newDisplaycolor;
    }


    // Command the server to runt this function.
    [Command]
    private void CmdSetDisplayName(string newDisplayName)
    {

        if (newDisplayName.Length > 5)
        {

            RpcLogNewName(newDisplayName);

            SetDisplayName(newDisplayName);
        }
        else
        {
            Debug.LogError("Error Name Is Too Short!");
        }
    }

    #endregion

    #region Client
    private void HandleDisplayColorUpdated(Color oldColor,Color newColor)
    {
        displayColorRenderer.material.SetColor("_Color", newColor);

    }


    private void HandleDisplayNameUpdated(string oldName,string newName)
    {
        displayNameText.text = newName;
    }

    [ContextMenu("Set My Name")]
    private void SetMyName()
    {
        CmdSetDisplayName("My");
    }



    //Say all client to run this code
    [ClientRpc]
    private void RpcLogNewName(string newName)
    {
        Debug.Log(newName);
    }

    #endregion

}


