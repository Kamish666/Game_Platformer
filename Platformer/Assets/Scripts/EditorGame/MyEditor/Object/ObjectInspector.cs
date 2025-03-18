using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ObjectInspector : MonoBehaviour
{
    [SerializeField] private GameObject parameterPanelPrefab;
    [SerializeField] private GameObject togglePanelPrefab;
    [SerializeField] private Transform uiParent;
    private Dictionary<string, InputField> inputFields = new Dictionary<string, InputField>();
    
    public void DisplayObjectParameters(GameObject obj)
    {
        if (obj == null)
        {
            Debug.Log("Объект не выбран.");
            return;
        }
        
        ClearUI();
        
        Transform objTransform = obj.transform;
        CreateUIField("Координаты X", objTransform.position.x.ToString(), value => objTransform.position = new Vector3(float.Parse(value), objTransform.position.y, objTransform.position.z));
        CreateUIField("Координаты Y", objTransform.position.y.ToString(), value => objTransform.position = new Vector3(objTransform.position.x, float.Parse(value), objTransform.position.z));
        CreateUIField("Поворот Z", objTransform.rotation.eulerAngles.z.ToString(), value => objTransform.rotation = Quaternion.Euler(0, 0, float.Parse(value)));
        
        MonoBehaviour[] scripts = obj.GetComponents<MonoBehaviour>();
        foreach (MonoBehaviour script in scripts)
        {
            System.Type type = script.GetType();
            while (type != null && type != typeof(MonoBehaviour))
            {
                Debug.Log($"Скрипт: {type.Name}");
                FieldInfo[] fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                foreach (FieldInfo field in fields)
                {
                    if (field.IsPublic || field.GetCustomAttribute<SerializeField>() != null)
                    {
                        if (field.FieldType == typeof(bool))
                        {
                            bool boolValue = (bool)field.GetValue(script);
                            CreateUIField(field.Name, boolValue, newValue => field.SetValue(script, newValue == "True"));
                        }
                        else
                        {
                            object fieldValue = field.GetValue(script);
                            CreateUIField(field.Name, fieldValue, newValue => field.SetValue(script, ConvertValue(newValue, field.FieldType)));
                        }

                    }
                }
                type = type.BaseType;
            }
        }
    }
    
    private void ClearUI()
    {
        foreach (Transform child in uiParent)
        {
            Destroy(child.gameObject);
        }
        inputFields.Clear();
    }

    private void CreateUIField(string name, object value, System.Action<string> onValueChanged)
    {
        GameObject panel;

        if (value is bool) // Проверяем тип данных
        {
            panel = Instantiate(togglePanelPrefab, uiParent);
            Text label = panel.transform.Find("Label").GetComponent<Text>();
            Toggle toggle = panel.transform.Find("Toggle").GetComponent<Toggle>();

            label.text = name;
            bool boolValue = (bool)value; // Приводим object к bool
            toggle.isOn = boolValue;

            toggle.onValueChanged.AddListener(isChecked => onValueChanged(isChecked.ToString()));
        }
        else
        {
            panel = Instantiate(parameterPanelPrefab, uiParent);
            Text label = panel.transform.Find("Label").GetComponent<Text>();
            InputField inputField = panel.transform.Find("InputField").GetComponent<InputField>();

            label.text = name;
            inputField.text = value.ToString();
            inputField.onEndEdit.AddListener(value => onValueChanged(value));
        }
    }


    private object ConvertValue(string value, System.Type type)
    {
        if (type == typeof(int)) return int.Parse(value);
        if (type == typeof(float)) return float.Parse(value);
        if (type == typeof(bool)) return bool.Parse(value);
        return value;
    }
}