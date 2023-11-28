using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;
//using UnityEngine.XR.Interaction.Toolkit;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Camera cam;

    private PointManager pointManager;
    private CircleSpawner spawner;

    [SerializeField] private XRRayInteractor leftHand;
    [SerializeField] private XRRayInteractor rightHand;

    [SerializeField] private GameObject _prefab;
    [SerializeField] private GameObject _prefabBp;
    [SerializeField] private GameObject _prefabWall;
    [SerializeField] private GameObject _prefabBpWall;

    //[SerializeField] private GameObject currentCell;
    //[SerializeField] private GameObject previousCell;

    //[SerializeField] private Material previewMat;
    //[SerializeField] private Material normalMat;
    //[SerializeField] private Material blockedMat;



    //[SerializeField] private Text debugText;
    private void Awake()
    {
        pointManager = gameObject.GetComponent<PointManager>();
        spawner = gameObject.GetComponent<CircleSpawner>();
    }
    void Start()
    {
        Hide();
    }

    private void Update()
    {
        #region For using on desktop
        //if (isBuilding) {
        //    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //    RaycastHit hit;
        //    if (Physics.Raycast(ray, out hit) && CheckTileSelection() != null) {
        //        Vector3 offset = new Vector3(0, 0.17f, 0);
        //        float rayLength = float.MaxValue;
        //        Debug.DrawRay(ray.origin, ray.direction * rayLength, Color.black);
        //        _prefabBp.SetActive(true);
        //        Vector3 intersection = hit.collider.transform.position;
        //        _prefabBp.transform.position = intersection + offset;
        //        ColorSurroundingCells(CheckTileSelection());

        //        if (Input.GetMouseButtonDown(0)) {
        //            _prefabBp.SetActive(false);
        //            if (CheckTileSelection() != null) {
        //                //place the tower in the top 
        //                if (!pointManager.DeductPoinstIfSufficient(50)) {
        //                    RevertState(adjecentcells);
        //                    EndBuilding();
        //                    return;
        //                }

        //                GameObject clone = Instantiate(_prefab, CheckTileSelection().transform.position, Quaternion.identity);
        //                DisableSurroundingCells(outerCells, normalMat);
        //                DisableSurroundingCells(adjecentcells, blockedMat, false, true);
        //                EndBuilding();
        //            }
        //        }

        //    }
        //}
        #endregion

        if (isBuilding)
        {
            Show();
            GameObject selectedTile;
            bool tileSelected = CheckTileSelection(out selectedTile);
            if (selectedTile != null)
            {
                Vector3 offset = new Vector3(0, 0.17f, 0);
                XRSimpleInteractable interactable = selectedTile.GetComponent<XRSimpleInteractable>();
                _prefabBp.SetActive(true);
                Vector3 intersection = selectedTile.transform.position + offset;
                _prefabBp.transform.position = intersection;
                _prefabBp.transform.rotation = selectedTile.transform.rotation;
                //ColorSurroundingCells(selectedTile);

                //text = tileSelected.ToString() + " Tile name: " + selectedTile.name;
                if (tileSelected && interactable.isSelected || Input.GetMouseButtonDown(0))
                {
                    _prefabBp.SetActive(false);
                    if (!pointManager.DeductPoinstIfSufficient(50))
                    {
                        //RevertState(adjecentcells);
                        EndBuilding();
                        return;
                    }
                    //TODO: future implementation make turret always look down the lane.
                    //Quaternion forwards = Quaternion.LookRotation(selectedTile.transform.position - cam.transform.position, Vector3.up );
                    GameObject clone = Instantiate(_prefab, selectedTile.transform.position, selectedTile.transform.rotation);
                    //DisableCell(selectedTile);
                    //debugText.text = selectedTile.name;
                    // DisableSurroundingCells(outerCells, normalMat);
                    // DisableSurroundingCells(adjecentcells, blockedMat, false, true);
                    EndBuilding();
                }
            }
        }
    }

    //Button fuction
    private bool isBuilding = false;
    public void StartBulding()
    {
        Show();
        isBuilding = true;
    }

    //editor version
    private GameObject CheckTileSelection()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            Tile tileComponent = hit.collider.GetComponent<Tile>();

            if (tileComponent != null && tileComponent.enabled == true)
            {
                return hit.collider.gameObject;
            }
            else
            {
                Debug.Log("Selected a tile outside of the range.");
                return null; // Return null to indicate tile selection outside of the range
            }
        }
        else
        {
            Debug.Log("Clicked on something other than a tile:" + hit.collider.name);
            return null; // Return null for non-tile objects
        }
    }

    /// <summary>
    /// The A3 version of checking if we selected an tile that returns the gameobject from the raycast from the hand
    /// </summary>
    /// <param name="selectedTile"></param>
    /// <returns></returns>
    /// 
    private bool CheckTileSelection(out GameObject selectedTile)
    {
        selectedTile = null;
        Tile tileComponent = null;

        RaycastHit rightRaycastHit;
        RaycastHit leftRaycastHit;

        try
        {
            if (rightHand.TryGetCurrent3DRaycastHit(out rightRaycastHit))
            {
                try
                {
                    tileComponent = rightRaycastHit.collider.GetComponent<Tile>();
                    //debugText.text = rightRaycastHit.Value.collider.name + tileComponent.name;

                }
                catch (Exception ex)
                {
                    //debugText.text += ex.Message;
                }

                if (tileComponent != null && tileComponent.enabled == true)
                {
                    Debug.Log("Selected a tile within the range.");
                    selectedTile = tileComponent.gameObject;
                    return true; // Return true for tile selection within the range
                }
                else
                {
                    Debug.Log("Selected a tile outside of the range.");
                    return false; // Return false to indicate tile selection outside of the range
                }

            }
            else if (leftHand.TryGetCurrent3DRaycastHit(out leftRaycastHit))
            {
                try
                {
                    tileComponent = leftRaycastHit.collider.GetComponent<Tile>();
                    //debugText.text = rightRaycastHit.Value.collider.name + tileComponent.name;
                }
                catch (Exception ex)
                {
                    //debugText.text += ex.Message;
                }

                if (tileComponent != null && tileComponent.enabled == true)
                {
                    Debug.Log("Selected a tile within the range.");
                    selectedTile = tileComponent.gameObject;
                    return true; // Return true for tile selection within the range
                }
                else
                {
                    Debug.Log("Selected a tile outside of the range.");
                    return false; // Return false to indicate tile selection outside of the range
                }
            }
            else
                return false;

        }
        catch (Exception ex)
        {
            //debugText.text += ex.Message;
            Debug.Log(ex.Message);
            return false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(rightHand.transform.position, rightHand.transform.forward * 5f);
    }


    private void DisableCell(GameObject cell)
    {
        cell.GetComponent<Tile>().enabled = false;
    }

    private void EnableCell(GameObject cell)
    {
        cell.GetComponent<Tile>().enabled = enabled;
    }

    public void Hide()
    {
        cam.cullingMask &= ~(1 << LayerMask.NameToLayer("GridField"));
    }
    public void Show()
    {
        cam.cullingMask |= 1 << LayerMask.NameToLayer("GridField");
    }

    public void Quit()
    {
        Application.Quit();
    }
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void EndBuilding()
    {
        isBuilding = false;
        Hide();
    }
}
    #region
    //public List<GameObject> adjecentcells = new List<GameObject>();
    //public List<GameObject> outerCells = new List<GameObject>();
    //private void ColorSurroundingCells(GameObject selectedCell) {
    //    try {

    //        if (selectedCell != null) {
    //            // Get the position of the selected cell
    //            Vector3 selectedPosition = selectedCell.transform.position;

    //            // Get the size of the selected cell
    //            Vector3 cellScale = selectedCell.transform.localScale;

    //            // Define the relative positions of the surrounding cells based on cell size
    //            Vector3[] offsets = new Vector3[]
    //            {
    //            Vector3.zero, Vector3.zero,                        //Middle
    //            new Vector3(cellScale.x + 0.15f, 0, 0),          // Right
    //            new Vector3(-(cellScale.x + 0.15f), 0, 0),         // Left
    //            new Vector3(0, 0, cellScale.z + 0.15f),          // Up
    //            new Vector3(0, 0, -(cellScale.z + 0.15f)),         // Down
    //            new Vector3(cellScale.x + 0.15f, 0, cellScale.z + 0.15f), // Diagonal Up-Right
    //            new Vector3(-(cellScale.x + 0.15f), 0, cellScale.z + 0.15f),// Diagonal Up-Left
    //            new Vector3(cellScale.x + 0.15f, 0, -(cellScale.z + 0.15f)),// Diagonal Down-Right
    //            new Vector3(-(cellScale.x + 0.15f), 0, -(cellScale.z + 0.15f))// Diagonal Down-Left
    //            };


    //            RevertState(adjecentcells); //clear new

    //            // Color the surrounding cells
    //            foreach (Vector3 offset in Calc3x3(cellScale, 3)) {
    //                Vector3 position = selectedPosition + offset;
    //                GameObject adjacentCell = GetCellAtPosition(position);
    //                if (adjacentCell != null && !adjecentcells.Contains(adjacentCell)) {
    //                    adjecentcells.Add(adjacentCell); //add new
    //                   // ColorCell(adjacentCell, previewMat, false);
    //                }
    //            }

    //            Vector3[] offsets5x5 = Calc5x5(cellScale);

    //            foreach (Vector3 offset in offsets5x5) {
    //                Vector3 pos = selectedPosition + offset;
    //                GameObject adjacentCell = GetCellAtPosition(pos);
    //                if (adjacentCell != null && !outerCells.Contains(adjacentCell)) {
    //                    outerCells.Add(adjacentCell);
    //                }
    //            }

    //        }
    //        else {
    //            //debugText.text = "selected cell is empty";
    //        }
    //    }
    //    catch (Exception e) {
    //        //debugText.text = e.Message;
    //    }
    //}

    ////Optimizable
    ////calculate a 5x5 grid
    //private Vector3[] Calc3x3(Vector3 cellScale, int gridSize) {
    //    float offset = cellScale.x + 0.15f;
    //    List<Vector3> vectors = new List<Vector3>();

    //    for (int i = 0; i < gridSize; i++) {
    //        for (int j = 0; j < gridSize; j++) {
    //            float x = i * offset - (gridSize - 1) * offset * 0.5f;
    //            float z = j * offset - (gridSize - 1) * offset * 0.5f;

    //            vectors.Add(new Vector3(x, 0, z));
    //        }
    //    }
    //    return vectors.ToArray();
    //}
    //private Vector3[] Calc5x5(Vector3 cellScale) {
    //    List<Vector3> vectors = new List<Vector3>();
    //    float offset = cellScale.x = 0.15f;
    //    for (int i = -1; i <= 1; i += 2) {
    //        for (int j = -1; j <= 1; j += 2) {
    //            // Right, Left, Up, Down
    //            vectors.Add(new Vector3(2 * i * (cellScale.x + 0.15f), 0, 0));
    //            vectors.Add(new Vector3(0, 0, 2 * j * (cellScale.z + 0.15f)));

    //            // Diagonals
    //            vectors.Add(new Vector3(2 * i * (cellScale.x + 0.15f), 0, j * (cellScale.z + 0.15f)));
    //            vectors.Add(new Vector3(i * (cellScale.x + 0.15f), 0, 2 * j * (cellScale.z + 0.15f)));

    //            // Additional Diagonals
    //            vectors.Add(new Vector3(2 * i * (cellScale.x + 0.15f), 0, 2 * j * (cellScale.z + 0.15f)));
    //            vectors.Add(new Vector3(2 * i * (cellScale.x + 0.15f), 0, -2 * j * (cellScale.z + 0.15f)));
    //            vectors.Add(new Vector3(-2 * i * (cellScale.x + 0.15f), 0, 2 * j * (cellScale.z + 0.15f)));
    //            vectors.Add(new Vector3(-2 * i * (cellScale.x + 0.15f), 0, -2 * j * (cellScale.z + 0.15f)));
    //        }
    //    }
    //    return vectors.ToArray();


    //}

    //private GameObject GetCellAtPosition(Vector3 position) {
    //    // Perform a raycast or any other method to find the cell at the specified position
    //    try {

    //        RaycastHit hit;
    //        Ray ray = new Ray(position + new Vector3(0, 10, 0), Vector3.down);

    //        if (Physics.Raycast(ray, out hit)) {
    //            try {
    //                if (hit.collider.gameObject.GetComponent<Tile>() != null)
    //                    return hit.collider.gameObject;
    //                else
    //                    return null;
    //            }
    //            catch (Exception e) {
    //                Debug.LogError(e.Message);
    //            }
    //        }

    //        return null;
    //    }
    //    catch (Exception e) {
    //        //debugText.text = e.ToString();
    //        return null;
    //    }
    //}


    //private void DisableSurroundingCells(List<GameObject> cells, Material material) {
    //    if (cells != null) {
    //        foreach (GameObject cell in cells) {
    //            ColorCell(cell, material);
    //            cell.GetComponentInChildren<Tile>().enabled = false;
    //        }
    //        outerCells.Clear();
    //    }
    //}
    //private void DisableSurroundingCells(List<GameObject> cells, Material mat, bool skip, bool blocked) {
    //    if (cells != null) {
    //        foreach (GameObject cell in cells) {
    //            cell.GetComponentInChildren<Tile>().enabled = false;
    //            cell.GetComponentInChildren<Tile>().IsBlocked = blocked;

    //            ColorCell(cell, mat, skip);
    //        }
    //        adjecentcells.Clear();
    //    }
    //}

    //private void ColorCell(GameObject cell, Material mat) {
    //    if (cell != null) {
    //        // Change the color of the cell
    //        Renderer renderer = cell.GetComponentInChildren<Renderer>();
    //        if (cell.GetComponent<Tile>().enabled != false) {
    //            renderer.material = mat;
    //        }
    //    }
    //}

    //private void ColorCell(GameObject cell, Material mat, bool skip) {
    //    if (cell != null) {
    //        // Change the color of the cell
    //        Renderer renderer = cell.GetComponentInChildren<Renderer>();
    //        if ((skip == false || cell.GetComponent<Tile>().IsBlocked == false)) {
    //            renderer.material = mat;
    //        }
    //    }
    //}
    //private void RevertState(List<GameObject> cells) {
    //    if (cells != null) {
    //        foreach (GameObject cell in cells) {
    //           // ColorCell(cell, normalMat, false);
    //            //Renderer renderer = cell.GetComponentInChildren<Renderer>();
    //            //if (renderer.material != blockedMat &&  cell.GetComponent<Tile>().enabled != false) {
    //            //    renderer.material = normalMat;
    //            //    //debugText.text += "Reverting: " + cell.name + " ";
    //            //}
    //        }
    //        adjecentcells.Clear();
    //        outerCells.Clear();
    //    }
    //}
    #endregion