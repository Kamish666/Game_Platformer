using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TreeEditor;
using UnityEngine;

public class EnemyColors : MonoBehaviour
{
    private List<SpriteRenderer> _spriteRenderers = new List<SpriteRenderer>();
    private List<Collider2D> _colliders = new List<Collider2D>();


    // Переменные для каждого цвета
    [GameEditorAnnotation][SerializeField] private bool _isRedEnemy;
    [GameEditorAnnotation][SerializeField] private bool _isGreenEnemy;
    [GameEditorAnnotation][SerializeField] private bool _isBlueEnemy;

    [SerializeField] private Material redMaterial;
    [SerializeField] private Material greenMaterial;
    [SerializeField] private Material blueMaterial;
    [SerializeField] private Material grayMaterial;

    private ChangeColor changeColor;

    public bool IsRedEnemy{ get{ return _isRedEnemy; } }
    public bool IsGreenEnemy { get { return _isGreenEnemy; } }
    public bool IsBlueEnemy { get { return _isBlueEnemy; } }

    private void Awake()
    {
        _spriteRenderers = GetComponentsInChildren<SpriteRenderer>(true).ToList();
        _colliders = GetComponentsInChildren<Collider2D>(true).ToList();

        changeColor = ChangeColor.instance;

        Debug.Log("EnemyColors");

        if (changeColor != null)
        {
            Debug.Log("EnemyColors успешно подписался");
            changeColor.enemyColors += OnColorChanged;
        }
        else
        {
            Debug.Log("EnemyColors не подписался");
            StartCoroutine(SwitchColor());
        }

    }


    private void OnColorChanged(bool red, bool green, bool blue)
    {
        // Проверка условий на основе цвета врага
        if ((!_isGreenEnemy && !_isBlueEnemy && !_isRedEnemy) ||
            (_isGreenEnemy && _isBlueEnemy && _isRedEnemy))
        {
            EnableEnemy(grayMaterial);
        }       
        else if (_isRedEnemy && red)
        {
            EnableEnemy(redMaterial);
        }
        else if (_isGreenEnemy && green)
        {
            EnableEnemy(greenMaterial);
        }
        else if (_isBlueEnemy && blue)
        {
            EnableEnemy(blueMaterial);
        }
        else
        {
            DisableEnemy();
        }
    }

    private void EnableEnemy(Material material)
    {
        foreach (var col in _colliders)
            col.enabled = true;

        foreach (var sr in _spriteRenderers)
        {
            sr.enabled = true;
            sr.material = material;
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
        ChangeColor changeColorScript = ChangeColor.instance;
        if (changeColorScript != null)
        {
            changeColorScript.enemyColors -= OnColorChanged;
        }
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
