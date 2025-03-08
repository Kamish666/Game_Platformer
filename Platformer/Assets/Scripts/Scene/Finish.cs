using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Finish : MonoBehaviour
{
    private SceneController _sceneController;
    private void Start()
    {
        _sceneController = FindObjectOfType<SceneController>();

        if (gameObject.GetComponent<BoxCollider2D>() != null)
        {
            gameObject.AddComponent<BoxCollider2D>().isTrigger = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerMovement>() != null)
        {
            _sceneController.LoadNextScene();
        }
    }
}
