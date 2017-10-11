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
    private GameObject enemy;

    private void Awake()
    {
        switch (shootType)
        {
            case ShotType.FORWARDSHOOTER:
                enemyPrefab = Register.instance.propertiesForwardShooter.gameObjectPrefab;
                break;
            case ShotType.FORWARD:
                enemyPrefab = Register.instance.propertiesForward.gameObjectPrefab;
                break;
            case ShotType.LASERDIAGONAL:
                enemyPrefab = Register.instance.propertiesLaserDiagonal.gameObjectPrefab;
                break;
            case ShotType.SPHERICALAIMING:
                enemyPrefab = Register.instance.propertiesSphericalAiming.gameObjectPrefab;
                break;
            case ShotType.BOMBDROP:
                enemyPrefab = Register.instance.propertiesBombDrop.gameObjectPrefab;
                break;
            case ShotType.TRAIL:
                enemyPrefab = Register.instance.propertiesTrail.gameObjectPrefab;
                break;
            case ShotType.DOUBLEAIMING:
                enemyPrefab = Register.instance.propertiesDoubleAiming.gameObjectPrefab;
                break;
        }
        isRight = transform.position.x >= Register.instance.player.transform.position.x ? true : false;

        enemy = Instantiate(enemyPrefab, transform.position, enemyPrefab.transform.rotation) as GameObject;
        Enemy enemyScript = enemy.GetComponent<Enemy>();
        enemyScript.movementType = movementType;
        enemyScript.shotType = shootType;
        enemyScript.isRight = isRight;
        enemy.SetActive(false);
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
            enemy.SetActive(true);
            Destroy(gameObject);
        }
    }
}
