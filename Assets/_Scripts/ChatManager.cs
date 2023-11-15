using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatManager : MonoBehaviour
{
    [SerializeField] GameObject offlineOverlay;
    private static ChatManager _singleton;
    public static ChatManager Singleton
    {
        get => _singleton;
        private set
        {
            if (_singleton == null)
                _singleton = value;
            else if (_singleton != value)
            {
                Debug.Log($"{nameof(ChatManager)} instance already exists, destroying duplicate!");
                Destroy(value);
            }
        }
    }

    public enum SentBy
    {
        Player, 
        Opponent,
    }

    private void Awake()
    {
        Singleton = this;
    }

    // Update is called once per frame
    void Update()
    {
        offlineOverlay.SetActive(!Game.Singleton.isMultiplayer);
    }

    public void ClearAllMessages()
    {

    }

    public void Message(string message, SentBy sentBy)
    {

    }
}
