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
    [SerializeField] private GameObject vector2ElementPrefab;
    [SerializeField] private GameObject vector2ScrollViewPrefab;
    [SerializeField] private Transform uiParent;

    private Dictionary<string, InputField> inputFields = new Dictionary<string, InputField>();

    public static ObjectInspector instance;

    private void Awake()
    {
        instance = this;
    }

    public void DisplayObjectParameters(GameObject obj)
    {
        if (obj == null)
        {
            Debug.Log("Объект не выбран.");
            return;
        }

        ClearUI();

        Transform objTransform = obj.transform;

        CreateUIField("Координаты X", objTransform.position.x,
            value => objTransform.position = new Vector3(float.Parse(value), objTransform.position.y, objTransform.position.z));

        CreateUIField("Координаты Y", objTransform.position.y,
            value => objTransform.position = new Vector3(objTransform.position.x, float.Parse(value), objTransform.position.z));

        CreateUIField("Поворот Z", objTransform.rotation.eulerAngles.z,
            value => objTransform.rotation = Quaternion.Euler(0, 0, float.Parse(value)));

        MonoBehaviour[] scripts = obj.GetComponents<MonoBehaviour>();

        HashSet<string> shownFields = new HashSet<string>();

        foreach (MonoBehaviour script in scripts)
        {
            Type type = script.GetType();
            while (type != null && type != typeof(MonoBehaviour))
            {
                FieldInfo[] fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);
                foreach (FieldInfo field in fields)
                {
                    string uniqueKey = field.Name + "_" + field.DeclaringType;

                    if (shownFields.Contains(uniqueKey))
                        continue;

                    shownFields.Add(uniqueKey);

                    if (field.GetCustomAttribute<GameEditorAnnotation>() == null)
                        continue;

                    var annotation = field.GetCustomAttribute<GameEditorAnnotation>();
                    string displayName = string.IsNullOrEmpty(annotation.DisplayName) ? field.Name : annotation.DisplayName;

                    if (field.FieldType == typeof(bool))
                    {
                        bool boolValue = (bool)field.GetValue(script);
                        CreateUIField(displayName, boolValue, newValue => field.SetValue(script, newValue == "True"));
                    }
                    else if (field.FieldType == typeof(Vector2[]))
                    {
                        Vector2[] points = (Vector2[])field.GetValue(script);
                        CreateVector2Scroll(displayName, points, updatedArray => field.SetValue(script, updatedArray));
                    }
                    else
                    {
                        object fieldValue = field.GetValue(script);
                        CreateUIField(displayName, fieldValue, newValue => field.SetValue(script, ConvertValue(newValue, field.FieldType)));
                    }
                }

                type = type.BaseType;
            }
        }

    }

    public void ClearUI()
    {
        foreach (Transform child in uiParent)
            Destroy(child.gameObject);

        inputFields.Clear();
    }

    private void CreateUIField(string name, object value, Action<string> onValueChanged)
    {
        GameObject panel;

        if (value is bool)
        {
            panel = Instantiate(togglePanelPrefab, uiParent);
            Text label = panel.transform.Find("Label").GetComponent<Text>();
            Toggle toggle = panel.transform.Find("Toggle").GetComponent<Toggle>();

            label.text = name;
            toggle.isOn = (bool)value;
            toggle.onValueChanged.AddListener(isChecked => onValueChanged(isChecked.ToString()));
        }
        else
        {
            panel = Instantiate(parameterPanelPrefab, uiParent);
            Text label = panel.transform.Find("Label").GetComponent<Text>();
            InputField inputField = panel.transform.Find("InputField").GetComponent<InputField>();

            label.text = name;
            ConfigureInputField(inputField, value, onValueChanged);
        }
    }



    private void CreateVector2Scroll(string name, Vector2[] points, Action<Vector2[]> onChanged)
    {
        GameObject scrollViewObj = Instantiate(vector2ScrollViewPrefab, uiParent);
        scrollViewObj.transform.Find("Title").GetComponent<Text>().text = name;

        Transform content = scrollViewObj.transform.Find("Scroll View/Viewport/Content");

        List<Vector2> pointList = new List<Vector2>(points);

        void Redraw()
        {
            foreach (Transform child in content)
                Destroy(child.gameObject);

            for (int i = 0; i < pointList.Count; i++)
            {
                int index = i;
                Vector2 point = pointList[i];
                float prevX = point.x;
                float prevY = point.y;

                GameObject element = Instantiate(vector2ElementPrefab, content);
                element.transform.Find("Index").GetComponent<Text>().text = (i + 1).ToString();

                InputField xField = element.transform.Find("X").GetComponent<InputField>();
                InputField yField = element.transform.Find("Y").GetComponent<InputField>();


                ConfigureInputField(xField, point.x, newX =>
                {
                    prevX = float.Parse(newX);
                    pointList[index] = new Vector2(prevX, pointList[index].y);
                    onChanged(pointList.ToArray());
                });

                ConfigureInputField(yField, point.y, newY =>
                {
                    prevY = float.Parse(newY);
                    pointList[index] = new Vector2(pointList[index].x, prevY);
                    onChanged(pointList.ToArray());
                });


                Button removeButton = element.transform.Find("Remove").GetComponent<Button>();
                removeButton.onClick.AddListener(() =>
                {
                    pointList.RemoveAt(index);
                    onChanged(pointList.ToArray());
                    Redraw();
                });
            }
        }

        Button addButton = scrollViewObj.transform.Find("AddButton").GetComponent<Button>();
        addButton.onClick.AddListener(() =>
        {
            pointList.Add(Vector2.zero);
            onChanged(pointList.ToArray());
            Redraw();
        });

        Redraw();
    }

    private void ConfigureInputField(InputField inputField, object initialValue, Action<string> onValueChanged)
    {
        string lastValidValue = initialValue.ToString();
        inputField.text = lastValidValue;

        inputField.onEndEdit.AddListener((string newValueRaw) =>
        {
            string newValue = newValueRaw.Replace('.', ',').Trim();
            bool isValid = false;

            if (initialValue is float || initialValue is double)
            {
                isValid = float.TryParse(newValue, out _);
            }
            else if (initialValue is int)
            {
                isValid = int.TryParse(newValue, out _);
            }
            else
            {
                isValid = true;
            }

            if (isValid)
            {
                lastValidValue = newValue;
                onValueChanged(newValue);
            }
            else
            {
                inputField.text = lastValidValue;
            }
        });

        inputField.onValidateInput += (string text, int charIndex, char addedChar) =>
        {
            if (initialValue is int)
                return char.IsDigit(addedChar) ? addedChar : '\0';
            if (initialValue is float || initialValue is double)
                return (char.IsDigit(addedChar) || addedChar == '.' || addedChar == ',' || addedChar == '-') ? addedChar : '\0';
            return addedChar;
        };
    }


    private object ConvertValue(string value, Type type)
    {
        if (type == typeof(int)) return int.Parse(value);
        if (type == typeof(float)) return float.Parse(value);
        if (type == typeof(bool)) return bool.Parse(value);
        return value;
    }
}