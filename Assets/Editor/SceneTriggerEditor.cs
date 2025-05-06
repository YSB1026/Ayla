using UnityEditor;
using UnityEngine;
using System.Linq;

[CustomEditor(typeof(SceneTrigger))]
public class SceneTriggerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();  // ��ü ������Ʈ

        // sceneName �Ӽ� �׸��� (��Ӵٿ�)
        SerializedProperty sceneNameProp = serializedObject.FindProperty("sceneName");
        if (sceneNameProp != null)
        {
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
