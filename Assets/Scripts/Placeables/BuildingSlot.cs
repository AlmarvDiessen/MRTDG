using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingSlot : MonoBehaviour {
    [SerializeField] protected bool _isFilled = false;

    public bool IsFilled {
        get { return _isFilled; }
        set { _isFilled = TowerBuild(); }
    }

    public bool TowerBuild() {
        _isFilled = true;
        return _isFilled;
    }

    public void Reset()
    {
        _isFilled = false;
    }
}
