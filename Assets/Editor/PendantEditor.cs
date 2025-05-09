using UnityEditor;
using UnityEngine;
using System.Linq;

[CustomEditor(typeof(PendantEvent))]
public class PendantEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();  // ��ü ������Ʈ

        // sceneName �Ӽ� �׸��� (��Ӵٿ�)
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
            EditorGUILayout.LabelField("ȸ�� ��", EditorStyles.boldLabel);

            var sceneNames = EditorBuildSettings.scenes
                .Where(s => s.enabled)
                .Select(s => System.IO.Path.GetFileNameWithoutExtension(s.path))
                .ToArray();

            if (sceneNames.Length > 0)
            {
                // ��Ӵٿ�
                int index = Mathf.Max(0, System.Array.IndexOf(sceneNames, sceneNameProp.stringValue));
                int selectedIndex = EditorGUILayout.Popup("Scene Name", index, sceneNames);

                // ���õ� �� �̸��� �Ӽ��� ����
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

        serializedObject.ApplyModifiedProperties();  // �Ӽ� ����
    }
}
