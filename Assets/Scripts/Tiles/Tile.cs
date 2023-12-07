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

        if (other != null) {
            XRGrabInteractable interactable = other.GetComponent<XRGrabInteractable>();

            if (interactable != null) {
                // Check if the slot is already occupied by an interactable object
                XRGrabInteractable existingInteractable = other.GetComponent<XRGrabInteractable>();

                if (IsBlocked) {
                    // If the slot is occupied, return the grabbed object to its old position
                    ReturnObjectToOriginalPosition(existingInteractable);
                }
                else {
                    // If the slot is empty, proceed to move the object to the new slot
                    MoveObjectToNewSlot(interactable);
                }
            }
        }
    }

    private void ReturnObjectToOriginalPosition(XRGrabInteractable interactable) {
        // Move the grabbed object back to its original position
        interactable.transform.position = interactable.GetComponent<ReturnToTile>().OriginalPosition;
        interactable.interactionManager.SelectExit(interactable.interactorsSelecting.FirstOrDefault(), interactable);
    }

    private void MoveObjectToNewSlot(XRGrabInteractable interactable) {
        // Move the object to the new slot
        Debug.Log(gameObject.name + "new slot");
        interactable.transform.position = gameObject.transform.position;
        interactable.interactionManager.SelectExit(interactable.interactorsSelecting.FirstOrDefault(), interactable);
        IsBlocked = true;
    }


    private void OnTriggerExit(Collider other) {
        IsBlocked = false;
        rb = null;
    }

}
