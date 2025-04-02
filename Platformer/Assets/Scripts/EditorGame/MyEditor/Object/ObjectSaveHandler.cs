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
        {
            _enemyParent = GameObject.Find("Enemy").transform;
        }
    }

    public void OnSave()
    {
        List<ObjectData> dataList = new List<ObjectData>();

        foreach (Transform child in _enemyParent)
        {
            ObjectData objData = new ObjectData();
            objData.name = child.name;
            objData.position = child.position;
            objData.rotation = child.rotation.eulerAngles;
            objData.scriptParameters = new Dictionary<string, string>();

            MonoBehaviour[] scripts = child.GetComponents<MonoBehaviour>();
            foreach (MonoBehaviour script in scripts)
            {
                Type type = script.GetType();
                FieldInfo[] fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                foreach (FieldInfo field in fields)
                {
                    if (field.IsPublic || field.GetCustomAttribute<SerializeField>() != null)
                    {
                        objData.scriptParameters[field.Name] = field.GetValue(script)?.ToString();
                    }
                }
            }

            dataList.Add(objData);
        }

        FileHandler.SaveToJSON(dataList, _fileName);
    }

    public void OnLoad()
    {
        List<ObjectData> dataList = FileHandler.ReadListFromJSON<ObjectData>(_fileName);

        foreach (Transform child in _enemyParent)
        {
            Destroy(child.gameObject);
        }

        foreach (var objData in dataList)
        {
            GameObject obj = new GameObject(objData.name);
            obj.transform.parent = _enemyParent;
            obj.transform.position = objData.position;
            obj.transform.rotation = Quaternion.Euler(objData.rotation);

            MonoBehaviour[] scripts = obj.GetComponents<MonoBehaviour>();
            foreach (MonoBehaviour script in scripts)
            {
                Type type = script.GetType();
                FieldInfo[] fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                foreach (FieldInfo field in fields)
                {
                    if (field.IsPublic || field.GetCustomAttribute<SerializeField>() != null)
                    {
                        if (objData.scriptParameters.TryGetValue(field.Name, out string value))
                        {
                            field.SetValue(script, ConvertValue(value, field.FieldType));
                        }
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
        return value;
    }
}

[Serializable]
public class ObjectData
{
    public string name;
    public Vector3 position;
    public Vector3 rotation;
    public Dictionary<string, string> scriptParameters;
}

