using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ReturnToTile : MonoBehaviour
{
    private XRGrabInteractable grabInteractable;
    private Vector3 originalPosition;
    private Vector3 newSlotPosition; // Assuming you have this defined somewhere
    private ShakeObject shakeObject;
    public Vector3 NewSlotPosition { get => newSlotPosition; set => newSlotPosition = value; }
    public Vector3 OriginalPosition { get => originalPosition; set => originalPosition = value; }

    private void Start() {
        grabInteractable = GetComponent<XRGrabInteractable>();
        OriginalPosition = transform.position;
        grabInteractable.selectExited.AddListener(OnRelease);
        grabInteractable.selectEntered.AddListener(OnGrab);

        shakeObject = GetComponent<ShakeObject>();
    }



    private void OnRelease(SelectExitEventArgs args) {
        if (!IsInNewSlot()) {
            transform.position = OriginalPosition;
        }

        if(shakeObject != null) {
            shakeObject.enabled = true;
        }

    }

    private void OnGrab(SelectEnterEventArgs args) {
        if(shakeObject != null) {
            shakeObject.enabled = false;
        }
    }

    private bool IsInNewSlot() {
        float distanceThreshold = 0.1f; // Adjust this threshold based on your needs

        // Check if the distance to the new slot position is within the threshold
        float distance = Vector3.Distance(transform.position, NewSlotPosition);
        originalPosition = newSlotPosition;
        shakeObject.enabled = true;
        return distance < distanceThreshold;
    }


    private bool IsBeingHeld() {
        // Check if the object is currently being grabbed
        return grabInteractable.isSelected;
    }
}
