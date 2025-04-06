using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpingTrap : MonoBehaviour
{
    [SerializeField] private Animation _jamp;

    [GameEditorAnnotation] [SerializeField] private float _strenght;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var player = collision.gameObject.GetComponent<PlayerMovement>();
        if (player != null)
        {
            player.GetComponent<Rigidbody2D>().AddForce(transform.up * _strenght, ForceMode2D.Impulse);
        }
    }


}
