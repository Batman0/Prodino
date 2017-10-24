using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct EnemyCreationData
{
    public GameObject enemyPrefab;
    public float delay;
}

public class EnemySpawner : MonoBehaviour
{
    private bool isRight;
    private int dataIndex;
    public EnemyCreationData[] enemyCreationData;

    private void Awake()
    {
        dataIndex = 0;
        isRight = transform.position.x >= Register.instance.player.transform.position.x ? true : false;
    }

    private void Update()
    {
        SpawnEnemy();
    }


    void SpawnEnemy()
    {
        if (dataIndex < enemyCreationData.Length && Time.time >= enemyCreationData[dataIndex].delay)
        {
            GameObject enemy = Instantiate(enemyCreationData[dataIndex].enemyPrefab, transform.position, enemyCreationData[dataIndex].enemyPrefab.transform.rotation) as GameObject;
            Enemy enemyScript = enemy.GetComponent<Enemy>();
            enemyScript.isRight = isRight;
            dataIndex++;
        }
        else if (dataIndex >= enemyCreationData.Length)
        {
            Destroy(gameObject);
        }
    }
}
