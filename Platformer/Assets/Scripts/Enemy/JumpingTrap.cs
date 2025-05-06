using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpingTrap : MonoBehaviour
{
    [SerializeField] private Animation _jamp;

    [GameEditorAnnotation] [SerializeField] private float _strenght;

    [GameEditorAnnotation][SerializeField] private float _scaleX = 1f;

    protected bool _isEditor = false;

    // Start is called before the first frame update
    void Start()
    {
        ChangeColor changeColor = ChangeColor.instance;
        if (changeColor == null)
            _isEditor = true;

        if (_isEditor == true)
            StartCoroutine(Editor());
    }

    protected virtual void OnValidate()
    {
        ApplyScale();
    }

    protected void ApplyScale()
    {
        transform.localScale = new Vector3(_scaleX, transform.localScale.y, transform.localScale.z);
    }

    IEnumerator Editor()
    {
        while (true)
        {
            ApplyScale();
            yield return new WaitForSeconds(0.1f);

        }
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
