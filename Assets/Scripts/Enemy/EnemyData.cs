using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu]
public class EnemyData : ScriptableObject
{
    [SerializeField] private GameObject fx;
    [SerializeField] private GameObject model;
    [SerializeField] private int health;
    [SerializeField] private int spawnableInWave;
    [SerializeField] private int pointValue;
    [SerializeField] private int damageValue;
    [SerializeField] private float moveSpeed;

    public GameObject Model { get => model; set => model = value; }
    public GameObject FX { get => fx; set => fx = value; }
    public int Health { get => health; set => health = value; }
    public int SpawnableInWave { get => spawnableInWave; set => spawnableInWave = value; }
    public int PointValue { get => pointValue; set => pointValue = value; }
    public int DamageValue { get => damageValue; set => damageValue = value; }
    public float MoveSpeed { get => moveSpeed; set => moveSpeed = value; }
}
