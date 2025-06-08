using System.Collections;
using System.Collections.Generic;
using Unity.Profiling;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [GameEditorAnnotation("Red")]
    [SerializeField] private bool _isRed;
    [GameEditorAnnotation("Green")]
    [SerializeField] private bool _isGreen;
    [GameEditorAnnotation("Blue")]
    [SerializeField] private bool _isBlue;

    private string _checkpointID;
    private SpriteRenderer _renderer;
    private static bool _startScene = false;

    private void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        _checkpointID = $"{transform.position.x}_{transform.position.y}_{transform.position.z}";
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<ChangeColor>())
        {
            CheckpointManager.Instance.ActivateCheckpoint(transform.position, _checkpointID);
        }
    }

    private void OnActive(string checkpointID)
    {
        if (checkpointID == _checkpointID)
        {
            _renderer.color = Color.green;

            if (_startScene == false)
            {
                _startScene = true;
                string color = GetCheckpointColor();
                ChangeColor.instance.ChooceColor(color);
            }
        }
        else
        {
            _renderer.color = Color.white;
        }
    }

    private string GetCheckpointColor()
    {
        List<string> availableColors = new List<string>();
        if (_isRed) availableColors.Add("Red");
        if (_isGreen) availableColors.Add("Green");
        if (_isBlue) availableColors.Add("Blue");

        if (availableColors.Count == 0)
            return "Green"; // значение по умолчанию

        int randomIndex = UnityEngine.Random.Range(0, availableColors.Count);
        return availableColors[randomIndex];
    }

    private void ResetStartScene()
    {
        _startScene = false;
        Debug.Log("ResetStartScene");
    }

    private void OnEnable()
    {
        CheckpointManager.OnCheckpointActive += OnActive;
        SceneController.instance.OnLevelExit += ResetStartScene;
        SceneController.instance.OnLevelRestart += ResetStartScene;
    }

    private void OnDisable()
    {
        CheckpointManager.OnCheckpointActive -= OnActive;
        SceneController.instance.OnLevelExit -= ResetStartScene;
        SceneController.instance.OnLevelRestart -= ResetStartScene;
    }
}
