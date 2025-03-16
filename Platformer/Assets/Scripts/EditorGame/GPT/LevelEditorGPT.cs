using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelEditorGPT : MonoBehaviour
{
    public List<GameObject> availablePrefabs;
    public Transform levelContainer;
    public Vector2 gridSize = new Vector2(1, 1);
    private int selectedPrefabIndex = -1;
    private GameObject selectedObject;
    private Color originalColor;

    public Button rotateButton;
    public Dropdown prefabDropdown;
    public Image selectionHighlight;

    void Start()
    {
        rotateButton.onClick.AddListener(RotateSelectedObject);
        prefabDropdown.onValueChanged.AddListener(SetSelectedPrefab);

        prefabDropdown.ClearOptions();
        List<string> options = new List<string>();
        foreach (var prefab in availablePrefabs)
        {
            options.Add(prefab.name);
        }
        prefabDropdown.AddOptions(options);
    }

    private Vector2 SnapToGrid(Vector2 position)
    {
        float x = Mathf.Round(position.x / gridSize.x) * gridSize.x;
        float y = Mathf.Round(position.y / gridSize.y) * gridSize.y;
        return new Vector2(x, y);
    }

    void Update()
    {
        if (selectedPrefabIndex >= 0 && Input.GetMouseButtonDown(0))
        {
            Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 snappedPosition = SnapToGrid(mouseWorldPos);
            PlaceObject(snappedPosition);
        }
        else if (Input.GetMouseButtonDown(0))
        {
            SelectObjectAtMouse();
        }
    }

    public void PlaceObject(Vector2 position)
    {
        GameObject newObj = Instantiate(availablePrefabs[selectedPrefabIndex], position, Quaternion.identity, levelContainer);
        selectedPrefabIndex = -1;
    }

    private void SelectObjectAtMouse()
    {
        Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Collider2D hit = Physics2D.OverlapPoint(mouseWorldPos);

        if (hit != null)
        {
            if (selectedObject != null)
            {
                ResetObjectColor();
            }
            selectedObject = hit.gameObject;
            HighlightObject(selectedObject);
        }
    }

    private void HighlightObject(GameObject obj)
    {
        SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            originalColor = sr.color;
            sr.color = Color.yellow;
        }
    }

    private void ResetObjectColor()
    {
        if (selectedObject != null)
        {
            SpriteRenderer sr = selectedObject.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                sr.color = originalColor;
            }
        }
    }

    private void RotateSelectedObject()
    {
        if (selectedObject != null)
        {
            selectedObject.transform.Rotate(0, 0, 90);
        }
    }

    private void SetSelectedPrefab(int index)
    {
        selectedPrefabIndex = index;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.gray;
        for (float x = -10; x <= 10; x += gridSize.x)
        {
            Gizmos.DrawLine(new Vector3(x, -10, 0), new Vector3(x, 10, 0));
        }
        for (float y = -10; y <= 10; y += gridSize.y)
        {
            Gizmos.DrawLine(new Vector3(-10, y, 0), new Vector3(10, y, 0));
        }
    }
}

