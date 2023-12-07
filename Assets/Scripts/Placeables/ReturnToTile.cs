using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ReturnToTile : MonoBehaviour
{
    private XRGrabInteractable grabInteractable;
    public Vector3 originalPosition;
    public Vector3 newSlotPosition; // Assuming you have this defined somewhere
    private ShakeObject shakeObject;
    public Vector3 NewSlotPosition { get => newSlotPosition; set => newSlotPosition = value; }
    public Vector3 OriginalPosition { get => originalPosition; set => originalPosition = value; }

    private void Start() {
        grabInteractable = GetComponent<XRGrabInteractable>();
        grabInteractable.selectExited.AddListener(OnRelease);
        grabInteractable.selectEntered.AddListener(OnGrab);

        shakeObject = GetComponent<ShakeObject>();
        OriginalPosition = transform.position;
    }



    private void OnRelease(SelectExitEventArgs args) {
        Hide();
        if (!IsInNewSlot()) {
            transform.position = OriginalPosition;

        }

        if (shakeObject != null) {
            shakeObject.enabled = true;
        }

    }

    private void OnGrab(SelectEnterEventArgs args) {
        Show();
        if (shakeObject != null) {
            shakeObject.enabled = false;
        }
    }


    private bool IsInNewSlot() {
        float distanceThreshold = 0.1f; // Adjust this threshold based on your needs

        // Check if the distance to the new slot position is within the threshold
        float distance = Vector3.Distance(transform.position, NewSlotPosition);
        shakeObject.OriginalPosition = NewSlotPosition;

        shakeObject.enabled = true;
        return distance < distanceThreshold;
    }


    private bool IsBeingHeld() {
        // Check if the object is currently being grabbed
        return grabInteractable.isSelected;
    }
    public void Hide() {
        Camera.main.cullingMask &= ~(1 << LayerMask.NameToLayer("GridField"));
    }
    public void Show() {
        Camera.main.cullingMask |= 1 << LayerMask.NameToLayer("GridField");
    }
}
