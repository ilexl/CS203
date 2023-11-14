using Riptide;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using wfMultiplayer;

public class Matchmaking : MonoBehaviour
{

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
        Player player = PlayerList[fromClientId];
        Debug.Log("Got player successfully");
        Match match = player.currentMatch;
        MatchList.PlayerDisconnect(match);
        PlayerList.Remove(fromClientId);
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
            if (player.InMatch) continue;
            if (unmatchedPlayer == null)
            {
                unmatchedPlayer = player; continue;
            }

            unmatchedPlayer.InMatch = true;
            player.InMatch = true;
            List<Player> matchPlayers = new List<Player> { unmatchedPlayer, player };

            Debug.Log($"Matched players: {unmatchedPlayer} and {player}");


            Match match = MatchList.CreateNewMatch(new List<List<char>>(), matchPlayers);
            Debug.Log("Created new match successfully");
            unmatchedPlayer.currentMatch = match;
            player.currentMatch = match;
            SendStartGameToAllPlayers(matchPlayers);
            Debug.Log("Set new player matches successfully");
            unmatchedPlayer = null;
        }
    }


    private void SendStartGameToAllPlayers(List<Player> matchPlayers)
    {
        bool startingPlayerAlreadyAssigned = false;
        foreach (var player in matchPlayers)
        {
            foreach (var otherPlayer in matchPlayers)
            {
                if (otherPlayer == player) continue;
                bool startingTurn = false ? startingPlayerAlreadyAssigned : true;
                SendStartGame(player, otherPlayer, startingTurn);
            }
            startingPlayerAlreadyAssigned = true;
        }
    }
    private void SendStartGame(Player player, Player otherPlayer, bool startingTurn)
    {
        Debug.Log($"Telling player {player} to start match with {otherPlayer}.");

        Message message = Message.Create(MessageSendMode.Reliable, (ushort)ServerToClientId.gameStarted);
        message.AddUShort(otherPlayer.Id);
        message.AddString(otherPlayer.Username);
        message.AddBool(startingTurn);
        NetworkManager.Singleton.Server.Send(message, player.Id);
    }
}
