using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FloatingPoints : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI deductionText;
    [SerializeField] private float floatSpeed;
    [SerializeField] private float fadeDuration;
    [SerializeField] private float maxFloatDistance; // Maximum distance to float up
    [SerializeField] private int cost;
    private Tower tower;
    private Camera mainCamera; // Reference to the main camera

    void Start() {
        tower = GetComponent<Tower>();
        cost = tower.PointAmount;
        deductionText.text = "-" + cost.ToString();
        mainCamera = Camera.main; // Assuming you're using a single main camera in the scene

        // Call the function to make the text float up on start
        StartCoroutine(FloatUpAndFadeOnStart());
    }


    private IEnumerator FloatUpAndFadeOnStart() {
        // Ensure deductionText is not null before trying to manipulate it
        if (deductionText != null) {
            Vector3 startPos = deductionText.transform.position;
            Vector3 targetPos = startPos + Vector3.up * floatSpeed;

            float elapsedTime = 0f;

            while (elapsedTime < fadeDuration && Vector3.Distance(startPos, deductionText.transform.position) < maxFloatDistance) {
                // Move the text upwards
                deductionText.transform.position = Vector3.Lerp(startPos, targetPos, elapsedTime / fadeDuration);

                // Face the text towards the camera/player
                deductionText.transform.LookAt(mainCamera.transform.position, Vector3.up);
                deductionText.transform.Rotate(0, 180, 0); // Additional rotation to handle potential flipping


                // Fade out the text
                float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
                deductionText.GetComponent<CanvasGroup>().alpha = alpha;

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            // Ensure the text is fully faded out
            deductionText.GetComponent<CanvasGroup>().alpha = 0f;
        }
        else {
            Debug.LogWarning("Deduction Text is not assigned!");
        }
    }
}
