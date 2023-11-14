using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitForGame : MonoBehaviour
{
    [SerializeField] WindowManager windowManager;
    [SerializeField] Window game;
    [SerializeField] Window mainMenu;
    public void GameReady()
    {
        windowManager.ShowWindow(game);
    }
}

