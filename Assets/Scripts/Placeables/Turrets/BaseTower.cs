using System;
using System.Collections;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
#nullable enable

/// <summary>
/// A base class that all towers implement
/// </summary>
public abstract class TowerBase : MonoBehaviour
{
    /// <summary>
    /// The type of the tower
    /// </summary>
    public Type TowerType
    {
        get { return GetType(); }
    }


    /// <summary>
    /// The damage the projectile of this tower should deal to the enemy
    /// </summary>
    [SerializeField] protected float damage = 1f;
    /// <summary>
    /// The speed of the projectile
    /// </summary>
    [SerializeField] protected float firePower = 50f;
    /// <summary>
    /// The range of the tower
    /// </summary>
    [SerializeField] protected float range = 0.40f;
    /// <summary>
    /// The fire rate of this tower
    /// </summary>
    [SerializeField] protected float fireRate = 0.5f;
    /// <summary>
    /// The number of upgrades this tower has. must be 0-5
    /// </summary>
    [SerializeField, Range(0, 5)] protected int upgradeSlots = 0;
    [SerializeField] private Transform rotationPoint;
    [SerializeField] private ParticleSystem ShploofParticleSystem;
    [SerializeField] private int ParticleCount = 500;
    [SerializeField, Range(0.00001f, 0.01f)] private float ParticleEmitDelay = 500;
    protected bool isShooting;
    public TMP_Text damageUpgradeCount;
    public TMP_Text rangeUpgradeCount;
    public TMP_Text fireRateUpgradeCount;


    /// <summary>
    /// The enemy manager
    /// </summary>
    protected EnemyManager enemyManager;
    /// <summary>
    /// The transform of the towerslot this tower is placed on. this is used when the table is moved to place the tower back on the slot
    /// </summary>
    protected Transform InhabitingTowerSlot;

    protected void Init() {
        enemyManager = GameObject.FindGameObjectWithTag("EnemyManager").GetComponent<EnemyManager>();
        if (enemyManager == null)
            throw new Exception("fuck you enemy manager was not found >:( in tower type: " + GetType().Name);
    }

    /// <summary>
    /// Call this to give the tower its next target. this target may be the same as the previous one
    /// </summary>
    protected void AquireTarget()
    {
        var data = CheckTargets();
        if (data.target != null)
        {
            if (rotationPoint != null)
                rotationPoint.LookAt(data.target.transform);
            if (data.distance <= range)
            {
                var newtarget = CheckTargets();

                if (isShooting)
                    return;
                if (newtarget.target != null)
                    StartCoroutine(FireCoroutine(newtarget.target));
            }
        }
    }
    private (GameObject? target, float distance) CheckTargets()
    {
        GameObject[] totalEnemies;
        totalEnemies = GameObject.FindGameObjectsWithTag("Enemy");

        GameObject? closestEnemy = null;
        if (totalEnemies.Length == 0)
            return (null, 0);
        float distance = Mathf.Infinity;

        Vector3 towerPosition = transform.position;

        foreach (GameObject enemy in totalEnemies)
        {
            Vector3 diffrence = enemy.transform.position - towerPosition;
            float curDistance = diffrence.sqrMagnitude;
            if (curDistance <= distance)
            {
                closestEnemy = enemy;
                distance = curDistance;
            }
        }
        return (closestEnemy, distance);
    }

    /// <summary>
    /// Upgrades the range of this tower if the player has enough points, and this tower doesnt already have 5 upgrades
    /// </summary>
    public int upgradeCost = 50;
    public void RangeUpgrade()
    {
        if (upgradeSlots == 5)
            return;
        if (!enemyManager.DeductPoinstIfSufficient(upgradeCost))
        {
            rangeUpgradeCount.text = "Upgrade:\r\n" + upgradeSlots + "/5";
            Debug.Log("RangeUpgrade ++");
            return;
        }

        upgradeSlots++;
        range += 0.04f;

    }
    /// <summary>
    /// Upgrades the damage of this tower if the player has enough points, and this tower doesnt already have 5 upgrades
    /// </summary>
    public void DamageUpgrade()
    {

        if (upgradeSlots == 5)
            return;
        if (!enemyManager.DeductPoinstIfSufficient(upgradeCost))
        {
            damageUpgradeCount.text = "Upgrade:\r\n" + upgradeSlots + "/5";
            Debug.Log("DamageUpgrade ++");

            return;
        }

        upgradeSlots++;
        damage += 1f;

    }
    /// <summary>
    /// Upgrades the fire rate of this tower if the player has enough points, and this tower doesnt already have 5 upgrades
    /// </summary>
    public void FireRateUpgrade()
    {
        if (upgradeSlots == 5)
            return;
        if (!enemyManager.DeductPoinstIfSufficient(upgradeCost))
        {
            fireRateUpgradeCount.text = "Upgrade:\r\n" + upgradeSlots + "/5";
            Debug.Log("FireRateUpgrade ++");

            return;
        }

        upgradeSlots++;
        fireRate += 0.2f;
    }
    /// <summary>
    /// Gets called if a valid target is found in <see cref="AquireTarget"/>
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    protected abstract IEnumerator FireCoroutine(GameObject target);

    public void DestroyTower()
    {
        //Destroy(this);
        Destroy(gameObject);
    }


    /// <summary>
    /// Emits particles using the attached particle system
    /// </summary>
    protected void ShploofEffect()
    {
        if (ShploofParticleSystem != null)
            StartCoroutine(EmitParticles());

        IEnumerator EmitParticles()
        {
            for (int i = 0; i < ParticleCount; i++)
            {
                ShploofParticleSystem.Emit(1);
                yield return new WaitForSeconds(ParticleEmitDelay);
            }
        }
    }

    protected void NewTurretEffect()
    {
        if (ShploofParticleSystem != null)
            StartCoroutine(EmitParticles());

        IEnumerator EmitParticles()
        {
            for (int i = 0; i < ParticleCount; i++)
            {
                ShploofParticleSystem.Emit(1);
                // Try new way
                yield return new WaitForSeconds(ParticleEmitDelay);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, 0.1f);
        Gizmos.DrawSphere(transform.position, range);
    }
}
