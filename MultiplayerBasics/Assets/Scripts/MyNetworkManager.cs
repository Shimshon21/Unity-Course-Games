using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class MyNetworkManager : NetworkManager
{
    int numPlayers = 0;


    public override void OnClientConnect(NetworkConnection conn)
    {
        base.OnClientConnect(conn);

        Debug.Log("Connected to a server");

    }

    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        base.OnServerAddPlayer(conn);

        MyNetworkPlayer player = conn.identity.GetComponent<MyNetworkPlayer>();

        numPlayers++;

        player.SetDisplayName($"Player{numPlayers}");

        player.SetDisplayColor(Random.ColorHSV());

        Debug.Log("Player have been added, overall num of player: " + numPlayers);
    }

}
