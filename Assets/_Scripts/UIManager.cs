using Riptide;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private static UIManager _singleton;
    public static UIManager Singleton
    {
        get => _singleton;
        private set
        {
            if (_singleton == null)
                _singleton = value;
            else if (_singleton != value)
            {
                Debug.Log($"{nameof(UIManager)} instance already exists, destroying duplicate!");
                Destroy(value);
            }
        }
    }

    [Header("Connect")]
    [SerializeField] private TMP_InputField usernameField;
    [SerializeField] private PopUpManager connectPopUpManager;
    [SerializeField] private WindowManager windowManager;
    [SerializeField] private Window WaitingForGame;
    [SerializeField] private Window MultiplayerConnect;
    [SerializeField] private Window Game;

    private void Awake()
    {
        Singleton = this;
    }

    public void ConnectClicked()
    {
        usernameField.interactable = false;
        NetworkManager.Singleton.Connect();
    }

    public void SearchClicked()
    {
        SendSearchStatus();
    }

    public void Disconnect()
    {
        NetworkManager.Singleton.Disconnect();
    }

    private void SendSearchStatus()
    {
        Message message = Message.Create(MessageSendMode.Reliable, (ushort)ClientToServerId.sendSearchForMatch);
        NetworkManager.Singleton.Client.Send(message);
    }
    public void ConnectionFailed()
    {
        usernameField.interactable = true;
        connectPopUpManager.ShowPopUp(0);
    }

    public void ConnectionSucceeded()
    {
        windowManager.ShowWindow(WaitingForGame);
    }

    public void SendName()
    {
        Message message = Message.Create(MessageSendMode.Reliable, (ushort)ClientToServerId.sendName);
        message.AddString(usernameField.text);
        NetworkManager.Singleton.Client.Send(message);
    }

    public void PlayerDisconnect()
    {
        windowManager.ShowWindow(MultiplayerConnect);
        connectPopUpManager.ShowPopUp(1);
    }

    // THIS IS IN THE UI MANAGER CLASS (:
    public void SendChatMessage(string chatMessage)
    {
        // Will To send message here to the other player from the server
        Message message = Message.Create(MessageSendMode.Reliable, (ushort)ClientToServerId.sendChat);
        message.AddString(chatMessage);
        NetworkManager.Singleton.Client.Send(message);
        // Alex creates chatMessage under here for GUI to display
        ChatManager.Singleton.Message(chatMessage, ChatManager.SentBy.Player);
    }

    [MessageHandler((ushort)ServerToClientId.recieveChat)]

    public static void RecieveChatMessage(Message message)
    {
        Singleton.UpdateChat(message.GetString());
    }

    public void UpdateChat(string message)
    {
        // Will to call this function when recieving a message from the server/other player

        // Alex creates message under here for GUI to display
        ChatManager.Singleton.Message(message, ChatManager.SentBy.Opponent);
    }
} 
