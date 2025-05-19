using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;

public class ParticleSystemColors : MonoBehaviour
{
    [SerializeField] private bool _isRed;
    [SerializeField] private bool _isGreen;
    [SerializeField] private bool _isBlue;

    [SerializeField] private Material _redMaterial;
    [SerializeField] private Material _greenMaterial;
    [SerializeField] private Material _blueMaterial;
    [SerializeField] private Material _blackMaterial;

    [SerializeField] private Gradient _redGradient;
    [SerializeField] private Gradient _greenGradient;
    [SerializeField] private Gradient _blueGradient;
    [SerializeField] private Gradient _blackGradient;

    private ParticleSystem _particleSystem;
    private ParticleSystem.ColorOverLifetimeModule _colorOverLifetime;
    private ParticleSystemRenderer _renderer;

    private ChangeColor changeColor;

    private void Awake()
    {
        _particleSystem = GetComponent<ParticleSystem>();
        _colorOverLifetime = _particleSystem.colorOverLifetime;
        _renderer = GetComponent<ParticleSystemRenderer>();

        ChangeColor changeColor = ChangeColor.instance;
        if (changeColor != null)
        {
            changeColor.enemyColors += OnColorChanged;
        }
        else
        {
            Debug.LogWarning("ChangeColor script not found in the scene");
        }

        var player = GetComponentInParent<Health>();
        if (player != null)
        {
            player.OnDie += DeactiveScript;
        }
        }

    private void OnColorChanged(bool red, bool green, bool blue)
    {
        if (!_isGreen && !_isBlue && !_isRed)
        {
            ApplyMaterialAndGradient(_blackMaterial, _blackGradient);
        }
        else if (_isRed && red)
        {
            ApplyMaterialAndGradient(_redMaterial, _redGradient);
        }
        else if (_isGreen && green)
        {
            ApplyMaterialAndGradient(_greenMaterial, _greenGradient);
        }
        else if (_isBlue && blue)
        {
            ApplyMaterialAndGradient(_blueMaterial, _blueGradient);
        }
        else
        {
            Disable();
        }
    }

    private void Disable()
    {
        GetComponent<ParticleSystemRenderer>().enabled = false;
    }

    private void ApplyMaterialAndGradient(Material material, Gradient gradient)
    {
        _renderer.material = material;
        _colorOverLifetime.enabled = true;
        _colorOverLifetime.color = new ParticleSystem.MinMaxGradient(gradient);
        GetComponent<ParticleSystemRenderer>().enabled = true;
    }

    private void DeactiveScript()
    {
        GetComponent<ParticleSystem>().Stop();
    }

    private void OnEnable()
    {
        if (changeColor ==  null)
        {
            changeColor = ChangeColor.instance;
        }
        if (changeColor !=  null)
        {
            OnColorChanged(changeColor.IsRed, changeColor.IsGreen, changeColor.IsBlue);
        }
    }
}
