using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;

public class ObjectSaveHandler : MonoBehaviour, ISaveHandler
{
    [SerializeField] private Transform _enemyParent;
    [SerializeField] private string _fileName = "objectData.json";

    private string _path;

    private void Awake()
    {
        if (_enemyParent == null)
            _enemyParent = GameObject.Find("Enemy").transform;
        Debug.Log("ObjectSaveHandler");
    }

    public void Save(string pathForFolder)
    {
        GetFullPath(pathForFolder);

        List<SavedObjectData> dataList = new List<SavedObjectData>();

        foreach (Transform child in _enemyParent)
        {
            var data = new SavedObjectData
            {
                prefabName = child.name,
                position = child.position,
                rotation = child.rotation.eulerAngles,
                components = new List<ComponentData>()
            };

            MonoBehaviour[] scripts = child.GetComponents<MonoBehaviour>();
            foreach (var script in scripts)
            {
                if (script == null) continue;

                Type type = script.GetType();
                while (type != null && type != typeof(MonoBehaviour))
                {
                    var compData = new ComponentData
                    {
                        scriptType = type.AssemblyQualifiedName,
                        fields = new List<Field>()
                    };

                    FieldInfo[] fieldInfos = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                    foreach (var field in fieldInfos)
                    {
                        if (/*(field.IsPublic || field.GetCustomAttribute<SerializeField>() != null) &&*/ field.GetCustomAttribute<GameEditorAnnotation>() != null)
                        {
                            object val = field.GetValue(script);
                            if (val != null)
                            {
                                string jsonValue;

                                if (field.FieldType == typeof(Vector2[]))
                                {
                                    jsonValue = JsonUtility.ToJson(new Vector2ArrayWrapper { array = (Vector2[])val });
                                }
                                else
                                {
                                    jsonValue = JsonUtility.ToJson(new Wrapper { value = val.ToString() });
                                }

                                compData.fields.Add(new Field
                                {
                                    key = field.Name,
                                    value = jsonValue
                                });
                            }
                        }
                    }

                    data.components.Add(compData);
                    type = type.BaseType;
                }
            }

            dataList.Add(data);
        }

        FileHandler.SaveToJSON(dataList, _path);
    }

    public void Load(string pathForFolder)
    {
        GetFullPath(pathForFolder);
        var dataList = FileHandler.ReadListFromJSON<SavedObjectData>(_path);

        foreach (Transform child in _enemyParent)
            Destroy(child.gameObject);

        foreach (var objData in dataList)
        {
            GameObject prefab = Resources.Load<GameObject>(objData.prefabName);
            GameObject obj = prefab != null ? Instantiate(prefab) : new GameObject(objData.prefabName);

            obj.name = objData.prefabName;
            obj.transform.parent = _enemyParent;
            obj.transform.position = objData.position;
            obj.transform.rotation = Quaternion.Euler(objData.rotation);

            foreach (var compData in objData.components)
            {
                Type type = Type.GetType(compData.scriptType);
                if (type == null) continue;

                var script = obj.GetComponent(type) ?? obj.AddComponent(type);
                foreach (var fieldEntry in compData.fields)
                {
                    FieldInfo field = type.GetField(fieldEntry.key, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                    if (field != null)
                    {
                        object val;
                        if (field.FieldType == typeof(Vector2[]))
                        {
                            val = JsonUtility.FromJson<Vector2ArrayWrapper>(fieldEntry.value).array;
                        }
                        else
                        {
                            string raw = JsonUtility.FromJson<Wrapper>(fieldEntry.value).value;
                            val = ConvertValue(raw, field.FieldType);
                        }

                        field.SetValue(script, val);
                    }
                }
            }
        }
    }

    private object ConvertValue(string value, Type type)
    {
        if (type == typeof(int)) return int.Parse(value);
        if (type == typeof(float)) return float.Parse(value);
        if (type == typeof(bool)) return bool.Parse(value);
        if (type == typeof(string)) return value;
        if (type == typeof(Vector2)) return JsonUtility.FromJson<Vector2>(value);
        return null;
    }

    private string GetFullPath(string path) => _path = Path.Combine(path, _fileName);

}




[Serializable]
public class SavedObjectData
{
    public string prefabName;
    public Vector3 position;
    public Vector3 rotation;
    public List<ComponentData> components;
}

[Serializable]
public class ComponentData
{
    public string scriptType;
    public List<Field> fields;
}

[Serializable]
public class Field
{
    public string key;
    public string value;
}


[Serializable]
public class Wrapper
{
    public string value;
}

[Serializable]
public class Vector2ArrayWrapper
{
    public Vector2[] array;
}


