using TMPro;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

/// <summary>
/// The enemy manager to handle the waves of enemies, and adding and deducting points from the player
/// </summary>
public class EnemyManager : MonoBehaviour
{
    private Transform table;

    // Spawnpoints
    public Transform[] spawnTransforms;

    // Movement nodes
    private List<Transform[]> nodes = new List<Transform[]>();
    public Transform[] nodes1;
    public Transform[] nodes2;
    public Transform[] nodes3;
    public Transform[] nodes4;
    public Transform[] nodes5;

    // Prefabs for all 5 enemies
    public GameObject[] enemyPrefabs;

    // Amount of enemies left to spawn
    private float[] enemyAmounts = new float[] { 3, 0, 0, 0, 0 };

    private int[] enemyAmountsRounded = new int[] { 3, 0, 0, 0, 0 };

    // Spawning time variables
    [SerializeField] private float time;
    [SerializeField] private float spawnTime;
    private float spawnDelay = 1f;
    private float waveDelay = 10f;

    private float enemyMultiplier = 1.2f;
    public static int currentWave = 1;

    // Reference to ui elements
    private TMP_Text waveText;
    private TMP_Text scoreText;
    private TMP_Text pointsText;
    // Scored
    public static int score;
    public static int points = 9000;


    public int GetPoints
    {
        get { return points; }
    }

    private void Start()
    {
        nodes.Add(nodes1);   
        nodes.Add(nodes2);
        nodes.Add(nodes3);
        nodes.Add(nodes4);
        nodes.Add(nodes5);
        waveText = GameObject.Find("WaveText").GetComponent<TMP_Text>();
        scoreText = GameObject.Find("ScoreText").GetComponent<TMP_Text>();
        pointsText = GameObject.Find("PointsText").GetComponent<TMP_Text>();
        table = transform.parent;

        //buildTower.gameObject.SetActive(false);
        //cancelBuildTower.gameObject.SetActive(false);


    }

    private void Update()
    {
        time = Time.timeSinceLevelLoad;
        WaveManager();
        pointsText.text = "Points: " + points;
    }

    private void SpawnEnemy(int enemyIndex)
    {
        int spawnIndex = Random.Range(0, spawnTransforms.Length);
        GameObject enemy = Instantiate(enemyPrefabs[enemyIndex], spawnTransforms[spawnIndex].position, Quaternion.identity, table);
        enemy.GetComponent<EnemyMovement>().enabled = true;

        // Set enemies height higher based on size
        enemy.transform.position += new Vector3(0, enemy.transform.lossyScale.y/2, 0);
        //enemy.GetComponent<EnemyMovement>().nodes = nodes[spawnIndex];
    }

    // check if the player has started the game through clicking on a button
    public bool hasGameStarted = false;
    public GameObject playButton;
    public GameObject moveTable;
    public GameObject towerCostText;
    public Image moveTableHelp;


    public void ClickPlayButton()
    {
        hasGameStarted = true;
    }
    private void WaveManager()
    {
        if (hasGameStarted == true)
        {
            moveTable.gameObject.SetActive(false);
            playButton.gameObject.SetActive(false);
            towerCostText.gameObject.SetActive(true);
            moveTableHelp.gameObject.SetActive(false);

            // Enable moveTable Button and enable Build Tower Button
            if (SpawnableEnemiesLeft())
            {
                if (time > spawnTime)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        if (enemyAmountsRounded[i] > 0)
                        {
                            SpawnEnemy(i);
                            enemyAmountsRounded[i]--;
                            break;
                        }
                    }
                    spawnTime = time + spawnDelay;
                }
            }
            // Move to the next wave
            else if (!GameObject.FindGameObjectWithTag("Enemy"))
            {
                currentWave++;
                spawnDelay *= 0.99f;
                if (currentWave == 5)
                {
                    enemyAmounts[1]++;
                }
                else if (currentWave == 10)
                {
                    enemyAmounts[2]++;
                }
                else if (currentWave == 15)
                {
                    enemyAmounts[3]++;
                }
                else if (currentWave == 20)
                {
                    enemyAmounts[4]++;
                }
                waveText.text = "Wave: " + currentWave;
                // Increase the enemy amounts and set the rounded values
                for (int i = 0; i < 5; i++)
                {
                    enemyAmounts[i] *= enemyMultiplier;
                    enemyAmountsRounded[i] = Mathf.RoundToInt(enemyAmounts[i]);
                }
                spawnTime = time + waveDelay;
            }
        }
    }

    private bool SpawnableEnemiesLeft()
    {
        foreach (int enemies in enemyAmountsRounded)
        {
            if (enemies > 0)
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Adds the given <paramref name="amount"/> of score points to the player
    /// </summary>
    /// <param name="amount"></param>
    public void AddScore(int amount)
    {
        score += amount;
        scoreText.text = "Score: " + score;
    }

    /// <summary>
    /// Adds the given <paramref name="amount"/> of points to the player
    /// </summary>
    /// <param name="amount"></param>
    public void AddPoints(int amount)
    {
        points += amount;
        pointsText.text = "Points: " + points;
    }

    /// <summary>
    /// Does not check if the player has more than the given <paramref name="amount"/> of points. this can lead to negative points
    /// </summary>
    /// <param name="amount"></param>
    public void ForceDeductPoints(int amount)
    {
        points -= amount;
        pointsText.text = "Points: " + points;
    }
    /// <summary>
    /// Deducts the given amount of points if the player has more than <paramref name="amount"/>. if the player does not have enough the points are not deducted
    /// </summary>
    /// <param name="amount"></param>
    /// <returns>True if the points were deducted, otherwise false</returns>
    public bool DeductPoinstIfSufficient(int amount)
    {
        if(points > amount)
        {
            points -= amount;
            pointsText.text = "Points: " + points;
            return true;
        }
        return false;
    }
}