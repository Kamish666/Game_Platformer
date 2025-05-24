using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasScaler))]
public class ScaleByMinScreenSize : MonoBehaviour
{
    public Vector2 referenceResolution = new Vector2(1920, 1080);
    private CanvasScaler _scaler;

    void Start()
    {
        ApplyScale();
    }

    void OnEnable()
    {
        ApplyScale();
    }

    private void ApplyScale()
    {
        if (_scaler == null)
            _scaler = GetComponent<CanvasScaler>();

        _scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        _scaler.referenceResolution = referenceResolution;
        _scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;

        float scaleX = (float)Screen.width / referenceResolution.x;
        float scaleY = (float)Screen.height / referenceResolution.y;

        // Масштабируем по меньшей стороне
        _scaler.matchWidthOrHeight = (scaleX < scaleY) ? 0 : 1;
    }
}
