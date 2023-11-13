using Riptide;
using Riptide.Utils;
using UnityEngine;
/********************************************************************************THIS IS THE SERVERSIDE VERSION DON'T BE A FOOL ************************************************************************/
public enum ClientToServerId : ushort
{
    name = 1,
}
public class NetworkManager : MonoBehaviour
{
    private static NetworkManager _singleton;
    public static NetworkManager Singleton
    {
        get => _singleton;
        private set
        {
            if (_singleton == null)
                _singleton = value;
            else if (_singleton != value)
            {
                Debug.Log($"{nameof(NetworkManager)} instance already exists, destroying duplicate!");
                Destroy(value);
            }
        }
    }

    /********************************************************************************THIS IS THE SERVERSIDE VERSION DON'T BE A FOOL ************************************************************************/
    public Server Server { get; private set; }

    [SerializeField] private ushort port;
    [SerializeField] private ushort maxClientCount;
    private void Awake()
    {
        Singleton = this;
    }

    private void Start()
    {
        RiptideLogger.Initialize(Debug.Log, Debug.Log, Debug.LogWarning, Debug.LogError, false);

        Server = new Server();
        Server.Start(port, maxClientCount);
        Server.ClientDisconnected += PlayerLeft;
    }
    /********************************************************************************THIS IS THE SERVERSIDE VERSION DON'T BE A FOOL ************************************************************************/
    private void FixedUpdate()
    {
        Server.Update();
    }

    private void OnApplicationQuit()
    {
        Server.Stop();
    }

    private void PlayerLeft(object sender, ServerDisconnectedEventArgs e)
    {
        PlayerList.DestroyPlayer(e.Client.Id);
    }
    /********************************************************************************THIS IS THE SERVERSIDE VERSION DON'T BE A FOOL ************************************************************************/
}
