using System.Collections;
using UnityEngine;

public class ShakeObject : MonoBehaviour
{
    public float shakeDuration = 0.1f;
    public float shakeMagnitude = 0.1f;

    private Vector3 originalPosition;
    private bool isShaking = false;


    private void Awake() {
        originalPosition = transform.position;
        
    }
    private void Start()
    {
    }

    public void StartShake()
    {
        while (!isShaking)
        {
            StartCoroutine(Shake());
        }
    }

    private IEnumerator Shake()
    {
        isShaking = true;
        float elapsedTime = 0;

        while (elapsedTime < shakeDuration)
        {
            Vector3 randomOffset = Random.insideUnitSphere * shakeMagnitude;
            transform.position = originalPosition + randomOffset;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = originalPosition;
        isShaking = false;
    }
}