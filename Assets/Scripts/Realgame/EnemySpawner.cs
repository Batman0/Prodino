using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    public MovementType movementType;
    public float delay;
    private float delayTimer;
    private GameObject enemyPrefab;

    private void Start()
    {
        enemyPrefab = Register.instance.enemyPrefab;
    }

    private void Update()
    {
        SpawnEnemy();
    }


    void SpawnEnemy()
    {
        if (delayTimer < delay)
        {
            delayTimer += Time.deltaTime;
        }
        else
        {
            GameObject enemy = Instantiate(Register.instance.enemyPrefab, transform.position, enemyPrefab.transform.rotation) as GameObject;
            NewEnemy enemyScript = enemy.GetComponent<NewEnemy>();
            enemyScript.movementType = movementType;
            Destroy(gameObject);
        }
    }
}
