using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.UI;

public class Debugger : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] private Text pointsText;
    [SerializeField] private XROrigin origin;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        pointsText.text = origin.RequestedTrackingOriginMode.ToString();
    }
}
