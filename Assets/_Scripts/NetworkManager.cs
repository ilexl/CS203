using Riptide;
using Riptide.Utils;
using System;
using UnityEngine;


public enum ClientToServerId : ushort
{
    name = 1,
    searchForMatch = 2,
}

public enum ServerToClientId : ushort
{
    gameStarted = 1,
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


    public Client Client { get; private set; }

    [SerializeField] private string ip;
    [SerializeField] private ushort port;
    private void Awake()
    {
        Singleton = this;
    }

    private void Start()
    {
        RiptideLogger.Initialize(Debug.Log, Debug.Log, Debug.LogWarning, Debug.LogError, false);
        Client = new Client();
        Client.Connected += DidConnect;
        Client.ConnectionFailed += FailedToConnect;
        Client.Disconnected += DidDisconnect;
    }

    private void FixedUpdate()
    {
        Client.Update();
    }

    private void OnApplicationQuit()
    {
        Client.Disconnect();
    }

    public void Connect()
    {
        Client.Connect($"{ip}:{port}");
    }

    private void DidConnect(object sender, EventArgs e)
    {
        UIManager.Singleton.SendName();
        UIManager.Singleton.ConnectionSucceeded();
    }

    private void FailedToConnect(object sender, EventArgs e)
    {
        UIManager.Singleton.ConnectionFailed();
    }

    private void DidDisconnect(object sender, EventArgs e)
    {
        // Popup and to main menu
        UIManager.Singleton.PlayerDisconnect();
    }
}
