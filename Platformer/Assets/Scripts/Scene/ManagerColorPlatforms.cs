/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerColorPlatforms : MonoBehaviour
{
    [ContextMenu("PlatformManager")]
    private void PlatformManager()
    {
        // Создание или поиск родительских объектов
        GameObject greenBlocks = GameObject.Find("GreenBlocks") ?? new GameObject("GreenBlocks");
        GameObject redBlocks = GameObject.Find("RedBlocks") ?? new GameObject("RedBlocks");
        GameObject blueBlocks = GameObject.Find("BlueBlocks") ?? new GameObject("BlueBlocks");

        SpriteRenderer[] temp = FindObjectsOfType<SpriteRenderer>();

        for (int i = 0; i < temp.Length; i++)
        {
            string str = temp[i].sprite.texture.name;
            switch (str)
            {
                case "Green":
                    {
                        temp[i].transform.SetParent(greenBlocks.transform);
                        temp[i].gameObject.layer = 8;
                        break;
                    }
                case "Red":
                    {
                        temp[i].transform.SetParent(redBlocks.transform);
                        temp[i].gameObject.layer = 8;
                        break;
                    }
                case "Blue":
                    {
                        temp[i].transform.SetParent(blueBlocks.transform);
                        temp[i].gameObject.layer = 8;
                        break;
                    }
            }
        }
    }
}*/


using UnityEngine;
using System.Collections.Generic;

public class ManagerColorPlatforms : MonoBehaviour
{
    [ContextMenu("PlatformManager")]
    private void PlatformManager()
    {
        // Создание или поиск родительских объектов
        Dictionary<string, GameObject> colorBlocks = new Dictionary<string, GameObject>
        {
            { "Green", GameObject.Find("GreenBlocks") ?? new GameObject("GreenBlocks") },
            { "Red", GameObject.Find("RedBlocks") ?? new GameObject("RedBlocks") },
            { "Blue", GameObject.Find("BlueBlocks") ?? new GameObject("BlueBlocks") }
        };

        SpriteRenderer[] spriteRenderers = FindObjectsOfType<SpriteRenderer>();

        foreach (var spriteRenderer in spriteRenderers)
        {
            if (spriteRenderer.sprite != null && spriteRenderer.sprite.texture != null)
            {
                string textureName = spriteRenderer.sprite.texture.name;

                if (colorBlocks.ContainsKey(textureName))
                {
                    spriteRenderer.transform.SetParent(colorBlocks[textureName].transform);
                    spriteRenderer.gameObject.layer = 8;
                    
                    // Проверка наличия BoxCollider2D и добавление, если его нет
                    BoxCollider2D boxCollider = spriteRenderer.GetComponent<BoxCollider2D>();
                    if (boxCollider == null)
                    {
                        boxCollider = spriteRenderer.gameObject.AddComponent<BoxCollider2D>();
                    }
                }
            }
        }
    }
}
