using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnPoint : MonoBehaviour
{

    private const int circleEnemyLayer = 10;
    public float spawnDelay;
    private float delayTimer;
    public GameObject enemyPrefab;

    void Start()
    {
        if (enemyPrefab.layer == circleEnemyLayer)
        {
            SpawnEnemy(true);
        }
    }

    void Update()
    {
        if (enemyPrefab.layer != circleEnemyLayer)
        {
            SpawnEnemy(false);
        }
    }

    void SpawnEnemy(bool immediate)
    {
        if (delayTimer < spawnDelay && !immediate)
        {
            delayTimer += Time.deltaTime;
        }
        else
        {
            GameObject enemy = Instantiate(enemyPrefab, transform.position, enemyPrefab.transform.rotation) as GameObject;
            delayTimer = 0.0f;
        }
    }

}
