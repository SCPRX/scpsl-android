using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class CIBuild
{
    public static void BuildAndroid()
    {
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
