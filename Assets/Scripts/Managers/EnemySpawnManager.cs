using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum EnemyStringName
{
    ForwardShooter,
    Forward,
    LaserDiagonal,
    SphericalAiming,
    BombDrop,
    Trail,
    DoubleAiming

}

[System.Serializable]
public struct EnemyCreationData
{
    public EnemyStringName enemyStringName;
    public float delay;
}

public class EnemySpawnManager : MonoBehaviour
{
    private bool isRight;
    private int dataIndex;
    private Register register;
    public EnemyCreationData[] enemyCreationData;
    private float timerToSpawn = 0.0f;

    private void Awake()
    {
        register = Register.instance;
        dataIndex = 0;
        isRight = transform.position.x >= register.player.transform.position.x ? true : false;      
    }

    private void Update()
    {
        if(!GameManager.instance.isBossAlive)
        {
            timerToSpawn += Time.deltaTime;
            SpawnEnemy(timerToSpawn);
        }
    }


    void SpawnEnemy(float _timerToSpawn)
    {
        if (dataIndex < enemyCreationData.Length && _timerToSpawn >= enemyCreationData[dataIndex].delay)
        {
            EnemyStringName enemyCD = enemyCreationData[dataIndex].enemyStringName;
            string propertiesString = register.enemyPropertiesDictionary[enemyCD.ToString()].enemyName;
            GameObject enemyObject = PoolManager.instance.pooledEnemyClass[propertiesString].GetpooledEnemy();
            enemyObject.transform.position = transform.position;
            Enemy enemyScript = enemyObject.GetComponent<Enemy>();
            enemyScript.isRight = isRight;
            if (!enemyScript.isRight)
            {
                enemyObject.transform.Rotate(Vector3.up, 180, Space.World);
            }
            enemyObject.SetActive(true);
            dataIndex++;
        }
        else if (dataIndex >= enemyCreationData.Length)
        {
            gameObject.SetActive(false);
        }
    }
}
