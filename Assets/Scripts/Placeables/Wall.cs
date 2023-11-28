using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Wall : Entity
{
    public LayerMask IgnoreMe;

    protected override void Start() {
        base.Start();
        //TODO set dynamic version for 
        healthComponent.Initialize(10,10);
    }

    protected override void HandleHealthChange(int currentHealth, int maxHealth) {
        base.HandleHealthChange(currentHealth, maxHealth);
    }

    protected override void Die() {
        Destroy(gameObject);
    }
}
