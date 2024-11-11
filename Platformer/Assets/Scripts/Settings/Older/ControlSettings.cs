using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ControlSettings
{
    public KeyCode moveLeft = KeyCode.A;
    public KeyCode moveRight = KeyCode.D;
    public KeyCode jump = KeyCode.Space;
    public KeyCode changeColorLeft = KeyCode.Mouse0; // левая кнопка мыши
    public KeyCode changeColorRight = KeyCode.Mouse1; // правая кнопка мыши
}
