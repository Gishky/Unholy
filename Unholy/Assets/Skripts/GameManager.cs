using FishNet.Object;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    private void Awake() => instance = this;
    public static GameManager Instance() => instance;

    public ClientManager clientManager;
    public PlayerType clientPlayerType;

    void Start()
    {
        GameObject[] clients = GameObject.FindGameObjectsWithTag("Client");
        foreach (GameObject client in clients)
        {
            if (client.GetComponent<ClientManager>().IsOwner)
            {
                clientManager = client.GetComponent<ClientManager>();
            }
        }
        clientPlayerType = clientManager.playerType;

        clientManager.SpawnPlayer(clientPlayerType == PlayerType.Demon);
    }
}
