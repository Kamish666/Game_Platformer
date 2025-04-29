using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class PrefabCleaner : EditorWindow
{
    private string folderPath = "Assets/PrefabForEditor"; // ������� ���� � ����� ����� � ���������

    [MenuItem("Tools/Clean Prefabs")]
    public static void ShowWindow()
    {
        GetWindow<PrefabCleaner>("Prefab Cleaner");
    }

    private void OnGUI()
    {
        GUILayout.Label("Prefab Cleaner", EditorStyles.boldLabel);
        folderPath = EditorGUILayout.TextField("Folder Path", folderPath);

        if (GUILayout.Button("Clean Prefabs"))
        {
            CleanPrefabs();
        }
    }

    private void CleanPrefabs()
    {
        string[] prefabPaths = Directory.GetFiles(folderPath, "*.prefab", SearchOption.AllDirectories);

        foreach (string prefabPath in prefabPaths)
        {
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
            if (prefab != null)
            {
                // ������� ��� ���������� ��������
                MonoBehaviour[] scripts = prefab.GetComponentsInChildren<MonoBehaviour>(true);
                foreach (var script in scripts)
                {
                    if (script != null)
                    {
                        DestroyImmediate(script, true);
                    }
                }

                // ������� ��������
                Animator animator = prefab.GetComponent<Animator>();
                if (animator != null)
                {
                    DestroyImmediate(animator, true);
                }

                // ������� ���� ���������
                BoxCollider boxCollider = prefab.GetComponent<BoxCollider>();
                if (boxCollider != null)
                {
                    DestroyImmediate(boxCollider, true);
                }

                // ��������� ��������� � �������
                PrefabUtility.SavePrefabAsset(prefab);
            }
        }

        AssetDatabase.Refresh();
        Debug.Log("Cleaned all prefabs in the specified folder.");
    }
}
