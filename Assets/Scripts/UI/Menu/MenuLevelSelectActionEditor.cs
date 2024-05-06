#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.SceneManagement;

[CustomEditor(typeof(MenuLevelSelectAction))]
public class MenuLevelSelectActionEditor : UnityEditor.Editor
{
    public override void OnInspectorGUI()
    {
        EditorGUI.BeginChangeCheck();

        DrawPropertiesExcluding(serializedObject, "Levels");

        EditorGUILayout.PropertyField(m_Levels, true);

        if (GUILayout.Button("Reset"))
        {
            UpdateStats();
        }

        if (EditorGUI.EndChangeCheck())
        {
            serializedObject.ApplyModifiedProperties();
        }
    }

    private void UpdateStats()
    {
        int size = m_Levels.arraySize;
        Dictionary<string, int> lookup = new();
        for (int i = 0; i < size; i++)
        {
            var val = m_Levels.GetArrayElementAtIndex(i);
            string name = val.FindPropertyRelative("name").stringValue;
            lookup[name] = i;
        }

        int index = size;

        foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
        {
            if (scene.enabled)
            {

                string name = scene.path.Split("/").Last().Split(".").First();
                int i;
                if (lookup.ContainsKey(name))
                {
                    i = lookup[name];
                }
                else
                {
                    m_Levels.InsertArrayElementAtIndex(index);
                    i = index++;
                }

                var val = m_Levels.GetArrayElementAtIndex(i);
                val.FindPropertyRelative("LevelID").stringValue = name;
            }
        }
    }

    private void OnEnable()
    {
        m_Levels = serializedObject.FindProperty("Levels");
    }

    private SerializedProperty m_Levels;
}
#endif