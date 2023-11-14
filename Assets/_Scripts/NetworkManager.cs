using Riptide;
using Riptide.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public enum ClientToServerId : ushort
{
    name = 1,
    searchForMatch = 2,
    sendTurn = 3,
}

public enum ServerToClientId : ushort
{
    gameStarted = 1,
    recieveBoardState = 2,
    opponentDisconnected = 5,
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

    public void Disconnect()
    {
        Client.Disconnect();
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

    public void ClientPlays(List<List<char>> board, int score)
    {
        Message message = Message.Create(MessageSendMode.Reliable, (ushort)ClientToServerId.sendTurn);
        message.AddString(CompileBoard(board));
        message.AddInt(score);
        Singleton.Client.Send(message);
    }


    // WILL NEEDS TO USE THIS WHEN RECIEVING DATA FROM SERVER
    [MessageHandler((ushort)ServerToClientId.recieveBoardState)]
    public void OppenentPlays(Message message)
    {
        string boardRAW = message.GetString();
        int score = message.GetInt();

        List<List<char>> boardP = new List<List<char>>();

        for (int i = 0; i < Game.instance.board.BoardSize * 2; i++)
        {
            for (int j = 0; j < Game.instance.board.BoardSize * 2; j++)
            {
                boardP[i][j] = ' ';
            }
        }

        List<string> temp = boardRAW.Split('\n').ToList();
        for (int i = 0; i < temp.Count; i++)
        {
            for (int j = 0; j < temp[i].Count(); j++)
            {
                boardP[i][j] = temp[i][j];
            }
        }

        Game.instance.OppPlay(boardP, score);
    }
    [MessageHandler((ushort)ServerToClientId.opponentDisconnected)]
    public static void OpponentDisconnect(Message message)
    {
        //alex stuff
        Debug.Log("yes.");
    }

    private string CompileBoard(List<List<char>> board)
    {
        string output = "";
        foreach (var row in board)
        {
            foreach (char  c in row)
            {
                output += c;
            }
            output += "\n";
        }
        return output;
    }
}


