using Riptide;
using System.Collections.Generic;
using UnityEngine;

public class WaitForGame : MonoBehaviour
{
    [SerializeField] static WaitForGame waitForGame;
    [SerializeField] Game GameMain;

    [Space(10)]

    [SerializeField] WindowManager windowManagerMAIN;
    [SerializeField] Window game;
    [SerializeField] Window mainMenu;

    [Space(10)]

    [SerializeField] WindowManager WaitForGameWindows;
    [SerializeField] Window searchForMatch;
    [SerializeField] Window searchingForMatch;
    [SerializeField] Window matchFound;
    [SerializeField] Window waitingForServer;
    private void Awake()
    {
        if (waitForGame == null)
        {
            waitForGame = FindAnyObjectByType<WaitForGame>();
        }
    }

    public void GameReady()
    {
        windowManagerMAIN.ShowWindow(game);
    }

    public void SearchForMatch()
    {
        WaitForGameWindows.ShowWindow(searchingForMatch);
    }

    public void Disconnect()
    {
        windowManagerMAIN.ShowWindow(mainMenu);
        // disconnect from server
    }

    public void MatchFound()
    {
        WaitForGameWindows.ShowWindow(matchFound);
    }

    [MessageHandler((ushort)ServerToClientId.recieveGameStarted)]

    private static void RecieveGameStartCall(Message message)
    {
        
        ushort otherPlayerId = message.GetUShort();
        Game.Singleton.OtherPlayerUsername = message.GetString();
        bool DoWeStart = message.GetBool();
        Debug.Log($"Started match with player [{Game.Singleton.OtherPlayerUsername}:{otherPlayerId}]. ");
        string nextLine = "You are ";
        string nextLine2 = DoWeStart ? "" : "not ";
        nextLine += nextLine2 + "starting!";
        Debug.Log(nextLine);
        Game.Singleton.NewGame();
        Game.Singleton.isMultiplayer = true;
        waitForGame.GameReady();
        waitForGame.GameMain.LocalCanPlay(DoWeStart);
    }
}

