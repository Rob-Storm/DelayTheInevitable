using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{

    PlayerMain player;

    public GameObject[] spawnObjects;

    public GameObject healthKit;

    public float coolDown;

    public float minX, maxX, minZ, maxZ;

    Vector3 mapSize;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMain>();

        StartCoroutine(SpawnTimer());
    }

    IEnumerator SpawnTimer()
    {
        int randomIndex = Random.Range(0, spawnObjects.Length);
        Vector3 randomSpawnPosition = new Vector3(Random.Range(minX,maxX), -0.5f, Random.Range(minZ, maxZ));

        if(player.isAlive)
        {
            Instantiate(spawnObjects[randomIndex], randomSpawnPosition, Quaternion.identity);
            yield return new WaitForSeconds(coolDown);
            StartCoroutine(SpawnTimer());
        }

        else
        {
            Instantiate(healthKit, randomSpawnPosition, Quaternion.identity);
            yield return new WaitForSeconds(0.25f);
            StartCoroutine(SpawnTimer());
        }
    }
}
