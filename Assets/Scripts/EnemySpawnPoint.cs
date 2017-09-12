using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnPoint : MonoBehaviour
{

    public enum EnemyType { Default, SemiCircular, Square, Diagonal }

    public bool isRight;
    private const int circleEnemyLayer = 10;
    public float spawnDelay;
    private float delayTimer;
    public EnemyType myType;
    [Tooltip("It is necessary to fill these fields if 'My Type' is 'Square' only")]
    [Header("Path Definers")]
    public Transform[] targets;
    public float[] speeds;
    public float[] waitingTimes;

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
            Quaternion enemyPrefabRotation = enemyPrefab.transform.rotation;
            Debug.Log(enemyPrefabRotation.eulerAngles + " " + enemyPrefab.name);
            Quaternion rotation = isRight ? enemyPrefabRotation : Quaternion.Euler(enemyPrefabRotation.z, enemyPrefabRotation.x, enemyPrefabRotation.y + 180);
            GameObject enemy = Instantiate(enemyPrefab, transform.position, enemyPrefab.transform.rotation) as GameObject;
            Enemy enemyScript = enemy.GetComponent<Enemy>();
            enemyScript.isRight = isRight;
            if (myType == EnemyType.Square)
            {
                EnemySquare enemySquare = (EnemySquare)enemyScript;
                enemySquare.targets = targets;
                enemySquare.speeds = speeds;
                enemySquare.waitingTimes = waitingTimes;
            }
            Destroy(gameObject);
        }
    }
}
