using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Tile : MonoBehaviour
{
    private Collider collider;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private BoxCollider trigger;
    public bool isBlocked;
    public Collider Collider { get => collider; set => collider = value; }
    public bool IsBlocked { get => isBlocked; set => isBlocked = value; }

    private void Start() {
        Collider = GetComponent<Collider>();

    }

    private void OnTriggerEnter(Collider other) {
        Debug.Log("Entered");
        IsBlocked = true;

        if (other != null) {
            rb = other.GetComponent<Rigidbody>();
            ReturnToTile returnToTile = other.GetComponent<ReturnToTile>();
            returnToTile.NewSlotPosition = transform.position;
            if (other.GetComponent<Tower>() != null) {
                XRGrabInteractable interactable = other.GetComponent<XRGrabInteractable>();

                // Check if the object is being grabbed
                if (interactable.isSelected) {
                    // If grabbed, release it from the old slot
                    XRBaseInteractor interactor = interactable.selectingInteractor;

                    // Simulate select exit from the old slot using the interaction manager
                    interactable.interactionManager.SelectExit(interactable.interactorsSelecting.First(), interactable);
                }

                // Move the object to the new slot
                other.gameObject.transform.position = transform.position;
            }
        }
    }


    private void OnTriggerExit(Collider other) {
        IsBlocked = false;
        rb = null;
    }

}
