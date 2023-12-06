using System.Collections;
using UnityEngine;

public class ShakeObject : MonoBehaviour
{
    public float shakeDuration = 0.1f;
    public float shakeMagnitude = 0.1f;

    private Vector3 originalPosition;
    private bool isShaking = false;

    public Vector3 OriginalPosition { get => originalPosition; set => originalPosition = value; }

    private void Awake() {
        OriginalPosition = transform.position;
        
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
            transform.position = OriginalPosition + randomOffset;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = OriginalPosition;
        isShaking = false;
    }
}