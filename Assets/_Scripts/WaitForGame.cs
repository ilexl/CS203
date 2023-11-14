using Riptide;
using System.Collections.Generic;
using UnityEngine;

public class WaitForGame : MonoBehaviour
{
    [SerializeField] WindowManager windowManager;
    [SerializeField] Window game;
    [SerializeField] Window mainMenu;
    public void GameReady()
    {
        windowManager.ShowWindow(game);
    }

    [MessageHandler((ushort)ServerToClientId.gameStarted)]

    private void RecieveGameStartCall(Message message)
    {
        ushort otherPlayerId = message.GetUShort();
        string otherPlayerUsername = message.GetString();
        bool DoWeStart = message.GetBool();
        Debug.Log($"Started match with player [{otherPlayerUsername}:{otherPlayerId}]. ");
        string nextLine = "You are ";
        string nextLine2 = DoWeStart ? "" : "not";
        nextLine += nextLine2 + " starting!";
        Debug.Log(nextLine);
        GameReady();
    }
}

