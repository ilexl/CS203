using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceAspectRatio : MonoBehaviour
{
    private void Start()
    {
        
#if UNITY_EDITOR
        Debug.Log("UNITY EDITOR ENABLED - PLEASE ENSURE RESOLUTION IS 16:9");
#else
        // Calculate the target height based on the screen width and 16:9 aspect ratio
        int targetHeight = Screen.width * 9 / 16;

        // Set the game's resolution to match the target width and height
        Screen.SetResolution(Screen.width, targetHeight, false);
        
#endif
    }
}
