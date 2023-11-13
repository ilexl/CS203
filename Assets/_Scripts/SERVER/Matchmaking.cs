using Riptide;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using wfMultiplayer;

public class Matchmaking : MonoBehaviour
{

    public GameObject MatchListObj;
    private MatchList matchList;
    public static Dictionary<ushort, Player> PlayerList = new Dictionary<ushort, Player>();

    [MessageHandler((ushort)ClientToServerId.name)]

    private static void NewPlayer(ushort fromClientId, Message message)
    {
        string messageText = message.GetString();
        string username = string.IsNullOrEmpty(messageText) ? $"Guest {fromClientId}" : messageText;
        Player newPlayer = new Player(fromClientId, username);
        PlayerList.Add(fromClientId, newPlayer);

        Debug.Log($"{newPlayer} has connected, awaiting match.");
    }

    public static void DestroyPlayer(ushort fromClientId)
    {
        Debug.Log($"{PlayerList[fromClientId]} has disconnected.");
        Matchlist.PlayerDisconnect(Player.GetPlayerById(fromClientId));
        PlayerList.Remove(fromClientId);
    }

    private void Awake()
    {
        matchList = MatchListObj.GetComponent<MatchList>();
    }
    private void FixedUpdate()
    {
        MatchPlayers();
    }

    private void MatchPlayers()
    {
        Player unmatchedPlayer = null;
        foreach (var kvp in PlayerList)
        {
            Player player = kvp.Value;
            if (unmatchedPlayer == null)
            {
                unmatchedPlayer = player; continue;
            }

            unmatchedPlayer.InMatch = true;
            player.InMatch = true;
            List<Player> matchPlayers = new List<Player> { player, unmatchedPlayer };

            Debug.Log($"Matched players: {unmatchedPlayer} and {player}");
            

            Match match = matchList.CreateNewMatch(new List<List<char>>(), matchPlayers);

            unmatchedPlayer.currentMatch = match;
            player.currentMatch = match;
            unmatchedPlayer = null;
        }
    }
}
