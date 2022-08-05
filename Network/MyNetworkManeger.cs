using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MyNetworkManeger : NetworkManager
{
    [SerializeField] private GameObject unitSpawnerPrefab;
    [SerializeField] GameOver gameOver;

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        base.OnServerAddPlayer(conn);
        GameObject unitSpawnerInstance = Instantiate(unitSpawnerPrefab, conn.identity.transform.position, conn.identity.transform.rotation);
        NetworkServer.Spawn(unitSpawnerInstance, conn);

        //MyNetworkPlayer player = conn.identity.GetComponent<MyNetworkPlayer>();
        /*player.SetDisplayName($"Player {numPlayers}");
        Color playerColor = new Color(
            Random.Range(0f, 1f),
            Random.Range(0f, 1f),
            Random.Range(0f, 1f));

        player.SetDisplayColor(playerColor);
        Debug.Log("Manager color " + playerColor.ToString());*/
    }

    public override void OnServerSceneChanged(string sceneName)
    {
        if (SceneManager.GetActiveScene().name.StartsWith("GameScene"))
        {
            GameOver gameOverInstance = Instantiate(gameOver);

            NetworkServer.Spawn(gameOverInstance.gameObject);
        }
    }
}
