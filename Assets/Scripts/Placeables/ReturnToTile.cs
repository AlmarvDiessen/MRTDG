using System;
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
    [SerializeField]private GameObject currentTile;
    public LayerMask IgnoreMe;

    public Vector3 NewSlotPosition { get => newSlotPosition; set => newSlotPosition = value; }
    public Vector3 OriginalPosition { get => originalPosition; set => originalPosition = value; }
    public GameObject CurrentTile { get => currentTile; set => currentTile = value; }

    private void Start() {
        grabInteractable = GetComponent<XRGrabInteractable>();
        grabInteractable.selectExited.AddListener(OnRelease);
        grabInteractable.selectEntered.AddListener(OnGrab);

        shakeObject = GetComponent<ShakeObject>();
    }
    // O Set orginPos --> Grab --> Hold --> Release --> NewPos? ---> No --> return to original pos;
    // X place on new slot
    // X if(occupied?) --> return to orignal pos
    private void Update() {

    }

    private void OnRelease(SelectExitEventArgs args) {
        Hide();

        //check if a new tile is under it and is empty

        if (!IsInNewSlot()) {
            transform.position = OriginalPosition;

        }
        else {
            transform.position = NewSlotPosition;
        }

        if (shakeObject != null) {
            shakeObject.enabled = true;
        }

    }

    private void OnGrab(SelectEnterEventArgs args) {
        Show();
        Debug.Log("Grabbed");
        if (shakeObject != null) {
            shakeObject.enabled = false;
        }
    }


    private bool IsInNewSlot() {
        float distanceThreshold = 0.1f; // Adjust this threshold based on your needs
        NewSlotPosition = GetCellAtPosition(transform.position).transform.position;
        // Check if the distance to the new slot position is within the threshold
        float distance = Vector3.Distance(transform.position, NewSlotPosition);
        OriginalPosition = NewSlotPosition;
        shakeObject.OriginalPosition = NewSlotPosition;

        shakeObject.enabled = true;
        return distance < distanceThreshold;
    }

    public void Hide() {
        Camera.main.cullingMask &= ~(1 << LayerMask.NameToLayer("GridField"));
    }
    public void Show() {
        Camera.main.cullingMask |= 1 << LayerMask.NameToLayer("GridField");
    }

    private GameObject GetCellAtPosition(Vector3 position) {
        // Perform a raycast or any other method to find the cell at the specified position
        try {
            RaycastHit hit;
            Ray ray = new Ray(position + new Vector3(0, 0.1f, 0), Vector3.down);

            if (Physics.Raycast(ray, out hit, 10f, ~IgnoreMe)) {
                try {
                    Tile tile = hit.collider.gameObject.GetComponent<Tile>();
                    if (tile != null && !tile.isBlocked) {
                        CurrentTile = hit.collider.gameObject;
                        return hit.collider.gameObject;
                    }
                    else {
                        if(CurrentTile != null)
                            return CurrentTile;
                        else
                         return null;
                    }
                }
                catch (Exception e) {
                    Debug.LogError(e.Message);
                }
            }
            return currentTile;
        }
        catch (Exception e) {
            //debugText.text = e.ToString();
            return null;
        }
    }
}
