using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public ShootType shootType;
    public MovementType movementType;
    public bool isRight;
    public float delay;
    private float delayTimer;
    private GameObject enemyPrefab;
    private EnemyProperties enemyProperties;

    private void Start()
    {
        enemyProperties = Register.instance.enemyProperties;
        switch (shootType)
        {
            case ShootType.DEFAULT:
                enemyPrefab = enemyProperties.straightEnemy;
                break;
            case ShootType.LASER:
                enemyPrefab = enemyProperties.diagonalEnemy;
                break;
            case ShootType.TRAIL:
                enemyPrefab = enemyProperties.aimEnemy;
                break;
            case ShootType.BOMB:
                enemyPrefab = enemyProperties.bombEnemy;
                break;
        }
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
            //Quaternion enemyRotation = enemyPrefab.transform.rotation;
            //Quaternion rotation = isRight ? enemyPrefab.transform.rotation : Quaternion.Euler(enemyRotation.z, enemyRotation.x + 180, enemyRotation.y);
            GameObject enemy = Instantiate(enemyPrefab, transform.position, enemyPrefab.transform.rotation) as GameObject;
            Enemy enemyScript = enemy.GetComponent<Enemy>();
            enemyScript.movementType = movementType;
            enemyScript.enemyProperties = enemyProperties;
            enemyScript.shootType = shootType;
            enemyScript.isRight = isRight;
            Destroy(gameObject);
        }
    }
}
