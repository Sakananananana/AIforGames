using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public Transform gameController;
    public Transform mob;
    public Transform target;

    GameObject[] taggedObject;
    public string targetTag = "Enemy";
    bool isCoroutineFinished = true;
    bool enemySpawning = false;
    int enemyCountInWave = 0;
    int enemyCount;
    int num;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Array move up
        //After instatiate +1 to tagged object
        taggedObject = GameObject.FindGameObjectsWithTag(targetTag);
        enemyCount = taggedObject.Length;

        if (enemySpawning == false)
        {
            if (enemyCount < 3)
            {
                StartCoroutine(SpawnPoint());
                if (!isCoroutineFinished) 
                {
                    enemySpawning = true;
                    Debug.Log("Coroutine still running");
                }
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

            Transform newMob = Instantiate(mob, spawnPos, Quaternion.identity);

            MeeleMobBehaviour mobScript = newMob.GetComponent<MeeleMobBehaviour>();

            mobScript.target = target;

            newMob.SetParent(mobSpawnPoint.transform);
        }

        isCoroutineFinished = true;

        enemySpawning = false;

        enemyCountInWave += 1;
    }

}
