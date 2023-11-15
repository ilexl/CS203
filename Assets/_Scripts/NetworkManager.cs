using Riptide;
using Riptide.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif


public enum ClientToServerId : ushort
{
    sendName = 1,
    sendSearchForMatch = 2,
    sendTurnToServer = 3,
    sendChat = 4,
}

public enum ServerToClientId : ushort
{
    recieveGameStarted = 1,
    recieveBoardState = 2,
    recieveChat = 4,
    recieveOpponentDisconnect = 5,
}
public class NetworkManager : MonoBehaviour
{
    public void TempLocalSever()
    {
        ip = "192.168.1.77";
    }

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
        Message message = Message.Create(MessageSendMode.Reliable, (ushort)ClientToServerId.sendTurnToServer);
        message.AddString(CompileBoard(board));
        message.AddInt(score);
        Singleton.Client.Send(message);
    }


    // WILL NEEDS TO USE THIS WHEN RECIEVING DATA FROM SERVER
    [MessageHandler((ushort)ServerToClientId.recieveBoardState)]
    public static void OppenentPlays(Message message)
    {
        string boardRAW = message.GetString();
        int score = message.GetInt();

        List<List<char>> boardP = new List<List<char>>();

        for (int i = 0; i < Game.Singleton.board.BoardSize * 2; i++)
        {
            List<char> row = new List<char>();
            for (int j = 0; j < Game.Singleton.board.BoardSize * 2; j++)
            {
                row.Add(' ');
            }
            boardP.Add(row);
        }

        List<string> temp = boardRAW.Split('\n').ToList();
        for (int i = 0; i < temp.Count; i++)
        {
            for (int j = 0; j < temp[i].Count(); j++)
            {
                boardP[i][j] = temp[i][j];
            }
        }

        Game.Singleton.OppPlay(boardP, score);
    }
    [MessageHandler((ushort)ServerToClientId.recieveOpponentDisconnect)]
    public static void OpponentDisconnect(Message message)
    {
        //alex stuff
        Game.Singleton.LocalCanPlay(false);
        Game.Singleton.popUpManager.ShowPopUp(8);
        Game.Singleton.isMultiplayer = false;
    }

    private static string CompileBoard(List<List<char>> board)
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


#if UNITY_EDITOR
[CustomEditor(typeof(NetworkManager))]
public class EDITOR_NetworkManager : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        NetworkManager nm = (NetworkManager)target;
        if(GUILayout.Button("temp local"))
        {
            nm.TempLocalSever();
        }
    }
}
#endif