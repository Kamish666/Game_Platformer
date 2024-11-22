using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSourcejSound;
    [SerializeField] private AudioClip _changeColor;
    [SerializeField] private AudioClip[] _died;
    [SerializeField] private AudioClip[] _jump;

    private int _jumpCount, _diedCount;


    private void Start()
    {
        ChangeColor changeColorScript = FindObjectOfType<ChangeColor>();
        if (changeColorScript != null)
        {
            changeColorScript.enemyColors += OnColorChanged;
        }
        else
        {
            Debug.LogWarning("ChangeColor script not found in the scene!");
        }


        PlayerMovement playerMovementScript = FindObjectOfType<PlayerMovement>();
        if (playerMovementScript != null)
        {
            playerMovementScript.jumpAct += OnJump;
        }
        else
        {
            Debug.LogWarning("PlayerMovement script not found in the scene!");
        }

        Health healthScript = FindObjectOfType<Health>();
        if (healthScript != null)
        {
            healthScript.OnDie += OnDied;
        }
        else
        {
            Debug.LogWarning("Health script not found in the scene!");
        }


        _jumpCount = _jump.Length;
        _diedCount = _died.Length;
    }
    private void OnColorChanged(bool green, bool red, bool blue)
    {
        _audioSourcejSound.PlayOneShot(_changeColor);
    }

    public void OnJump()
    {
        int i = Random.Range(0, _jumpCount);
        _audioSourcejSound.PlayOneShot(_jump[i]);
    }

    private void OnDied()
    {
        int i = Random.Range(0, _diedCount);
        _audioSourcejSound.PlayOneShot(_died[i]);
    }

}
