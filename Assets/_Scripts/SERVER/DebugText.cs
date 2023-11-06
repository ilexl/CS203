
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class DebugText : MonoBehaviour
{
    // Adjust via the Inspector
    public int maxLines = 8;
    private Queue<string> queue = new Queue<string>();
    private string currentText = "";

    void OnEnable()
    {
        Application.logMessageReceivedThreaded += HandleLog;
    }

    void OnDisable()
    {
        Application.logMessageReceivedThreaded -= HandleLog;
    }

    void HandleLog(string logString, string stackTrace, LogType type)
    {
        // Delete oldest message
        if (queue.Count >= maxLines) queue.Dequeue();

        queue.Enqueue(logString);

        var builder = new StringBuilder();
        foreach (string st in queue)
        {
            builder.Append(st).Append("\n");
        }

        currentText = builder.ToString();
    }

    void OnGUI()
    {
        GUI.Label(
           new Rect(
               0,                   // x, left offset
               0,                   // y, bottom offset
               Screen.width,        // width
               Screen.height        // height
           ),
           currentText,             // the display text
           GUI.skin.textArea        // use a multi-line text area
        );
    }
}