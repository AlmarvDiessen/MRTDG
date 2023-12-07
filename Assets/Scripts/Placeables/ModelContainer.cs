using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelContainer : MonoBehaviour
{
    [SerializeField] private GameObject turretModel;
    [SerializeField] private GameObject previewModel;
    [SerializeField] private GameManager gameManager;

    public delegate void ModelSetDelegate();
    public ModelSetDelegate onModelSet;


    public void SetModel() {
        gameManager.Prefab = turretModel;
        gameManager.PrefabBp = previewModel;
        gameManager.PrefabCost = turretModel.GetComponent<Tower>().PointAmount;

        gameManager.StartBulding();
    }
}
