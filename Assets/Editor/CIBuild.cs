using System.IO;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.SceneManagement;
using UnityEngine;

public class CIBuild
{
    public static void BuildAndroid()
    {
        // 目标平台/架构设置（Unity 6 必须显式指定，否则报 Target architecture not specified）
        EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Android, BuildTarget.Android);
        // 2 核构建机用 Mono 后端：免 IL2CPP C++ 编译（IL2CPP 在这个项目要跑 2-3 小时）
        PlayerSettings.SetScriptingBackend(NamedBuildTarget.Android, ScriptingImplementation.Mono2x);
        PlayerSettings.SetApplicationIdentifier(NamedBuildTarget.Android, "com.scprx.scpsl");
        PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARM64;
        PlayerSettings.Android.minSdkVersion = AndroidSdkVersions.AndroidApiLevel23;
        PlayerSettings.Android.targetSdkVersion = AndroidSdkVersions.AndroidApiLevelAuto;
        PlayerSettings.companyName = "SCPRX";
        PlayerSettings.productName = "SCPSL Android";
        PlayerSettings.bundleVersion = "0.1.0";
        PlayerSettings.SplashScreen.show = false;

        Directory.CreateDirectory("Assets/Scenes");
        var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
        var camGo = new GameObject("Main Camera");
        camGo.tag = "MainCamera";
        var cam = camGo.AddComponent<Camera>();
        cam.clearFlags = CameraClearFlags.SolidColor;
        cam.backgroundColor = new Color(0.05f, 0.05f, 0.1f, 1f);
        camGo.transform.position = new Vector3(0f, 1f, -5f);
        var lightGo = new GameObject("Directional Light");
        var light = lightGo.AddComponent<Light>();
        light.type = LightType.Directional;
        lightGo.transform.rotation = Quaternion.Euler(50f, -30f, 0f);
        var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.transform.position = new Vector3(0f, 1f, 0f);
        EditorSceneManager.SaveScene(scene, "Assets/Scenes/Smoke.unity");
        EditorBuildSettings.scenes = new[] { new EditorBuildSettingsScene("Assets/Scenes/Smoke.unity", true) };

        Directory.CreateDirectory("build/Android");
        var options = new BuildPlayerOptions
        {
            scenes = new[] { "Assets/Scenes/Smoke.unity" },
            locationPathName = "build/Android/Smoke.apk",
            target = BuildTarget.Android,
            options = BuildOptions.None
        };
        var report = BuildPipeline.BuildPlayer(options);
        Debug.Log("BUILD RESULT: " + report.summary.result + " size=" + report.summary.totalSize);
        if (report.summary.result != UnityEditor.Build.Reporting.BuildResult.Succeeded)
        {
            EditorApplication.Exit(1);
        }
    }
}
