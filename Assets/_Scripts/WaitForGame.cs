using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitForGame : MonoBehaviour
{
    [SerializeField] WindowManager windowManagerMAIN;
    [SerializeField] Window game;
    [SerializeField] Window mainMenu;

    [Space(10)]

    [SerializeField] WindowManager WaitForGameWindows;
    [SerializeField] Window searchForMatch;
    [SerializeField] Window searchingForMatch;
    [SerializeField] Window matchFound;
    [SerializeField] Window waitingForServer;
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
}

