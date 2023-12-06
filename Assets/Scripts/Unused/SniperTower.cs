using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperTower : Tower
{
    // Start is called before the first frame update
    void Start()
    {
        healthComponent.Initialize(5, 5);
    }

    // Update is called once per frame
    protected override void Update()
    {
       base.Update();
    }
}
