using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using wfMultiplayer;
using static UnityEditor.Experimental.GraphView.GraphView;

public class MatchList : MonoBehaviour
{
    public List<Match> matchList;

}
    /*****************************Lobby Creation*****************************/

    public void CreateNewMatch(List<List<char>> gameBoard, List<wfMultiplayer.Player> players)
    {
        Match match = new Match(gameBoard, players);
        Debug.Log($"Creating match {matchList.Count}, {match}");
        matchList.Add(match);
    }

    private void LogNewLobby(List<wfMultiplayer.Player> players)
    {
        string playerText = "";
        foreach (var player in players)
        {
            playerText += player.ToString() + ", ";
        }
        playerText = playerText.Substring(0, playerText.Length - 3);
        Debug.Log($"Creating match {matchList.Count} with players: " + playerText);

    }

