using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    //can add more behaviours or further make diffrent types of enemies with diffrent types of behaviours.
    [SerializeField] private int damage = 1;
    [SerializeField] private EnemyObstacleCheck check; 
    [SerializeField] private float enemySpeed = 5f;

    public int Damage { get => damage; set => damage = value; }
    public EnemyObstacleCheck Check { get => check; set => check = value; }
    public float EnemySpeed { get => enemySpeed; set => enemySpeed = value; }

    public void Awake() {
        base.Awake();
        check = this.gameObject.GetComponent<EnemyObstacleCheck>();
    }

    protected override void HandleHealthChange(int currentHealth, int maxHealth) {
        base.HandleHealthChange(currentHealth, maxHealth);
    }

    protected override void Die() {
        PointManager.AddPoints(pointAmount);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other) {
        Base targetEntity = other.GetComponent<Base>();
        if (targetEntity != null && targetEntity.GetComponent<Base>() != null) {
            Destroy(gameObject);
            targetEntity.HealthComponent.TakeDamage(damage);
        }
    }
}
