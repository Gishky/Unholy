using FishNet.Managing;
using FishNet.Object;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClientManager : NetworkBehaviour
{
    public PlayerType playerType;

    [SerializeField] private GameObject demonPrefab;
    [SerializeField] private GameObject cultistPrefab;

    public override void OnStartClient()
    {
        base.OnStartClient();
        if (!base.IsOwner) GetComponent<ClientManager>().enabled = false;
    }

    public void SpawnPlayer(bool isDemon)
    {
        if (isDemon)
        {
            SpawnObject(demonPrefab);
        }
        else
        {
            SpawnObject(cultistPrefab);
        }
    }

    [ServerRpc]
    public void SpawnObject(GameObject obj)
    {
        GameObject spawned = Instantiate(obj);
        base.Spawn(spawned, base.Owner);
    }
}

public enum PlayerType
{
    Demon, Cultist
}