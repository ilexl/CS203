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
            myLog = GUI.TextArea(new Rect(10, 10, Screen.width - 10, Screen.height - 10), myLog, normalStyle);
        }
    }
}