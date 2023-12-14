using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceOnPlane : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject objToPlace;
    void Start() {
        StartCoroutine(StartOnPlane());
    }

    public IEnumerator StartOnPlane() {
        yield return new WaitForSeconds(5);
        objToPlace.transform.position = SrayForGameObject.CheckforObject.GetGameObject(transform.position, Vector3.down, null).transform.position;
    }
}
