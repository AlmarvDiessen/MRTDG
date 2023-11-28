using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;
using UnityEngine.UI;

#nullable enable

public class BasicTower : MonoBehaviour
{
      public GameObject projectilePrefab;
    public float projectileSpeed = 10.0f;
    public float fireRate = 1.0f;
    public float maxTargetRange = 10.0f;

    protected Transform currentTarget;

    protected virtual void Start()
    {
        StartCoroutine(FireCoroutine());
    }

    protected virtual void Update()
    {
        FindNearestTarget();
    }

    protected void FindNearestTarget()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, maxTargetRange);

        float closestDistance = Mathf.Infinity;
        Transform closestTarget = null;

        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Enemy")) // Change "Enemy" to the appropriate tag for your targets
            {
                Transform targetTransform = collider.transform;
                float distance = Vector3.Distance(transform.position, targetTransform.position);

                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestTarget = targetTransform;
                }
            }
        }

        currentTarget = closestTarget;
    }

    protected virtual IEnumerator FireCoroutine()
    {
        while (true)
        {
            if (currentTarget != null)
            {
                Vector3 targetDirection = currentTarget.position - transform.position;
                float timeOfFlight = targetDirection.magnitude / projectileSpeed;
                Vector3 initialVelocity = targetDirection / timeOfFlight;

                GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
                Rigidbody projectileRigidbody = projectile.GetComponent<Rigidbody>();

                if (projectileRigidbody != null)
                {
                    projectileRigidbody.velocity = initialVelocity;
                }

                Destroy(projectile, timeOfFlight);
            }

            yield return new WaitForSeconds(1.0f / fireRate);
        }
    }
}