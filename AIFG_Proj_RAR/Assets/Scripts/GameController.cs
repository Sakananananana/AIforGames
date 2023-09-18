using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GameController : MonoBehaviour
{
    //declaration
    public Transform gameController;
    public Transform spawnedMob;
    //mark main character as target
    public Transform target;
    //enemy & mobs to spawn in game
    public Transform goblin;
    public Transform[] allEnemy_Mob;
    int enemyToBeSpawn;
    int nextMob;

    //Check on ground enemy and coroutine progress
    Transform[] thisWaveSpawnthisEnemy;
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
    int goblinNum;
    int mageNum;
    int num;

    // Start is called before the first frame update
    void Start()
    {
        nextMob = 0;
        waveCount = 1;
        enemyCountInWave = 0;
        maxEnemyCountInWave = 10;
        EnemyWave();

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
            if (enemyCount <= 9)
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

            mobScript.target = target;
        }

        isCoroutineFinished = true;

        enemySpawning = false;

        enemyCountInWave += 1;

        nextMob++;
    }

    IEnumerator Interlude()
    {
        isInterlude = true;
        waveCount += 1;
        yield return new WaitForSeconds(30);

        maxEnemyCountInWave += waveCount;
        Debug.Log("this is next wave count " + maxEnemyCountInWave);
        enemyCountInWave = 0;
        isInterlude = false;
    }

    void EnemyWave()
    {
        switch (waveCount)
        { 
            case 1:
                {
                    enemyToBeSpawn = allEnemy_Mob.Length;
                    goblinNum = 7;
                    mageNum = 3;

                    for (int i = 0; i < maxEnemyCountInWave; i++)
                    {
                        int randomIndex = Random.Range(0, enemyToBeSpawn);
                        thisWaveSpawnthisEnemy[i] = allEnemy_Mob[randomIndex];
                        if (allEnemy_Mob[randomIndex] == goblin)
                        {
                            goblinNum--;
                        }
                        else
                        { 
                            mageNum--;
                        }

                        if (goblinNum == 0 && mageNum == 0)
                        { 
                            break;
                        } 
                    }

                    //7 goblin, 3 mage
                }break;

            case 2:
                { 
                    //6 goblin, 3 mage, 3 wolf rider
                }break;

            case 3: 
                { 
                    //pending
                }break;
        }
    }

}
