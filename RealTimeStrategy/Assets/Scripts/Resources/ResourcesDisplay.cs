using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResourcesDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text resourcesDisplayText = null;

    private RTSPlayer player;


    private void Update()
    {
        if (player == null)
        {
            player = NetworkClient.connection.identity.GetComponent<RTSPlayer>();

            if(player != null)
            {
                HandleOnClientResourcesUpdate(player.GetResources());

                player.ClientOnResourcesUpdated += HandleOnClientResourcesUpdate;
            }
        }
    }

    private void OnDestroy()
    {
        player.ClientOnResourcesUpdated -= HandleOnClientResourcesUpdate;    
    }


    private void HandleOnClientResourcesUpdate(int resources)
    {
        resourcesDisplayText.text = $"Resources:{resources}";

    }
}
