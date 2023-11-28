using System;
using UnityEngine;

public class Blueprint : MonoBehaviour {
    RaycastHit _hit;
    public Vector3 _movePoint;
    public GameObject _prefab;
    public bool _isPlaceable;
    [SerializeField] private Camera cam;
    BuildingSlot slot;

    void Start() {
        _movePoint = Input.mousePosition;
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out _hit, 500f))
            _movePoint = _hit.point;
    }
    // Update is called once per frame
    private void FixedUpdate() {
        if (Input.GetMouseButtonDown(0)) {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out _hit, 5000f)) {
                transform.position = _hit.point;
                if (_hit.collider is BoxCollider)
                {
                    slot = _hit.collider.gameObject.GetComponent<BuildingSlot>();
                    PlaceTower();
                }
            }
        }
    }

    public void PlaceTower() 
    {
        GameObject tower = Instantiate(_prefab, slot.transform.position, slot.transform.rotation);
        tower.transform.SetParent(slot.transform, true);
        slot.TowerBuild();
        Destroy(gameObject);
    }
}



