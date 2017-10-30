using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct EnemyCreationData
{
    public ShotType prefab;
    public float delay;
}

public class EnemySpawner : MonoBehaviour
{
    private bool isRight;
    private int dataIndex;
    private Register register;
    public EnemyCreationData[] enemyCreationData;

    private void Awake()
    {
        register = Register.instance;
        dataIndex = 0;
        isRight = transform.position.x >= register.player.transform.position.x ? true : false;      
    }

    private void Update()
    {
        SpawnEnemy();
    }


    void SpawnEnemy()
    {
        if (dataIndex < enemyCreationData.Length && Time.time >= enemyCreationData[dataIndex].delay)
        {
            GameObject enemyObject = PoolManager.instance.GetpooledEnemies(PoolManager.instance.pooledEnemyClass[enemyCreationData[dataIndex].prefab.ToString()]);
            enemyObject.transform.position = transform.position;
            Enemy enemyScript = enemyObject.GetComponent<Enemy>();
            enemyScript.isRight = isRight;
            enemyObject.SetActive(true);
            dataIndex++;
        }
        else if (dataIndex >= enemyCreationData.Length)
        {
            Destroy(gameObject);
        }
    }
}
