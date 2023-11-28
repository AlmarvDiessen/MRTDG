using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class EnemySpawnInfo {
    public EnemyData enemyType;
    public int enemyCount;
}



public class CircleSpawner : MonoBehaviour {
    [SerializeField]private List<GameObject> spawnedEnemies = new List<GameObject>();
    [SerializeField]private List<Transform> spawnPoints = new List<Transform>();

    public GameObject spawnGround;
    private Bounds bounds;
    public List<EnemySpawnInfo> enemySpawnInfoList;
    public int initialWaveSize = 5;
    private int waveSize;
    public float outerRadius = 5.0f;
    public float innerRadius = 2.0f;
    public float timeBetweenWaves = 5.0f;
    public float timeBetweenSpawns = 1.0f;
    public float waveSizeMultiplier = 1.5f;

    private Coroutine spawnCoroutine;
    private bool isSpawningWave = false;
    [SerializeField] private bool isPaused = false;
    private int currentWave = 1;

    [SerializeField] private GameObject attackPoint;
    [SerializeField] private GameObject playButton;
    [SerializeField] private GameObject startWaveButton;


    private void Start() {
        waveSize = initialWaveSize;
        //buildButton.SetActive(false);
        bounds = spawnGround.GetComponent<BoxCollider>().bounds;
    }

    public void StartGame()
    {
        spawnCoroutine = StartCoroutine(SpawnWaves());
        playButton.SetActive(false);
    }

    public void Resume() {
        startWaveButton.SetActive(false);
        isPaused = false;
    }

    private IEnumerator SpawnWaves() {
        while (true) {

            while (isPaused) {
                yield return null;
            }

            isSpawningWave = true;
            yield return new WaitForSeconds(3f);
            foreach (var spawnInfo in enemySpawnInfoList) {
                for (int i = 0; i < spawnInfo.enemyCount * waveSize; i++) // Increase enemy count based on the current wave
                {
                    if (!isSpawningWave)
                        yield break; // Stop spawning if the wave was cleared prematurely

                    //float randomAngle = Random.Range(0f, 360f);
                    //float randomRadius = Random.Range(innerRadius, outerRadius);

                    //float x = randomRadius * Mathf.Cos(randomAngle * Mathf.Deg2Rad);
                    //float y = randomRadius * Mathf.Sin(randomAngle * Mathf.Deg2Rad); 

   

                    float x = Random.Range(bounds.min.x, bounds.max.x);
                    float z = Random.Range(bounds.min.z, bounds.max.z);

                    Vector3 spawnPosition = new Vector3(x, 0.2f, z); // for a spawn field

                    if (currentWave >= spawnInfo.enemyType.SpawnableInWave) {
                        GameObject enemyPrefab = spawnInfo.enemyType.Model;
                        GameObject spawnEffect = spawnInfo.enemyType.FX;
                        Quaternion targetRotation = Quaternion.LookRotation(attackPoint.transform.position - transform.position, Vector3.up);
                        GameObject spawnedFX = Instantiate(spawnEffect, SpawnPosition() + new Vector3(0,0.2f,0), targetRotation);
                        yield return new WaitForSeconds(spawnedFX.GetComponent<ParticleSystem>().main.duration);
                        GameObject spawnedEnemy = Instantiate(enemyPrefab, spawnedFX.transform.position - new Vector3(0,0.2f,0), targetRotation);
                        spawnedEnemy.transform.rotation = Quaternion.Euler(0, targetRotation.eulerAngles.y, 0);
                        spawnedEnemy = LoadData(spawnedEnemy, spawnInfo);
                        spawnedEnemies.Add(spawnedEnemy);
                        Destroy(spawnedFX, 1f);
                    }
                    yield return new WaitForSeconds(timeBetweenSpawns);
                }
            }

            isSpawningWave = false;


            // Check if all enemies from the previous wave have been eliminated
            while (!AllEnemiesEliminated()) {
                yield return null;
            }

            yield return new WaitForSeconds(timeBetweenWaves);

            currentWave++; // Increase the current wave
            foreach (EnemySpawnInfo info in enemySpawnInfoList) {
                int oldEnemyCount = info.enemyCount;
                info.enemyCount = Mathf.RoundToInt(info.enemyCount * waveSizeMultiplier);
                if (oldEnemyCount == info.enemyCount) {
                    if(currentWave >= info.enemyType.SpawnableInWave)
                    info.enemyCount++;
                }
                spawnedEnemies.Clear();
            }
        }
    }

    private Vector3 SpawnPosition() {
        int random = Random.Range(0, spawnPoints.Count);
        Vector3 pos = spawnPoints[random].position;
        return pos;
    }

    private GameObject LoadData(GameObject enemy, EnemySpawnInfo spawnInfo) {
        Enemy enemyInstance = enemy.GetComponent<Enemy>();
        enemyInstance.HealthComponent.GetComponent<Health>().Initialize(spawnInfo.enemyType.Health, spawnInfo.enemyType.Health);
        enemyInstance.EnemySpeed = spawnInfo.enemyType.MoveSpeed;
        enemyInstance.PointAmount = spawnInfo.enemyType.PointValue;
        enemyInstance.Damage = spawnInfo.enemyType.DamageValue;

        return enemy;    
    }

    private bool AllEnemiesEliminated() {
        foreach (var enemy in spawnedEnemies) {
            if (enemy != null) {
                return false;
            }
        }
        spawnedEnemies.Clear();
        isPaused = true;
        startWaveButton.SetActive(true);
        return true;
    }

    public void StopSpawning() {
        if (spawnCoroutine != null) {
            StopCoroutine(spawnCoroutine);
        }
    }

    // Add a method to stop the current wave
    public void StopCurrentWave() {
        isSpawningWave = false;
    }
}