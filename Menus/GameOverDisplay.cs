using System.Collections;
using System.Collections.Generic;
using Mirror;
using TMPro;
using UnityEngine;

public class GameOverDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text winnerNameText;
    [SerializeField] private GameObject GameOverParent;
    private void Start()
    {
        GameOver.ClientOnGameOver += ClientHandleGameOver;
    }
    private void OnDestroy()
    {
        GameOver.ClientOnGameOver -= ClientHandleGameOver;
    }

    public void LeaveGame()
    {
        if(NetworkServer.active && NetworkClient.isConnected) 
        {
            NetworkManager.singleton.StopHost();
        }
        else
        {
            NetworkManager.singleton.StopClient();
        }
    }

    void ClientHandleGameOver(string winner)
    {
        winnerNameText.text = $"{winner} Has Won";

        GameOverParent.SetActive(true);
    }
}
