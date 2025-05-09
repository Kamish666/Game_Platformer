using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerColors : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;

    [SerializeField] private Material redMaterial;
    [SerializeField] private Material greenMaterial;
    [SerializeField] private Material blueMaterial;
    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();

        ChangeColor changeColorScript = ChangeColor.instance;
        if (changeColorScript != null)
        {
            changeColorScript.enemyColors += OnColorChanged;
        }
        else
        {
            Debug.LogWarning("ChangeColor script not found in the scene");
        }
    }

    private void OnColorChanged(bool red, bool green, bool blue)
    {
        if (red)
        {
            ChacgedMaterial(redMaterial);
        }
        else if (green)
        {
            ChacgedMaterial(greenMaterial);
        }
        else
        {
            ChacgedMaterial(blueMaterial);
        }

    }

    private void ChacgedMaterial(Material material)
    {
        _spriteRenderer.material = material;
    }
}
