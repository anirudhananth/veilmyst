#if UNITY_EDITOR
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

[CustomEditor(typeof(Collectible))]
public class CollectibleEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawPropertiesExcluding(serializedObject, "CollectibleID", "m_CollectibleID");

        EditorGUI.BeginDisabledGroup(true);
        if (m_CollectibleID.stringValue == "")
        {
            GenerateID();
        }
        EditorGUILayout.PropertyField(m_CollectibleID, true);
        EditorGUI.EndDisabledGroup();
    }

    private void GenerateID()
    {
        string sceneName = EditorSceneManager.GetActiveScene().name;
        m_CollectibleID.stringValue = Collectible.GenerateID(sceneName, target.GetComponent<Collectible>().transform.position);
    }

    private void OnEnable()
    {
        m_CollectibleID = serializedObject.FindProperty("m_CollectibleID");
    }

    private SerializedProperty m_CollectibleID;
}
#endif