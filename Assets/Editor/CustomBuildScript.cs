using System;
using System.Collections.Generic;
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
            // dont show stacktrace for normal logs
            Application.SetStackTraceLogType(LogType.Log, StackTraceLogType.None);

            try
            {
                var profile = GetCommandArg("-profile") ?? "default";
                var platformArg = GetCommandArg("-platforms") ?? EditorUserBuildSettings.activeBuildTarget.ToString();
                var platforms = platformArg.Split(",").ToList();

                DoPreProcessing(profile, () => { DoBuilding(profile, platforms); });
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

        private static async void DoBuilding(string profile, List<string> platforms)
        {
            try
            {
                foreach (var platform in platforms)
                {
                    if (!SwitchPlatform(platform)) continue;

                    StopWatch.Restart();
                    await Task.Delay(5555);
                    StopWatch.Stop();
                    Debug.Log($"Finished DoBuilding for {platform} [Duration: {StopWatch.Elapsed:g}]");
                }

                if (Application.isBatchMode) EditorApplication.Exit(0);
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
                if (Application.isBatchMode) EditorApplication.Exit(1);
            }
        }
        private static bool SwitchPlatform(string platform)
        {
            try
            {
                var target = Enum.Parse<BuildTarget>(platform);
                var group = BuildPipeline.GetBuildTargetGroup(target);
                
                EditorUserBuildSettings.SwitchActiveBuildTargetAsync(group, target);
                return true;
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"{platform} is not supported. /n {ex.Message}");
                return false;
            }
        }

        private static string GetCommandArg(string name)
        {
            return Environment.GetCommandLineArgs().FirstOrDefault(arg => arg == name);
        }
    }
}