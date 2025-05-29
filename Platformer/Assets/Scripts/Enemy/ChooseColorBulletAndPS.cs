using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;

public enum ColorType
{
    Red,
    Green,
    Blue,
    RedGreen,
    RedBlue,
    GreenBlue,
    All
}


public class ChooseColorBulletAndPS : MonoBehaviour
{
//    [Header("Ñíàðÿäû")]
    [SerializeField] 
    private GameObject _redBullet, _greenBullet, _blueBullet,
        _redGreenBullet, _redBlueBullet, _greenBlueBullet, _allBullet;

/*    [Space(10)]
    [Header("Ñèñòåìà ÷àñòèö")]*/
    [SerializeField]
    private GameObject _redPS, _greenPS, _bluePS,
        _redGreenPS, _redBluePS, _greenBluePS, _allPS;

    private Dictionary<ColorType, GameObject> _colorPrefabsBullet, _colorPrefabsPS;

    private void Awake()
    {
        _colorPrefabsBullet = new Dictionary<ColorType, GameObject>
        {
            { ColorType.Red, _redBullet },
            { ColorType.Green, _greenBullet },
            { ColorType.Blue, _blueBullet },
            { ColorType.RedGreen, _redGreenBullet },
            { ColorType.RedBlue, _redBlueBullet },
            { ColorType.GreenBlue, _greenBlueBullet },
            { ColorType.All, _allBullet }
        };

        _colorPrefabsPS = new Dictionary<ColorType, GameObject>
        {
            { ColorType.Red, _redPS },
            { ColorType.Green, _greenPS },
            { ColorType.Blue, _bluePS },
            { ColorType.RedGreen, _redGreenPS },
            { ColorType.RedBlue, _redBluePS },
            { ColorType.GreenBlue, _greenBluePS },
            { ColorType.All, _allPS }
        };
    }

    private void Start()
    {
        var player = ChangeColor.instance;
        if (player == null)
        {
            ChangePaletteColor(ColorType.All);
            Debug.Log("ChooseColorBullet ÐÅÄÀÊÒÎÂ");
        }
        else
        {
            GetColors();
            Debug.Log("ChooseColorBullet ÈÃÐÀ");
        }
        Debug.Log("ChooseColorBullet");
    }

    private void GetColors()
    {
        var enemyColors = GetComponent<EnemyColors>();

        bool red = enemyColors.IsRedEnemy;
        bool green = enemyColors.IsGreenEnemy;
        bool blue = enemyColors.IsBlueEnemy;

        ColorType colorType;

        if (red && green && blue)
            colorType = ColorType.All;
        else if (red)
            colorType = green ? ColorType.RedGreen : (blue ? ColorType.RedBlue : ColorType.Red);
        else if (green)
            colorType = blue ? ColorType.GreenBlue : ColorType.Green;
        else if (blue)
            colorType = ColorType.Blue;
        else
            colorType = ColorType.All;

        ChangePaletteColor(colorType);
    }

    private void ChangePaletteColor(ColorType colorType)
    {
        var bulletPooler = PoolerBulletsAndPS.instance;
        var shooter = GetComponent<IShot>();

        float fireRate = shooter.FireRate;
        int amountBullet = (int)(10 / fireRate + (10 / fireRate == 0 ? 0 : 1));

        var prefabBullet = _colorPrefabsBullet[colorType];
        var prefabPS = _colorPrefabsPS[colorType];

        bulletPooler.Preload(prefabBullet, amountBullet);
        bulletPooler.Preload(prefabPS, Mathf.CeilToInt(amountBullet * 0.75f));

        shooter.ProjectileTag = prefabBullet.name;
    }
}
