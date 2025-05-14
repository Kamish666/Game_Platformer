using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    [SerializeField] private List<BackgroundLayer> _layers;

    private void Start()
    {
        foreach (var layer in _layers)
        {
            if (layer.background == null) continue;

            var renderer = layer.background.GetComponent<SpriteRenderer>();
            if (renderer != null)
            {
                layer.startPosX = layer.background.transform.position.x;
                layer.startPosY = layer.background.transform.position.y;
                layer.lengthX = renderer.bounds.size.x;
                layer.lengthY = renderer.bounds.size.y;
            }
        }
    }

    private void FixedUpdate()
    {
        foreach (var layer in _layers)
        {
            if (layer.background == null) continue;

            float tempX = transform.position.x * (1 - layer.parallaxEffectX);
            float distX = transform.position.x * layer.parallaxEffectX;

            float tempY = transform.position.y * (1 - layer.parallaxEffectY);
            float distY = transform.position.y * layer.parallaxEffectY;

            layer.background.transform.position = new Vector3(
                layer.startPosX + distX,
                layer.startPosY + distY,
                layer.background.transform.position.z
            );

            if (tempX > layer.startPosX + layer.lengthX)
                layer.startPosX += layer.lengthX;
            else if (tempX < layer.startPosX - layer.lengthX)
                layer.startPosX -= layer.lengthX;

            if (tempY > layer.startPosY + layer.lengthY)
                layer.startPosY += layer.lengthY;
            else if (tempY < layer.startPosY - layer.lengthY)
                layer.startPosY -= layer.lengthY;
        }
    }
}



[System.Serializable]
public class BackgroundLayer
{
    public GameObject background;

    [Range(0, 1)] public float parallaxEffectX = 0.5f;
    [Range(0, 1)] public float parallaxEffectY = 0.5f;

    [HideInInspector] public float startPosX, startPosY;
    [HideInInspector] public float lengthX, lengthY;
}
