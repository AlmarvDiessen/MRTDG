using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GizmoTest : MonoBehaviour
{
    void OnDrawGizmos() {
        // Draws a 5 unit long red line in front of the object
        Gizmos.color = Color.red;
        Vector3 direction = transform.TransformDirection(Vector3.forward) * 5000;
        Gizmos.DrawRay(transform.position, direction);
    }
}
