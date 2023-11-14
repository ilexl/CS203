using UnityEngine;

namespace DebugStuff
{
    public class DebugText : MonoBehaviour
    {

        private int maxLength = 3000;
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
            myLog = myLog + "\n" + output;
            if (myLog.Length > maxLength)
            {
                myLog = "";
            }
        }

        void OnGUI()
        {
            //if (!Application.isEditor) //Do not display in editor ( or you can use the UNITY_EDITOR macro to also disable the rest)
            {
                myLog = GUI.TextArea(new Rect(10, 10, Screen.width - 10, Screen.height - 10), myLog);
            }
        }
        //#endif
    }
}