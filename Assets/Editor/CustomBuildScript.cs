using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Editor
{
    public class CustomBuildScript : UnityEditor.Editor
    {
        private static Stopwatch StopWatch = new Stopwatch();

        /// <summary>
        /// Main entry point will be called from github actions.
        /// </summary>
        public static void Execute()
        {
            try
            {
                var profile = GetCommandArg("-profile") ?? "default";
                var platform = GetCommandArg("-platform") ?? EditorUserBuildSettings.activeBuildTarget.ToString();
                
                DoPreProcessing(profile, () =>
                {
                    DoBuilding(profile);
                });
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
                
                if (Application.isBatchMode)
                    EditorApplication.Exit(1);
            }
        }

        private static async void DoPreProcessing(string profile, Action onPreProcessComplete = null)
        {
            StopWatch.Restart();
            // simulation some processing
            await Task.Delay(7777);
            StopWatch.Stop();
            Debug.Log($"Finished DoPreProcessing [Duration: {StopWatch.Elapsed:g}]");
            
            onPreProcessComplete?.Invoke();
        }

        private static async void DoBuilding(string profile)
        {
            StopWatch.Restart();
            await Task.Delay(5555);
            StopWatch.Stop();
            Debug.Log($"Finished DoBuilding [Duration: {StopWatch.Elapsed:g}]");
        }

        private static string GetCommandArg(string name)
        {
            return Environment.GetCommandLineArgs().FirstOrDefault(arg => arg == name);
        }
    }
}