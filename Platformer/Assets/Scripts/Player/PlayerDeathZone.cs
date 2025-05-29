using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeathZone : MonoBehaviour
{
    [SerializeField] private LayerMask _collisionMask;
    private bool _isInside;
    private DeathZoneManager _manager;

    public bool IsInside => _isInside;

    public void Init(DeathZoneManager manager)
    {
        _manager = manager;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & _collisionMask) != 0)
        {
            _isInside = true;
            _manager?.CheckZones();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & _collisionMask) != 0)
        {
            _isInside = false;
            _manager?.CheckZones();
        }
    }
}
