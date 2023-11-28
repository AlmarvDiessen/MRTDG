using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Tower : Entity
{
    public GameManager gameManager;

    public LayerMask IgnoreMe;
    public float fireRate = 5f;
    private float fireCooldown = 0.0f;
    [SerializeField] private GameObject model;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Tile occupiedTile;
    // set model
    // model = getcomponent<tower>().GetModel();


    public float range = 5.0f;
    public int damagePerTick = 1;
    public int gridSize = 3;
    private IDamageable targetEnemy; // Use the IDamageable interface
    private List<Enemy> enemiesInRange = new List<Enemy>(); // Declare and initialize the list
    [SerializeField] private Transform pivot;
    [SerializeField] private Transform shootingPoint;
    [SerializeField] private ParticleSystem particleSystem;

    private Animator animation;


    //private void Start() {
    //    animation = gameObject.GetComponent<Animator>();
    //   // healthComponent.OnHealthZero += Die;
    //    healthComponent.Initialize(1, 5);
    //}    

    protected override void Start() {
        base.Start();
        animation = gameObject.GetComponent<Animator>();
        //TODO set dynamic version for 
        healthComponent.Initialize(5, 5);

        occupiedTile = GetCellAtPosition(transform.position).GetComponent<Tile>();
        occupiedTile.enabled = false;
    }


    protected virtual void Update() {
        FireForward();
    }

    protected virtual void FireForward() {
        RaycastHit hit;
        if (Physics.Raycast(shootingPoint.transform.position, shootingPoint.transform.forward, out hit, 80f, ~IgnoreMe)) {
            Enemy enemy = hit.collider.GetComponent<Enemy>();
            if (enemy != null) {
                if (fireCooldown <= 0.0f) {
                    GameObject clone = Instantiate(projectilePrefab, shootingPoint.transform.position, transform.rotation);
                    particleSystem = clone.GetComponentInChildren<ParticleSystem>();
                    particleSystem.Play();
                    animation.Play("Base Layer.Shoot", 0, 0.25f);
                    //Debug.Log("Playing anim");

                    fireCooldown = 1.0f / fireRate;
                }
                fireCooldown -= Time.deltaTime;
            }
        }
    }

    private void TargetEnemy() {
        //simulate targeting 
        GameObject closestEnemy = FindClosestEnemy(); // Find the closest enemy within range
        if (closestEnemy != null) {
            Vector3 targetDirection = closestEnemy.transform.position - pivot.position;
            targetDirection.y = 0.0f; // Lock rotation to the XZ plane (assuming Y is the vertical axis)
            pivot.rotation = Quaternion.LookRotation(targetDirection);
            // Check if it's time to fire
            if (fireCooldown <= 0.0f) {
                if (closestEnemy != null) {
                    // Rotate the tower's barrel towards the target enemy

                    Attack(closestEnemy.GetComponent<IDamageable>()); // Attack the closest enemy
                }
                fireCooldown = 1.0f / fireRate;
            }

            fireCooldown -= Time.deltaTime;
        }
    }

    void Attack(IDamageable enemy) {
        animation.SetTrigger("Shoot");
        enemy.TakeDamage(damagePerTick);
    }

    GameObject FindClosestEnemy() {
        float closestDistance = range;
        GameObject closestEnemy = null;

        foreach (Enemy enemies in enemiesInRange) {
            if (enemies != null) {
                IDamageable damageable = enemies.GetComponent<IDamageable>();

                if (damageable != null) {
                    float distance = Vector3.Distance(transform.position, enemies.transform.position);

                    if (distance < closestDistance) {
                        closestDistance = distance;
                        closestEnemy = enemies.gameObject;
                    }
                }
            }
        }

        return closestEnemy; // Set the closest enemy as the target
    }

    protected override void HandleHealthChange(int currentHealth, int maxHealth) {
        base.HandleHealthChange(currentHealth, maxHealth);
    }

    protected override void Die() {
        occupiedTile.enabled = true;
        occupiedTile = null;
        Destroy(gameObject);
    }



    void OnTriggerEnter(Collider other) {
        Enemy enemy = other.GetComponent<Enemy>();
        if (enemy != null) {
            enemiesInRange.Add(enemy);
        }
    }

    void OnTriggerExit(Collider other) {
        Enemy enemy = other.GetComponent<Enemy>();
        if (enemy != null) {
            enemiesInRange.Remove(enemy);
            if (enemy == targetEnemy) {
                targetEnemy = null; // Clear the target if the current target leaves the range
            }
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(shootingPoint.transform.position, shootingPoint.transform.forward * 5f);
    }


    private GameObject GetCellAtPosition(Vector3 position) {
        // Perform a raycast or any other method to find the cell at the specified position
        try {
            RaycastHit hit;
            Ray ray = new Ray(position + new Vector3(0, 0.1f, 0), Vector3.down);

            if (Physics.Raycast(ray, out hit, 10f, ~IgnoreMe)) {
                try {
                    if (hit.collider.gameObject.GetComponent<Tile>() != null)
                        return hit.collider.gameObject;
                    else
                        return null;
                }
                catch (Exception e) {
                    Debug.LogError(e.Message);
                }
            }
            return null;
        }
        catch (Exception e) {
            //debugText.text = e.ToString();
            return null;
        }
    }

}
