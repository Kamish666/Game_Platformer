using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;

public class ChooseColorBullet : MonoBehaviour
{
    [SerializeField] private string _red, _green, _blue, _redGreen, _redBlue, _greenBlue, _all;

    private void Start()
    {
        var player = PlayerMovementForceMode.instance;
        if (player == null)
            ChangePaletteColor(_all);
        else
            GetColors();
    }

    private void GetColors()
    {
        var enemyColors = gameObject.GetComponent<EnemyColors>();

        bool red = enemyColors.IsRedEnemy;
        bool green = enemyColors.IsGreenEnemy;
        bool blue = enemyColors.IsBlueEnemy;

        string tag;

        if (red && green && blue)
        {
            tag = _all;
        }
        else if (red)
        {
            tag = _red;
            if (green)
                tag = _redGreen;
            else if (blue)
                tag = _redBlue;
        }
        else if (green)
        {
            tag = _green;
            if (blue)
                tag = _greenBlue;
        }
        else if (blue)
            tag = _blue;
        else
            tag = _all;


        ChangePaletteColor(tag);
    }

    private void ChangePaletteColor(string tag)
    {
        var bulletPooler = BulletPooler.instance;
        var shooter = gameObject.GetComponent<IShot>();

        float fireRate = shooter.FireRate;

        int amountBullet = Mathf.RoundToInt(10/fireRate);

        bulletPooler.FindBulletTag(tag, amountBullet);
        shooter.ProjectileTag = tag;
    }
}
