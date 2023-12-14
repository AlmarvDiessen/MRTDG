using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SrayForGameObject : MonoBehaviour
{
    private static SrayForGameObject instance;

    public static SrayForGameObject CheckforObject
    {
        get
        {
            // If the instance doesn't exist, find it or create it
            if (instance == null) {
                instance = FindObjectOfType<SrayForGameObject>();

                // If there's still no instance in the scene, create a new GameObject and add the class to it
                if (instance == null) {
                    GameObject singletonObject = new GameObject(typeof(SrayForGameObject).Name);
                    instance = singletonObject.AddComponent<SrayForGameObject>();
                }
            }

            return instance;
        }
    }

    private void Awake() {
        // Ensure there's only one instance, and persist it between scenes
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log($"Singleton instance created: {typeof(SrayForGameObject).Name}");

        }
        else {
            // If an instance already exists, destroy this one
            Debug.Log($"Duplicate instance of {typeof(SrayForGameObject).Name} found. Destroying this instance.");

            Destroy(gameObject);
        }
    }
    /// <summary>
    /// Funtion to perform a check for a gameobject 
    /// </summary>
    /// <param name="position">Where to peform the raycast</param>
    /// <param name="direction"> which direction</param>
    /// <param name="ignoreLayer"> which layer to Ignore</param>
    /// <returns>A GameObject that have colliders</returns>
    public GameObject GetGameObject(Vector3 position, Vector3 direction, LayerMask ignoreLayer) {
        try {
            RaycastHit hit;
            Ray ray = new Ray(position + new Vector3(0, 0.1f, 0), direction);

            if (Physics.Raycast(ray, out hit, 10f, ~ignoreLayer)) {
                try {
                    if (hit.collider.gameObject != null)
                        return hit.collider.gameObject;
                    else
                        return null;
                }
                catch (Exception e) {
                    Debug.LogError(e.Message);
                }
            }
            return null;
        }
        catch (Exception e) {
            Debug.LogError(e.Message + "\n" + e.StackTrace);
            return null;
        }
    }

    private void OnDestroy() {
        Debug.Log($"Singleton instance of {typeof(SrayForGameObject).Name} destroyed.");
    }

}
