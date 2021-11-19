using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;
using System;


// Manage the network system 
public class RTSNetworkManager : NetworkManager
{

    // The intial base in the start of the game.
    [SerializeField] private GameObject unitBasePrefab;

    // The gameover handler.
    [SerializeField] private GameOverHandler gameOverHandlerPrefab = null;

    // Client connections events
    public static event Action ClientOnConnected;
    public static event Action ClientOnDisConnected;

    // The progession of the game.
    private bool isGameInProgess = false;

    // List of players currently connected.
    public List<RTSPlayer> players { get; } = new List<RTSPlayer>();

    #region Server

    // Close connection if tried to connect a game that already started.
    public override void OnServerConnect(NetworkConnection conn)
    {
        if(isGameInProgess)
            conn.Disconnect();
    }

    // Remove disconnected player.
    public override void OnServerDisconnect(NetworkConnection conn)
    {
        players.Remove(conn.identity.GetComponent<RTSPlayer>());
    }

    // Clear the players.
    public override void OnStopServer()
    {
        players.Clear();

        isGameInProgess = false;
    }

    // Start the game if there are 2 player at least.
    public void StartGame()
    {
        if(players.Count < 2) { return; }

        isGameInProgess = true;

        ServerChangeScene("Map_Scene_01");
    }

    //
    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        base.OnServerAddPlayer(conn);

        SetPlayerAttributes(conn);
    }


    // Set the player attributes.
    private void SetPlayerAttributes(NetworkConnection conn)
    {
        RTSPlayer player = conn.identity.GetComponent<RTSPlayer>();

        players.Add(player);

        player.SetDisplayName($"Player {players.Count}");

        player.SetTeamColor(UnityEngine.Random.ColorHSV());

        player.SetPartyOwner(players.Count == 1);
    }


    // Creating a new 'game over handler' when entering the map scene.
    public override void OnServerSceneChanged(string sceneName)
    {
        if (SceneManager.GetActiveScene().name.StartsWith("Map_Scene"))
        {
            GameOverHandler gameOverHandlerInstance = Instantiate(gameOverHandlerPrefab);

            // Instantiate the 'game over handler' in the server to be known to all
            // Clients.
            NetworkServer.Spawn(gameOverHandlerInstance.gameObject);

            InstantiatePlayersBases();
        }
    }

    // Instatiate the players bases in the map
    // and register them in the server
    private void InstantiatePlayersBases()
    {
        foreach (RTSPlayer player in players)
        {
            GameObject baseInstance = Instantiate(unitBasePrefab,
                GetStartPosition().position, Quaternion.identity);

            // Spawn the player on server.
            NetworkServer.Spawn(baseInstance, player.connectionToClient);
        }
    }

    #endregion


    #region Client
    public override void OnClientConnect(NetworkConnection conn)
    {
        base.OnClientConnect(conn);

        ClientOnConnected?.Invoke();
    }

    public override void OnClientDisconnect(NetworkConnection conn)
    {
        base.OnClientDisconnect(conn);

        ClientOnDisConnected?.Invoke();
    }

    #endregion





}
