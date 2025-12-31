using UnityEngine;

namespace OliversComputer.ACPlayer
{
    public static class BetterDebug
    {
        /// <summary>
        /// 是否允许输出普通 Log
        /// </summary>
        public static bool EnableLog = true;

        /// <summary>
        /// 是否允许输出 Warning
        /// </summary>
        public static bool EnableWarning = true;

        /// <summary>
        /// 是否允许输出 Error
        /// </summary>
        public static bool EnableError = true;

        [System.Diagnostics.Conditional("UNITY_EDITOR")]
        [System.Diagnostics.Conditional("DEVELOPMENT_BUILD")]
        public static void Log(string message)
        {
            if (!EnableLog) return;
            Debug.Log(message);
        }

        [System.Diagnostics.Conditional("UNITY_EDITOR")]
        [System.Diagnostics.Conditional("DEVELOPMENT_BUILD")]
        public static void LogWarning(string message)
        {
            if (!EnableWarning) return;
            Debug.LogWarning(message);
        }

        [System.Diagnostics.Conditional("UNITY_EDITOR")]
        [System.Diagnostics.Conditional("DEVELOPMENT_BUILD")]
        public static void LogError(string message)
        {
            if (!EnableError) return;
            Debug.LogError(message);
        }
    }
}