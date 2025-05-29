using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZoneManager : MonoBehaviour
{
    [SerializeField] private PlayerDeathZone[] _zones;

    private Health _health;

    private void Awake()
    {
        _health = GetComponentInParent<Health>();
        foreach (var zone in _zones)
        {
            zone.Init(this);
        }
    }

    public void CheckZones()
    {
        foreach (var zone in _zones)
        {
            if (!zone.IsInside)
                return; // хот€ бы одна не внутри Ч не умираем
        }

        _health?.HandleDeath(); // все 4 зоны внутри Ч смерть
    }
}
