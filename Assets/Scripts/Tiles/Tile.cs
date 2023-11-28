using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    private Collider collider;
    public bool isBlocked;
    public Collider Collider { get => collider; set => collider = value; }
    public bool IsBlocked { get => isBlocked; set => isBlocked = value; }

    private void Start() {
        Collider = GetComponent<Collider>();
    }
}
