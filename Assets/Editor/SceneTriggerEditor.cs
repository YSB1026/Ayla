using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.Linq;

[CustomEditor(typeof(SceneTrigger))]
public class SceneTriggerEditor : Editor
{
    private string[] sceneNames;
    private SerializedProperty sceneNameProp;

    void OnEnable()
    {
        sceneNameProp = serializedObject.FindProperty("sceneName");

        sceneNames = EditorBuildSettings.scenes
            .Where(s => s.enabled)
            .Select(s => System.IO.Path.GetFileNameWithoutExtension(s.path))
            .ToArray();
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        if (sceneNames.Length > 0)
        {
            int index = Mathf.Max(0, System.Array.IndexOf(sceneNames, sceneNameProp.stringValue));
            index = EditorGUILayout.Popup("Scene Name", index, sceneNames);
            sceneNameProp.stringValue = sceneNames[index];
        }
        else
        {
            EditorGUILayout.HelpBox("Cannot Found Scene in Build Settings", MessageType.Warning);
        }

        serializedObject.ApplyModifiedProperties();
    }
}
