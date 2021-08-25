using System;
using Altom.Editor;
using Altom.Editor.Logging;
using Altom.AltUnity.Instrumentation;
using NLog;
using UnityEditor;

public class BuildAltUnityTester
{
    private static readonly Logger logger = EditorLogManager.Instance.GetCurrentClassLogger();

    [MenuItem("Build/Android")]
    protected static void AndroidBuildFromCommandLine()
    {
        try
        {
            string versionNumber = DateTime.Now.ToString("yyMMddHHss");

            PlayerSettings.companyName = "Altom";
            PlayerSettings.productName = "sampleGame";
            PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.Android, "fi.altom.altunitytester");
            PlayerSettings.bundleVersion = versionNumber;
            PlayerSettings.Android.bundleVersionCode = int.Parse(versionNumber);
            PlayerSettings.Android.minSdkVersion = AndroidSdkVersions.AndroidApiLevel23;
            PlayerSettings.SetApiCompatibilityLevel(BuildTargetGroup.Android, ApiCompatibilityLevel.NET_4_6);
#if UNITY_2018_1_OR_NEWER
            PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARMv7;
#endif

            logger.Debug("Starting Android build..." + PlayerSettings.productName + " : " + PlayerSettings.bundleVersion);
            var buildPlayerOptions = new BuildPlayerOptions
            {
                scenes = new string[]
                {
                    "Assets/AltUnityTester/Examples/Scenes/Scene 1 AltUnityDriverTestScene.unity",
                    "Assets/AltUnityTester/Examples/Scenes/Scene 2 Draggable Panel.unity",
                    "Assets/AltUnityTester/Examples/Scenes/Scene 3 Drag And Drop.unity",
                    "Assets/AltUnityTester/Examples/Scenes/Scene 4 No Cameras.unity",
                    "Assets/AltUnityTester/Examples/Scenes/Scene 5 Keyboard Input.unity",
                    "Assets/AltUnityTester/Examples/Scenes/Scene6.unity"
                },

                locationPathName = "sampleGame.apk",
                target = BuildTarget.Android,
                options = BuildOptions.Development | BuildOptions.AutoRunPlayer
            };

            AltUnityBuilder.AddAltUnityTesterInScritpingDefineSymbolsGroup(BuildTargetGroup.Android);


            var instrumentationSettings = AltUnityTesterEditor.EditorConfiguration == null ? new AltUnityInstrumentationSettings() : AltUnityTesterEditor.EditorConfiguration.GetInstrumentationSettings();
            AltUnityBuilder.InsertAltUnityInScene(buildPlayerOptions.scenes[0], instrumentationSettings);

            var results = BuildPipeline.BuildPlayer(buildPlayerOptions);
            AltUnityBuilder.RemoveAltUnityTesterFromScriptingDefineSymbols(BuildTargetGroup.Android);


#if UNITY_2017
            if (results.Equals(""))
            {
                logger.Info("No Build Errors");
                EditorApplication.Exit(0);

            }
            else
                {
                    logger.Error("Build Error!");
                    EditorApplication.Exit(1);
                }

#else
            if (results.summary.totalErrors == 0)
            {
                logger.Info("No Build Errors");

            }
            else
            {
                logger.Error("Total Errors: " + results.summary.totalErrors);
                logger.Error("Build Error! " + results.steps + "\n Result: " + results.summary.result + "\n Stripping info: " + results.strippingInfo);
                EditorApplication.Exit(1);
            }

#endif

            logger.Info("Finished. " + PlayerSettings.productName + " : " + PlayerSettings.bundleVersion);
            EditorApplication.Exit(0);
        }
        catch (Exception exception)
        {
            logger.Error(exception);
            EditorApplication.Exit(1);
        }

    }

    [MenuItem("Build/iOS")]
    protected static void IosBuildFromCommandLine()
    {
        try
        {
            string versionNumber = DateTime.Now.ToString("yyMMddHHss");
            PlayerSettings.companyName = "Altom";
            PlayerSettings.productName = "sampleGame";
            PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.iOS, "fi.altom.altunitytester");
            PlayerSettings.bundleVersion = versionNumber;
            PlayerSettings.iOS.appleEnableAutomaticSigning = true;
            PlayerSettings.iOS.appleDeveloperTeamID = "59ESG8ELF5";
            PlayerSettings.SetApiCompatibilityLevel(BuildTargetGroup.iOS, ApiCompatibilityLevel.NET_4_6);
            logger.Debug("Starting IOS build..." + PlayerSettings.productName + " : " + PlayerSettings.bundleVersion);

            var buildPlayerOptions = new BuildPlayerOptions
            {
                locationPathName = "sampleGame",
                scenes = new string[]
                {
                    "Assets/AltUnityTester/Examples/Scenes/Scene 1 AltUnityDriverTestScene.unity",
                    "Assets/AltUnityTester/Examples/Scenes/Scene 2 Draggable Panel.unity",
                    "Assets/AltUnityTester/Examples/Scenes/Scene 3 Drag And Drop.unity",
                    "Assets/AltUnityTester/Examples/Scenes/Scene 4 No Cameras.unity",
                    "Assets/AltUnityTester/Examples/Scenes/Scene 5 Keyboard Input.unity",
                    "Assets/AltUnityTester/Examples/Scenes/Scene6.unity"
                },

                target = BuildTarget.iOS,
                options = BuildOptions.Development
            };

            AltUnityBuilder.AddAltUnityTesterInScritpingDefineSymbolsGroup(BuildTargetGroup.iOS);
            var instrumentationSettings = AltUnityTesterEditor.EditorConfiguration == null ? new AltUnityInstrumentationSettings() : AltUnityTesterEditor.EditorConfiguration.GetInstrumentationSettings();
            AltUnityBuilder.InsertAltUnityInScene(buildPlayerOptions.scenes[0], instrumentationSettings);

            var results = BuildPipeline.BuildPlayer(buildPlayerOptions);

#if UNITY_2017
            if (results.Equals(""))
            {
                logger.Info("No Build Errors");

            }
            else
            logger.Error("Build Error!");
            EditorApplication.Exit(1);

#else
            if (results.summary.totalErrors == 0)
            {
                logger.Info("No Build Errors");

            }
            else
            {
                logger.Error("Build Error!");
                EditorApplication.Exit(1);
            }

#endif
            logger.Info("Finished. " + PlayerSettings.productName + " : " + PlayerSettings.bundleVersion);
            EditorApplication.Exit(0);

        }
        catch (Exception exception)
        {
            logger.Error(exception);
            EditorApplication.Exit(1);
        }
    }
}
