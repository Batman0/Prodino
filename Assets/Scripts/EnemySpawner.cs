using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public ShotType shootType;
    public MovementType movementType;
    private bool isRight;
    public float delay;
    private float delayTimer;
    private GameObject enemyPrefab;
    private Properties properties;

    private void Start()
    {
        properties = Register.instance.properties;
        switch (shootType)
        {
            case ShotType.FORWARDSHOOTER:
                enemyPrefab = properties.defaultnemy;
                break;
            case ShotType.LASERDIAGONAL:
                enemyPrefab = properties.laserEnemy;
                break;
            case ShotType.TRAIL:
                enemyPrefab = properties.trailEnemy;
                break;
            case ShotType.BOMBDROP:
                enemyPrefab = properties.bombEnemy;
                break;
            case ShotType.FORWARD:
                enemyPrefab = properties.noFireEnemy;
                break;
        }
        isRight = transform.position.x >= Register.instance.player.transform.position.x ? true : false;
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
            enemyScript.shootType = shootType;
            enemyScript.isRight = isRight;
            Destroy(gameObject);
        }
    }
}
