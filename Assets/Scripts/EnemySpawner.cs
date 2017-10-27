using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct EnemyCreationData
{
    //public MovementType movement;
    //public ShotType shot;
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
            //Debug.Log(enemyCreationData[dataIndex].prefab.ToString());
            GameObject prefab = register.enemyProperties[enemyCreationData[dataIndex].prefab.ToString()].gameObjectPrefab;
            GameObject enemy = Instantiate(prefab, transform.position, prefab.transform.rotation) as GameObject;
            Enemy enemyScript = enemy.GetComponent<Enemy>();
            //enemyScript.movementType = enemyCreationData[dataIndex].movement;
            //enemyScript.shotType = enemyCreationData[dataIndex].shot;
            enemyScript.isRight = isRight;
            dataIndex++;
        }
        else if (dataIndex >= enemyCreationData.Length)
        {
            Destroy(gameObject);
        }
    }
}
