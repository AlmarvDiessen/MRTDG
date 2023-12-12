using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.XR.Interaction.Toolkit;

public class Tile : MonoBehaviour
{
    private Collider collider;
    [SerializeField] private Rigidbody rb;
    public bool isBlocked;
    public Collider Collider { get => collider; set => collider = value; }
    public bool IsBlocked { get => isBlocked; set => isBlocked = value; }

    private void Start() {
        Collider = GetComponent<Collider>();

    }

    private void OnTriggerEnter(Collider other) {
       isBlocked = true;
    }


    private void OnTriggerStay(Collider other) {
        isBlocked = true;
    }
    private void OnTriggerExit(Collider other) {
        rb = null;
        IsBlocked = false;
    }

}
