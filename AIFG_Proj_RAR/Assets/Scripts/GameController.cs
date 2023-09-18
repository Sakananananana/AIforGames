using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class GameController : MonoBehaviour
{
    #region Declaration
    public TextMeshProUGUI waveText;
    public TextMeshProUGUI timerText;
    float timeRemaining = 30;
    //declaration
    public Transform gameController;
    public Transform spawnedMob;
    //mark main character as target
    public Transform target;
    //enemy & mobs to spawn in game
    [SerializeField] List<Transform> prefabEnemy_Mob;
    public Transform goblin;
    public Transform troll;
    public Transform mage;
    public Transform wolf;
    int nextMob;

    //Check on ground enemy and coroutine progress
    [SerializeField] List<Transform> thisWaveSpawnthisEnemy;
    GameObject[] currentEnemyCount;
    public string targetTag = "Enemy";
    bool isCoroutineFinished;
    bool enemySpawning;
    bool isInterlude;
    //for enemy spawning mechanics
    int maxEnemyCountInWave;
    int enemyCountInWave;
    int enemyCount;
    int waveCount;
    //for randomize mob spawn and specific num of the mob
    int randomIndex;
    int goblinNum;
    int trollNum;
    int mageNum;
    int wolfNum;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        nextMob = 0;
        waveCount = 1;
        enemyCountInWave = 0;
        maxEnemyCountInWave = 10;
        EnemyWave();
        WaveNumberUpdate();

        isInterlude = false;
        enemySpawning = false;
        isCoroutineFinished = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        currentEnemyCount = GameObject.FindGameObjectsWithTag(targetTag);
        enemyCount = currentEnemyCount.Length;

        //for now it spawn until 10, now i want to control the mob on playground is 3 max and overall count is 10
        if (enemySpawning == false && enemyCountInWave != maxEnemyCountInWave)
        {
            if (enemyCount <= 2)
            {
                //Spawn enemy 
                StartCoroutine(SpawnPoint());
                if (!isCoroutineFinished)
                {
                    enemySpawning = true;
                    Debug.Log("Coroutine still running");
                }
            }
        }
        //to reset &  spawn enemy when it reached the max enemy in a wave, and all the enemy in playground is clear
        else if (enemyCountInWave == maxEnemyCountInWave && enemyCount == 0 && !isInterlude)
        {
            Debug.Log("Coroutine Started");
            StartCoroutine(Interlude());
            if (isInterlude)
            {
                Debug.Log("Resting");
            }
        }

        if (isInterlude)
        {
            TimerUpdate();
        }
    }

    IEnumerator SpawnPoint()
    {
        isCoroutineFinished = false;

        yield return new WaitForSeconds(1.5f);

        List<GameObject> spawnPoint = new List<GameObject>();

        foreach (Transform child in gameController)
        {
            if (child.CompareTag("Spawn"))
            {
                spawnPoint.Add(child.gameObject);
            }
        }
   
        if (spawnPoint.Count > 0)
        {
            GameObject mobSpawnPoint = spawnPoint[Random.Range(0, spawnPoint.Count)];

            Vector3 spawnPos = mobSpawnPoint.transform.position;

            Transform newMob = Instantiate(thisWaveSpawnthisEnemy[nextMob], spawnPos, Quaternion.identity);

            MeeleMobBehaviour mobScript = newMob.GetComponent<MeeleMobBehaviour>();

            newMob.SetParent(spawnedMob.transform);

            if (mobScript != null)
            {
                mobScript.target = target;
            }
        }

        isCoroutineFinished = true;

        enemySpawning = false;

        enemyCountInWave += 1;

        nextMob++;
    }

    IEnumerator Interlude()
    {
        isInterlude = true;
        thisWaveSpawnthisEnemy.Clear();
        waveCount += 1;
        
        yield return new WaitForSeconds(30);

        maxEnemyCountInWave += waveCount;
        Debug.Log("this is next wave count " + maxEnemyCountInWave);
        EnemyWave();
        WaveNumberUpdate();
        nextMob = 0;
        enemyCountInWave = 0;
        isInterlude = false;
        timeRemaining = 30;
    }

    void EnemyWave()
    {
        switch (waveCount)
        { 
            case 1:
                {
                    goblin = prefabEnemy_Mob[0];
                    mage = prefabEnemy_Mob[1];

                    mageNum = 3;
                    goblinNum = 7;

                    for (int i = 0; i < maxEnemyCountInWave; i++)
                    {
                        if (goblinNum == 0)
                        {
                            prefabEnemy_Mob.Remove(goblin);
                        }
                        if (mageNum == 0)
                        {
                            prefabEnemy_Mob.Remove(mage);
                        }

                        randomIndex = Random.Range(0, prefabEnemy_Mob.Count);
                        thisWaveSpawnthisEnemy.Add(prefabEnemy_Mob[randomIndex]);
                        if (prefabEnemy_Mob[randomIndex] == goblin)
                        {
                            goblinNum--;
                        }
                        else
                        {
                            mageNum--;
                        }
                    }

                    prefabEnemy_Mob.Clear();

                    prefabEnemy_Mob.Insert(0, goblin);
                    prefabEnemy_Mob.Insert(1, mage);
                    prefabEnemy_Mob.Insert(2, wolf);

                }
                break;

            case 2:
                {
                    goblin = prefabEnemy_Mob[0];
                    mage = prefabEnemy_Mob[1];
                    wolf = prefabEnemy_Mob[2];

                    wolfNum = 3;
                    mageNum = 3;
                    goblinNum = 6;
                    

                    for (int i = 0; i < maxEnemyCountInWave; i++)
                    {
                        if (goblinNum == 0)
                        {
                            prefabEnemy_Mob.Remove(goblin);
                        }
                        if (mageNum == 0)
                        {
                            prefabEnemy_Mob.Remove(mage);
                        }
                        if (wolfNum == 0)
                        {
                            prefabEnemy_Mob.Remove(wolf);
                        }

                        randomIndex = Random.Range(0, prefabEnemy_Mob.Count);
                        thisWaveSpawnthisEnemy.Add(prefabEnemy_Mob[randomIndex]);
                        if (prefabEnemy_Mob[randomIndex] == goblin)
                        {
                            goblinNum--;
                        }
                        if (prefabEnemy_Mob[randomIndex] == mage)
                        {
                            mageNum--;
                        }
                        if (prefabEnemy_Mob[randomIndex] == wolf)
                        {
                            wolfNum--;
                        }
                    }

                    prefabEnemy_Mob.Clear();

                    prefabEnemy_Mob.Insert(0, goblin);
                    prefabEnemy_Mob.Insert(1, mage);
                    prefabEnemy_Mob.Insert(2, wolf);
                    prefabEnemy_Mob.Insert(3, troll);
                }
                break;

            case 3: 
                {
                    goblin = prefabEnemy_Mob[0];
                    mage = prefabEnemy_Mob[1];
                    wolf = prefabEnemy_Mob[2];
                    troll= prefabEnemy_Mob[3];

                    wolfNum = 3;
                    mageNum = 3;
                    trollNum = 3;
                    goblinNum = 6;

                    for (int i = 0; i < maxEnemyCountInWave; i++)
                    {
                        if (goblinNum == 0)
                        {
                            prefabEnemy_Mob.Remove(goblin);
                        }
                        if (mageNum == 0)
                        {
                            prefabEnemy_Mob.Remove(mage);
                        }
                        if (wolfNum == 0)
                        {
                            prefabEnemy_Mob.Remove(wolf);
                        }
                        if (trollNum == 0)
                        {
                            prefabEnemy_Mob.Remove(troll);
                        }

                        randomIndex = Random.Range(0, prefabEnemy_Mob.Count);
                        thisWaveSpawnthisEnemy.Add(prefabEnemy_Mob[randomIndex]);
                        if (prefabEnemy_Mob[randomIndex] == goblin)
                        {
                            goblinNum--;
                        }
                        if (prefabEnemy_Mob[randomIndex] == mage)
                        {
                            mageNum--;
                        }
                        if (prefabEnemy_Mob[randomIndex] == wolf)
                        {
                            wolfNum--;
                        }
                        if (prefabEnemy_Mob[randomIndex] == troll)
                        {
                            trollNum--;
                        }
                    }

                    prefabEnemy_Mob.Clear();
                }
                break;
        }
    }

    void WaveNumberUpdate()
    {
        waveText.text = "Wave: " + waveCount.ToString() + "/3";
    }

    void TimerUpdate()
    {
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            timerText.text = timeRemaining.ToString();
        }
    }
}
