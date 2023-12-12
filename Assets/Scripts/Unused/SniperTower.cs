using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperTower : Tower
{
    // Start is called before the first frame update
    protected override void Start() {
        base.Start();
        healthComponent.Initialize(4, 4);
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }
}
