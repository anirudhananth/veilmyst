#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.SceneManagement;

[CustomEditor(typeof(SavesManager))]
public class SavesManagerEditor : UnityEditor.Editor
{
    public override void OnInspectorGUI()
    {
        DrawPropertiesExcluding(serializedObject, "m_LevelStats");

        EditorGUI.BeginChangeCheck();

        EditorGUILayout.PropertyField(m_LevelStats, true);

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
        int size = m_LevelStats.arraySize;
        Dictionary<string, int> lookup = new();
        for (int i = 0; i < size; i++)
        {
            var val = m_LevelStats.GetArrayElementAtIndex(i);
            string name = val.FindPropertyRelative("name").stringValue;
            lookup[name] = i;
        }

        int index = size;

        string prevLevelName = "";
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
                    m_LevelStats.InsertArrayElementAtIndex(index);
                    i = index;
                    var newArr = m_LevelStats.GetArrayElementAtIndex(i).FindPropertyRelative("crownsID");
                    newArr.ClearArray();
                    index++;
                }

                var val = m_LevelStats.GetArrayElementAtIndex(i);
                val.FindPropertyRelative("name").stringValue = name;
                Debug.Log(prevLevelName);
                if (!string.IsNullOrEmpty(prevLevelName))
                {
                    val.FindPropertyRelative("prereqLevel").stringValue = prevLevelName;
                }
                else
                {
                    val.FindPropertyRelative("unlocked").boolValue = true;
                }
                prevLevelName = name;
                if (EditorSceneManager.GetActiveScene().name == name)
                {
                    // Populate the crowns ID
                    var idArr = val.FindPropertyRelative("crownsID");
                    int idArrSize = 0;
                    idArr.ClearArray();
                    foreach (Collectible collectible in FindObjectsOfType<Collectible>())
                    {
                        idArr.InsertArrayElementAtIndex(idArrSize);
                        idArr.GetArrayElementAtIndex(idArrSize).stringValue = Collectible.GenerateID(name, collectible.transform);
                        idArrSize++;
                    }
                }
            }
        }
    }

    private void OnEnable()
    {
        m_LevelStats = serializedObject.FindProperty("m_LevelStats");
    }

    private SerializedProperty m_LevelStats;
}
#endif