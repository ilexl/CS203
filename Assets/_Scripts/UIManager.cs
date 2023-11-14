using Riptide;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

    private void SendSearchStatus()
    {
        Message message = Message.Create(MessageSendMode.Reliable, (ushort)ClientToServerId.searchForMatch);
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
        Message message = Message.Create(MessageSendMode.Reliable, (ushort)ClientToServerId.name);
        message.AddString(usernameField.text);
        NetworkManager.Singleton.Client.Send(message);
    }
} 
