using UnityEngine;

namespace DebugStuff
{
    public class DebugText : MonoBehaviour
    {
        private int maxLength = 3000;
        static string myLog = "";
        private string output;
        private string stack;

        GUIStyle errorStyle;
        GUIStyle warningStyle;
        GUIStyle normalStyle;

        void OnEnable()
        {
            Application.logMessageReceived += Log;

            // Set up your styles here
            errorStyle = new GUIStyle();
            errorStyle.normal.textColor = Color.red;

            warningStyle = new GUIStyle();
            warningStyle.normal.textColor = Color.yellow;

            normalStyle = new GUIStyle();
            normalStyle.normal.textColor = Color.white;
        }

        void OnDisable()
        {
            Application.logMessageReceived -= Log;
        }

        public void Log(string logString, string stackTrace, LogType type)
        {
            output = logString;
            stack = stackTrace;
            myLog = myLog + "\n";

            switch (type)
            {
                case LogType.Error:
                    myLog += "<color=#FF0000>" + output + "</color>";
                    break;
                case LogType.Warning:
                    myLog += "<color=#FFFF00>" + output + "</color>";
                    break;
                default:
                    myLog += output;
                    break;
            }

            if (myLog.Length > maxLength)
            {
                myLog = "";
            }
        }

        void OnGUI()
        {
            myLog = GUI.TextArea(new Rect(10, 10, Screen.width - 10, Screen.height - 10), myLog, normalStyle);
        }
    }
}