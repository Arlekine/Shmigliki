using UnityEditor.SceneManagement;
using UnityEditor;

[InitializeOnLoad]
public static class EditorInit
{
    static EditorInit()
    {
        var pathOfFirstScene = EditorBuildSettings.scenes[0].path;
        var sceneAsset = AssetDatabase.LoadAssetAtPath<SceneAsset>(pathOfFirstScene);
        //EditorSceneManager.playModeStartScene = sceneAsset;
    }
}