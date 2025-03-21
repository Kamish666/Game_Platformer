using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthEnemy : Health
{
    public override void HandleDeath()
    {
        base.HandleDeath();
        Destroy(gameObject);
    }
}
