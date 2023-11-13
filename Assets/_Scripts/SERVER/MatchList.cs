using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using wfMultiplayer;

public class MatchList : MonoBehaviour
{
    public static List<Match> list;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    /*****************************Lobby Creation*****************************/

    public Match CreateNewMatch(List<List<char>> gameBoard, List<Player> players)
    {
        Match match = new Match(gameBoard, players);
        Debug.Log($"Creating match {list.Count}, {match}");
        list.Add(match);
        return match;
    }

    private void LogNewLobby(List<Player> players)
    {
        string playerText = "";
        foreach (var player in players)
        {
            playerText += player.ToString() + ", ";
        }
        playerText = playerText.Substring(0, playerText.Length - 3);
        Debug.Log($"Creating match {list.Count} with players: " + playerText);

    }


    /*****************************Lobby Destruction**************************/
    public static void PlayerDisconnect(Player player)
    {
        DestroyLobbyByPlayer(player);
    }

    private static void DestroyLobbyByPlayer(Player player)
    {
        for (int i = 0; i < list.Count; i++)
        {
            Match item = list[i];
            if (item.GetPlayers().Contains(player))
            {
                Debug.Log($"Ending match {i}, {item}");
                list.Remove(item);
                return;
            }
        }
    }
}

