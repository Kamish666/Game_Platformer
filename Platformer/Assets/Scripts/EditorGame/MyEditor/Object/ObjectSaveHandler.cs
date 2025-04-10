using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class ObjectSaveHandler : MonoBehaviour
{
    [SerializeField] private string _fileName = "objectData.json";
    [SerializeField] private Transform _enemyParent;

    private void Start()
    {
        if (_enemyParent == null)
            _enemyParent = GameObject.Find("Enemy").transform;
    }

    public void OnSave()
    {
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
                        if (field.IsPublic || field.GetCustomAttribute<SerializeField>() != null)
                        {
                            object val = field.GetValue(script);
                            if (val != null)
                            {
                                compData.fields.Add(new Field
                                {
                                    key = field.Name,
                                    value = JsonUtility.ToJson(new Wrapper { value = val.ToString() })
                                });
                            }
                        }
                    }

                    type = type.BaseType;
                    data.components.Add(compData);
                }
            }

            dataList.Add(data);
        }

        FileHandler.SaveToJSON(dataList, _fileName);
    }

    public void OnLoad()
    {
        var dataList = FileHandler.ReadListFromJSON<SavedObjectData>(_fileName);

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
                        string raw = JsonUtility.FromJson<Wrapper>(fieldEntry.value).value;
                        object val = ConvertValue(raw, field.FieldType);
                        field.SetValue(script, val);
                    }
                }
            }
        }
    }

    private object ConvertValue(string value, Type type)
    {
        try
        {
            if (type == typeof(int)) return int.Parse(value);
            if (type == typeof(float)) return float.Parse(value);
            if (type == typeof(bool)) return bool.Parse(value);
            if (type == typeof(string)) return value;
            if (type == typeof(Vector2)) return JsonUtility.FromJson<Vector2>(value);
            if (type == typeof(Vector3)) return JsonUtility.FromJson<Vector3>(value);
        }
        catch (Exception e)
        {
            Debug.LogWarning($"������ �������������� \"{value}\" � ��� {type}: {e.Message}");
        }

        return null;
    }

    [Serializable]
    private class Wrapper
    {
        public string value;
    }
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




