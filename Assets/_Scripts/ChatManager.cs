using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
#if UNITY_EDITOR
using UnityEditor;
#endif


public class ChatManager : MonoBehaviour
{
    [SerializeField] GameObject offlineOverlay;
    private static ChatManager _singleton;
    [SerializeField] GameObject playerChatPrefab;
    [SerializeField] GameObject opponentChatPrefab;
    [SerializeField] Transform parent;
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
        foreach(Transform child in parent)
        {
            Destroy(child.gameObject);
        }
    }

    public void SendMessageTMPro(TMP_InputField inputField)
    {
        UIManager.Singleton.SendChatMessage(inputField.text);
        inputField.text = string.Empty;
    }

    public void Message(string message, SentBy sentBy)
    {
        if(SentBy.Player == sentBy)
        {
            GameObject newMessage = Instantiate(playerChatPrefab, parent);
            newMessage.GetComponent<Chat>().SetText(message);
        }
        else if (SentBy.Opponent == sentBy)
        {
            GameObject newMessage = Instantiate(opponentChatPrefab, parent);
            newMessage.GetComponent<Chat>().SetText(message);
        }
        else 
        {
            Debug.LogWarning("Message not created as not sent by opp or you...");
        }

    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(ChatManager))]
public class EDITOR_ChatManager : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        ChatManager cm = (ChatManager)target;
        if(GUILayout.Button("Player Large Message"))
        {
            cm.Message("Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It", ChatManager.SentBy.Player);

        }
        if (GUILayout.Button("Player Small Message"))
        {
            cm.Message("Test Message", ChatManager.SentBy.Player);

        }
        if (GUILayout.Button("Opponent Large Message"))
        {
            cm.Message("Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It", ChatManager.SentBy.Opponent);

        }
        if (GUILayout.Button("Opponent Small Message"))
        {
            cm.Message("Test Message", ChatManager.SentBy.Opponent);

        }
    }
}
#endif