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
                                string serializedValue;

                                // Попробуем сериализовать как JSON
                                if (field.FieldType.IsPrimitive || field.FieldType == typeof(string))
                                {
                                    serializedValue = val.ToString();
                                }
                                else
                                {
                                    serializedValue = JsonUtility.ToJson(val);
                                }

                                compData.fields.Add(new Field
                                {
                                    key = field.Name,
                                    type = field.FieldType.AssemblyQualifiedName,
                                    value = serializedValue
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
                Type scriptType = Type.GetType(compData.scriptType);
                if (scriptType == null) continue;

                var script = obj.GetComponent(scriptType) ?? obj.AddComponent(scriptType);
                foreach (var fieldEntry in compData.fields)
                {
                    FieldInfo field = scriptType.GetField(fieldEntry.key, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                    if (field != null)
                    {
                        Type fieldType = Type.GetType(fieldEntry.type);
                        if (fieldType == null) continue;

                        object val = DeserializeField(fieldEntry.value, fieldType);
                        field.SetValue(script, val);
                    }
                }
            }
        }
    }

    private object DeserializeField(string value, Type type)
    {
        try
        {
            if (type == typeof(int)) return int.Parse(value);
            if (type == typeof(float)) return float.Parse(value);
            if (type == typeof(bool)) return bool.Parse(value);
            if (type == typeof(string)) return value;
            if (type == typeof(Vector2)) return JsonUtility.FromJson<Vector2>(value);
            if (type == typeof(Vector3)) return JsonUtility.FromJson<Vector3>(value);
            if (type.IsEnum) return Enum.Parse(type, value);

            // Проверка на List<T>
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>))
            {
                Type itemType = type.GetGenericArguments()[0]; // Получаем T из List<T>
                Type listType = typeof(List<>).MakeGenericType(itemType);
                object list = Activator.CreateInstance(listType);
                MethodInfo addMethod = listType.GetMethod("Add");

                // Десериализуем массив объектов из JSON
                object[] items = JsonUtility.FromJson<SerializationWrapper>(value).items;
                foreach (object item in items)
                {
                    addMethod.Invoke(list, new[] { Convert.ChangeType(item, itemType) });
                }

                return list;
            }

            if (!type.IsPrimitive && !type.IsEnum)
                return JsonUtility.FromJson(value, type);
        }
        catch (Exception e)
        {
            Debug.LogWarning($"Не удалось десериализовать поле типа {type}: {e.Message}");
        }

        return null;
    }

    // Вспомогательный класс для десериализации списка
    [Serializable]
    private class SerializationWrapper
    {
        public object[] items;
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
    public string type; // Тип поля в строковом виде
    public string value; // Сериализованное значение
}





