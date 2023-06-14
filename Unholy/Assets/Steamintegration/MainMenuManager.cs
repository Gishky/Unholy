using Steamworks;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    private static MainMenuManager instance;

    [SerializeField] private GameObject menuScreen, lobbyScreen;
    [SerializeField] private TMP_InputField lobbyInput;

    [SerializeField] private TextMeshProUGUI lobbyTitle, lobbyIDText;
    [SerializeField] private Button startGameButton;

    [SerializeField] private Button selectDemonButton;
    [SerializeField] private Button selectCultistButton;

    private void Awake() => instance = this;

    private void Start()
    {
        OpenMainMenu();
    }

    public void CreateLobby()
    {
        BootstrapManager.CreateLobby();
    }

    public void OpenMainMenu()
    {
        CloseAllScreens();
        menuScreen.SetActive(true);
    }

    public void OpenLobby()
    {
        CloseAllScreens();
        lobbyScreen.SetActive(true);
    }

    public static void LobbyEntered(string lobbyName, bool isHost)
    {
        instance.lobbyTitle.text = lobbyName;
        instance.startGameButton.gameObject.SetActive(isHost);
        instance.lobbyIDText.text = BootstrapManager.CurrentLobbyID.ToString();
        instance.OpenLobby();

    }

    void CloseAllScreens()
    {
        menuScreen.SetActive(false);
        lobbyScreen.SetActive(false);
    }

    public void JoinLobby()
    {
        CSteamID steamID = new CSteamID(Convert.ToUInt64(lobbyInput.text));
        BootstrapManager.JoinByID(steamID);
    }

    public void LeaveLobby()
    {
        BootstrapManager.LeaveLobby();
        OpenMainMenu();
    }

    public void StartGame()
    {
        string[] scenesToClose = new string[] { "MenuSceneSteam" };
        BootstrapNetworkManager.ChangeNetworkScene("SteamGameScene", scenesToClose);
    }

    public void SelectDemon()
    {
        selectDemonButton.interactable = false;
        selectCultistButton.interactable = true;

        GameObject[] clients = GameObject.FindGameObjectsWithTag("Client");
        for (int i = 0; i < clients.Length; i++)
        {
            GameObject client = clients[i];
            GameObject otherClient = clients[(i + 1) % clients.Length];
            if (client.GetComponent<ClientMenuManager>().IsOwner)
                client.GetComponent<ClientMenuManager>().SetPlayerTypeServer(client, otherClient, PlayerType.Demon);
        }
    }

    public void SelectCultist()
    {
        selectDemonButton.interactable = true;
        selectCultistButton.interactable = false;

        GameObject[] clients = GameObject.FindGameObjectsWithTag("Client");
        for (int i = 0; i < clients.Length; i++)
        {
            GameObject client = clients[i];
            GameObject otherClient = clients[(i + 1) % clients.Length];
            if (client.GetComponent<ClientMenuManager>().IsOwner)
                client.GetComponent<ClientMenuManager>().SetPlayerTypeServer(client, otherClient, PlayerType.Cultist);
        }
    }

    public void CopyLobbyID()
    {
        GUIUtility.systemCopyBuffer = BootstrapManager.CurrentLobbyID.ToString();
    }
}
