using UnityEditor;
using UnityEngine;
using System.Linq;

[CustomEditor(typeof(PendantEvent))]
public class PendantEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();  // 객체 업데이트

        // sceneName 속성 그리기 (드롭다운)
        var puzzleProp = serializedObject.FindProperty("puzzleObject");
        EditorGUILayout.PropertyField(puzzleProp);

        var pendantColorProp = serializedObject.FindProperty("pendantColor");
        EditorGUILayout.PropertyField(pendantColorProp);

        var greenPendantTrigger = serializedObject.FindProperty("eventAfterRecallScene");
        EditorGUILayout.PropertyField(greenPendantTrigger);

        SerializedProperty sceneNameProp = serializedObject.FindProperty("sceneName");
        if (sceneNameProp != null)
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("회상 씬", EditorStyles.boldLabel);

            var sceneNames = EditorBuildSettings.scenes
                .Where(s => s.enabled)
                .Select(s => System.IO.Path.GetFileNameWithoutExtension(s.path))
                .ToArray();

            if (sceneNames.Length > 0)
            {
                // 드롭다운
                int index = Mathf.Max(0, System.Array.IndexOf(sceneNames, sceneNameProp.stringValue));
                int selectedIndex = EditorGUILayout.Popup("Scene Name", index, sceneNames);

                // 선택된 씬 이름을 속성에 적용
                if (selectedIndex >= 0 && selectedIndex < sceneNames.Length)
                {
                    sceneNameProp.stringValue = sceneNames[selectedIndex];
                }
            }
            else
            {
                EditorGUILayout.HelpBox("No scenes found in Build Settings.", MessageType.Warning);
            }
        }

        serializedObject.ApplyModifiedProperties();  // 속성 적용
    }
}
