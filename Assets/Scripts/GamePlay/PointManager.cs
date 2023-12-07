using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PointManager : MonoBehaviour
{
    public delegate void PointChangeHandler(int amount);
    public event PointChangeHandler PointChange;

    public int points = 250;
    [SerializeField] private TextMeshProUGUI pointsText;
    [SerializeField] private TextMeshProUGUI ErrorText;
    private float counter = 0;

    public int Points { get => points; set => points = value; }

    void Start() {
        pointsText.text = "Points: " + Points.ToString();
    }


    public void AddPoints(int amount) {
        Points += amount;
        pointsText.text = "Points: " + Points.ToString();
    }

    public bool DeductPoinstIfSufficient(int amount) {

        if (Points >= amount) {
            Points -= amount;
            pointsText.text = "Points: " + Points.ToString();

            return true;
        }
        else {

            StartCoroutine("DisplayError");

            return false;
        }
    }

    IEnumerator DisplayError() {

        ErrorText.text = "Insufficient points";
        yield return new WaitForSeconds(3f);
        ErrorText.text = "";
        yield return null;
    }

    private bool CheckSufficientPoints() {
        //TODO set diffrent tower costs
        if (Points < 50) {
            return false;
        }
        else {
            Points -= 50;
            return true;
        }
    }
}
