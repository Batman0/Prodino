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

    private void Start()
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
