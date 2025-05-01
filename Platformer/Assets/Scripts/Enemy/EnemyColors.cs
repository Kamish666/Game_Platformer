using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyColors : MonoBehaviour
{
    private List<SpriteRenderer> _spriteRenderers = new List<SpriteRenderer>();
    private List<Collider2D> _colliders = new List<Collider2D>();


    // Переменные для каждого цвета
    [GameEditorAnnotation][SerializeField] private bool _isRedEnemy;
    [GameEditorAnnotation][SerializeField] private bool _isGreenEnemy;
    [GameEditorAnnotation][SerializeField] private bool _isBlueEnemy;

    public bool IsRedEnemy{ get{ return _isRedEnemy; } }
    public bool IsGreenEnemy { get { return _isGreenEnemy; } }
    public bool IsBlueEnemy { get { return _isBlueEnemy; } }

    private void Start()
    {
        _spriteRenderers = GetComponentsInChildren<SpriteRenderer>(true).ToList();
        _colliders = GetComponentsInChildren<Collider2D>(true).ToList();

        ChangeColor changeColorScript = ChangeColor.Instance;
        if (changeColorScript != null)
        {
            changeColorScript.enemyColors += OnColorChanged;
        }
        else
        {
            Debug.Log("ChangeColor script not found in the scene!");
            StartCoroutine(SwitchColor());
        }
    }


    private void OnColorChanged(bool red, bool green, bool blue)
    {
        // Проверка условий на основе цвета врага
        if ((!_isGreenEnemy && !_isBlueEnemy && !_isRedEnemy) ||
            (_isGreenEnemy && _isBlueEnemy && _isRedEnemy))
        {
            EnableEnemy(Color.gray);
        }       
        else if (_isRedEnemy && red)
        {
            EnableEnemy(Color.red);
        }
        else if (_isGreenEnemy && green)
        {
            EnableEnemy(Color.green);
        }
        else if (_isBlueEnemy && blue)
        {
            EnableEnemy(Color.blue);
        }
        else
        {
            DisableEnemy();
        }
    }

    private void EnableEnemy(Color color)
    {
        foreach (var col in _colliders)
            col.enabled = true;

        foreach (var sr in _spriteRenderers)
        {
            sr.enabled = true;
            sr.color = color;
        }
    }

    private void DisableEnemy()
    {
        foreach (var col in _colliders)
            col.enabled = false;

        foreach (var sr in _spriteRenderers)
            sr.enabled = false;
    }

    IEnumerator SwitchColor()
    {
        int currentColorIndex = 0;
        while (true)
        {
            if (_spriteRenderers[0].color.a == 1 ) {
                bool[] cololorsEnemy = { _isRedEnemy, _isGreenEnemy, _isBlueEnemy };
                //int currentColorIndex = cololorsEnemy.Length;
                int iteration = 0;

                while (true)
                {
                    currentColorIndex = (currentColorIndex + 1) % cololorsEnemy.Length;

                    if (cololorsEnemy[currentColorIndex] == true)
                    {
                        //Debug.Log(currentColorIndex);
                        switch (currentColorIndex)
                        {
                            case 0: OnColorChanged(true, false, false); break;
                            case 1: OnColorChanged(false, true, false); break;
                            case 2: OnColorChanged(false, false, true); break;
                        }
                        break;
                    }

                    iteration++;
                    if (iteration == cololorsEnemy.Length)
                    {
                        OnColorChanged(false, false, false);
                        break;
                    }


                }
            }
            yield return new WaitForSeconds(1f);

        }
    }

    private void OnDestroy()
    {
        ChangeColor changeColorScript = FindObjectOfType<ChangeColor>();
        if (changeColorScript != null)
        {
            changeColorScript.enemyColors -= OnColorChanged;
        }
    }

}
