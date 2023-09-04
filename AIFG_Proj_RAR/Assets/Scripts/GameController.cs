using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public Transform gameController;
    public Transform mob;

    public string targetTag = "Enemy";
    int enemyCount;
    
    int num;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        GameObject[] taggedObject = GameObject.FindGameObjectsWithTag(targetTag);
        enemyCount = taggedObject.Length;
        Debug.Log(enemyCount);

        if (enemyCount < 3)
        {
            SpawnPoint();
            enemyCount += 1;
        }
        else if (enemyCount > 3) { }
    }

    public void SpawnPoint()
    {
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
            StartCoroutine(SpawnMob(spawnPoint.Count, spawnPoint));
        }  
    }

    IEnumerator SpawnMob(int spawnCount, List<GameObject> spawningPoint)
    {
        if (enemyCount < 3)
        {
            yield return new WaitForSeconds(1.5f);

            GameObject mobSpawnPoint = spawningPoint[Random.Range(0, spawnCount)];

            Vector3 spawnPos = mobSpawnPoint.transform.position;

            Transform newMob = Instantiate(mob, spawnPos, Quaternion.identity);

            newMob.SetParent(mobSpawnPoint.transform); 
        }
        else if (enemyCount > 3) { }
    }
}
