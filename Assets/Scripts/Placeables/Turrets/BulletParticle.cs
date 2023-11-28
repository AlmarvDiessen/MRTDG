using System.Collections.Generic;
using UnityEngine;

public class BulletParticle : MonoBehaviour
{
    public ParticleSystem particleSystem;
    public Rigidbody rb;
    public int damage;
    public float speed = 2f;
    List<ParticleCollisionEvent> colEvents = new List<ParticleCollisionEvent>();


    void Start() {
        // Get all Particle Systems attached to the GameObject

        rb = GetComponentInParent<Rigidbody>();
        rb.velocity = transform.forward * speed;

    }

    private void OnParticleCollision(GameObject other) {
        int events = particleSystem.GetCollisionEvents(other, colEvents);
        //for (int i = 0; i < events; i++) {}

        if (other.TryGetComponent(out Health health)) {
            health.TakeDamage(damage);
            Debug.Log(health.gameObject.name);
            particleSystem.Stop();
            Destroy(rb.gameObject);
        }
    }
}
