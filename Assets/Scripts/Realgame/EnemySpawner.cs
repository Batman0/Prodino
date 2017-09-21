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

    private void Start()
    {
        switch (shootType)
        {
            case ShootType.DEFAULT:
                enemyPrefab = Register.instance.enemyProperties.straightEnemy;
                break;
            case ShootType.LASER:
                enemyPrefab = Register.instance.enemyProperties.diagonalEnemy;
                break;
            case ShootType.TRAIL:
                enemyPrefab = Register.instance.enemyProperties.aimEnemy;
                break;
            case ShootType.BOMB:
                enemyPrefab = Register.instance.enemyProperties.bombEnemy;
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
            Quaternion enemyRotation = enemyPrefab.transform.rotation;
            Quaternion rotation = isRight ? enemyPrefab.transform.rotation : Quaternion.Euler(enemyRotation.z, enemyRotation.x + 180, enemyRotation.y);
            GameObject enemy = Instantiate(enemyPrefab, transform.position, enemyPrefab.transform.rotation) as GameObject;
            NewEnemy enemyScript = enemy.AddComponent<NewEnemy>();
            enemyScript.movementType = movementType;
            enemyScript.enemyProperties = Register.instance.enemyProperties;
            enemyScript.shootType = shootType;
            enemyScript.isRight = isRight;
            Destroy(gameObject);
            delayTimer = 0.0f;
        }
    }
}
