using FishNet.Managing;
using FishNet.Object;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientMenuManager : NetworkBehaviour
{
    private ClientManager client;
    private MainMenuManager mainMenuManager;

    public override void OnStartClient()
    {
        base.OnStartClient();
        if (!base.IsOwner)
            GetComponent<ClientManager>().enabled = false;


        client = GetComponent<ClientManager>();
        mainMenuManager = GameObject.Find("MainMenuManager").GetComponent<MainMenuManager>();

        if (GameObject.Find("NetworkManager").GetComponent<NetworkManager>().IsServer)
        {
            mainMenuManager.SelectDemon();
        }
        else
        {
            mainMenuManager.SelectCultist();
        }
    }

    [ServerRpc]
    public void SetPlayerTypeServer(GameObject client, GameObject otherClient, PlayerType playerType)
    {
        SetPlayerType(client, otherClient, playerType);
    }

    [ObserversRpc]
    public void SetPlayerType(GameObject client, GameObject otherClient, PlayerType playerType)
    {
        Debug.Log("Setting Player Type...");
        client.GetComponent<ClientManager>().playerType = playerType;
        if (client.GetComponent<ClientManager>().IsOwner)
            return;

        if (otherClient.GetComponent<ClientManager>().playerType == playerType)
        {
            if (playerType == PlayerType.Demon)
                mainMenuManager.SelectCultist();
            else
                mainMenuManager.SelectDemon();
        }
    }
}
