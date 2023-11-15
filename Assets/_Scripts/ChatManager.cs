using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatManager : MonoBehaviour
{
    [SerializeField] GameObject offlineOverlay;

    // Update is called once per frame
    void Update()
    {
        offlineOverlay.SetActive(!Game.Singleton.isMultiplayer);
    }
}
