using UnityEngine;

namespace DebugStuff
{
    public class DebugText : MonoBehaviour
    {

        private int maxLength = 30000;
        //#if !UNITY_EDITOR
        static string myLog = "";
        private string output;
        private string stack;

        void OnEnable()
        {
            Application.logMessageReceived += Log;
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
                // Calculate how many characters to trim from the start
                int charactersToTrim = myLog.Length - maxLength;

                // Trim characters from the start
                myLog = myLog.Substring(charactersToTrim, myLog.Length);
            }
        }

        void OnGUI()
        {
            // if (!Application.isEditor) //Do not display in editor ( or you can use the UNITY_EDITOR macro to also disable the rest)
            {
                // Set up the scroll view
                Rect scrollViewRect = new Rect(10, 10, Screen.width - 30, Screen.height - 30);
                Rect scrollContentRect = new Rect(0, 0, Screen.width - 50, Mathf.Max(0, myLog.Length * 20));

                // Create the scroll view
                Vector2 scrollPosition = GUI.BeginScrollView(scrollViewRect, Vector2.zero, scrollContentRect);

                // Display the log within the scroll view
                myLog = GUI.TextArea(new Rect(0, 0, Screen.width - 50, Mathf.Max(0, myLog.Length * 20)), myLog);

                // End the scroll view
                GUI.EndScrollView();
            }
        }
    }
}