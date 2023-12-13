using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGameDebugger : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Text infoText;
    [SerializeField] private Transform Transform;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        infoText.text = Transform.position.ToString();
    }
}
