using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletParticleSystem : MonoBehaviour
{
    private ParticleSystem ps;

    void Awake()
    {
        ps = GetComponent<ParticleSystem>();
    }

    void OnEnable()
    {
        ps.Play();
    }

    void Update()
    {
        if (!ps.IsAlive())
        {
            gameObject.SetActive(false);
        }
    }
}
