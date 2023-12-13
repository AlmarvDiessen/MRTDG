using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;

public class Recenter : MonoBehaviour
{
    [SerializeField] private Transform target;


    void Start() {
        XROrigin xROrigin = GetComponent<XROrigin>();
        xROrigin.MoveCameraToWorldLocation(target.position);
        xROrigin.MatchOriginUpCameraForward(target.up, target.forward);
        //xROrigin.MoveCameraToWorldLocation(target.position);
    }

    
}
