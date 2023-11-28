using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileRemover : MonoBehaviour
{
    private GameObject enteredGameObject;

    private void OnTriggerEnter(Collider other)
    {
        enteredGameObject = other.gameObject;
        Debug.Log(other.gameObject.name);
        Destroy(enteredGameObject);
    }
}