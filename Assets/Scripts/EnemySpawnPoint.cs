using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnPoint : MonoBehaviour
{

    public enum EnemyType { Default, SemiCircular, Square, Diagonal }

    private const int circleEnemyLayer = 10;
    public float spawnDelay;
    private float delayTimer;
    public EnemyType myType;

    void Update()
    {
        SpawnEnemy(false);
    }

    void SpawnEnemy(bool immediate)
    {
        if (delayTimer < spawnDelay && !immediate)
        {
            delayTimer += Time.deltaTime;
        }
        else
        {
            GameObject enemyPrefab = Register.instance.enemies[(int)myType];
            GameObject enemy = Instantiate(enemyPrefab, transform.position, enemyPrefab.transform.rotation) as GameObject;
            Destroy(gameObject);
        }
    }

}
