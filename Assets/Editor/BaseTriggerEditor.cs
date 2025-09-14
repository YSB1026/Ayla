using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BaseTrigger), true)]
public class BaseTriggerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        DrawPropertiesExcluding(serializedObject, "m_Script", "actions");

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("트리거 이후 오브젝트 액션", EditorStyles.boldLabel);

        EditorGUILayout.PropertyField(serializedObject.FindProperty("actions"), true);

        serializedObject.ApplyModifiedProperties();
    }
}
