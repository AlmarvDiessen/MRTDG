using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int damage;
    private Transform target;
    public float speed = 2;

    public void SetTarget(Transform target)
    {
        this.target = target;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<Health>().TakeDamage(damage);
        }
        Destroy(gameObject);
    }

    public void Update()
    {
        if (target == null || target.IsDestroyed())
        {
            Destroy(gameObject);
        }
        else
        {
            transform.LookAt(target.transform);
            // Move forward
            transform.position += speed * Time.deltaTime * transform.forward;
        }
    }
}