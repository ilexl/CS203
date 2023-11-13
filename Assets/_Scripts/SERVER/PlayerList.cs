using Riptide;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using wfMultiplayer;

public class PlayerList : MonoBehaviour
{
    public static Dictionary<ushort, Player> list = new Dictionary<ushort, Player>();

    [MessageHandler((ushort)ClientToServerId.name)]

    private static void NewPlayer(ushort fromClientId, Message message)
    {
        string messageText = message.GetString();
        string username = string.IsNullOrEmpty(messageText) ? $"Guest {fromClientId}" : messageText;
        Player newPlayer = new Player(fromClientId, username);
        list.Add(fromClientId, newPlayer);

        Debug.Log($"User {newPlayer} has connected, awaiting match.");
    }

    public static void DestroyPlayer(ushort fromClientId)
    {
        list.Remove(fromClientId);
    }
}
