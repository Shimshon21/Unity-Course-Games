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

    // Warns the client to access to that function.
    [Server]
    public void SetDisplayName(string newDisplayName)
    {
        displayName = newDisplayName;
    }
   

    public void SetRandomColor(Color colorVal)
    {
        color = colorVal;
    }

    private void HandleDisplayColorUpdated(Color oldColor,Color newColor)
    {
        displayColorRenderer.material.SetColor("_Color", newColor);

    }


    private void HandleDisplayNameUpdated(string oldName,string newName)
    {
        displayNameText.text = newName;
    }
}


