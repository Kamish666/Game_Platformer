using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSystemColors : MonoBehaviour
{
    [SerializeField] private Material redMaterial;
    [SerializeField] private Material greenMaterial;
    [SerializeField] private Material blueMaterial;

    [SerializeField] private Gradient redGradient;
    [SerializeField] private Gradient greenGradient;
    [SerializeField] private Gradient blueGradient;

    private ParticleSystem _particleSystem;
    private ParticleSystem.ColorOverLifetimeModule _colorOverLifetime;
    private ParticleSystemRenderer _renderer;

    private void Awake()
    {
        _particleSystem = GetComponent<ParticleSystem>();
        _colorOverLifetime = _particleSystem.colorOverLifetime;
        _renderer = GetComponent<ParticleSystemRenderer>();

        ChangeColor changeColorScript = ChangeColor.instance;
        if (changeColorScript != null)
        {
            changeColorScript.enemyColors += OnColorChanged;
        }
        else
        {
            Debug.LogWarning("ChangeColor script not found in the scene");
        }

        GetComponentInParent<Health>().OnDie += DeactiveScript;
    }

    private void OnColorChanged(bool red, bool green, bool blue)
    {
        if (red)
        {
            ApplyMaterialAndGradient(redMaterial, redGradient);
        }
        else if (green)
        {
            ApplyMaterialAndGradient(greenMaterial, greenGradient);
        }
        else
        {
            ApplyMaterialAndGradient(blueMaterial, blueGradient);
        }
    }

    private void ApplyMaterialAndGradient(Material material, Gradient gradient)
    {
        //_renderer.material = material;
        _colorOverLifetime.enabled = true;
        _colorOverLifetime.color = new ParticleSystem.MinMaxGradient(gradient);
    }

    private void DeactiveScript()
    {
        GetComponent<ParticleSystem>().Stop();
    }
}
