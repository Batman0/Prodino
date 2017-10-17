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
    //public ShotType shotType;
    //public MovementType movementType;
    private bool isRight;
    //public float delay;
    //private float delayTimer;
    private int dataIndex;
    public EnemyCreationData[] enemyCreationData;
    //private GameObject enemyPrefab;
    //private GameObject enemy;

    private void Awake()
    {
        dataIndex = 0;
        //switch (shotType)
        //{
        //    case ShotType.FORWARDSHOOTER:
        //        enemyPrefab = Register.instance.propertiesForwardShooter.gameObjectPrefab;
        //        break;
        //    case ShotType.FORWARD:
        //        enemyPrefab = Register.instance.propertiesForward.gameObjectPrefab;
        //        break;
        //    case ShotType.LASERDIAGONAL:
        //        enemyPrefab = Register.instance.propertiesLaserDiagonal.gameObjectPrefab;
        //        break;
        //    case ShotType.SPHERICALAIMING:
        //        enemyPrefab = Register.instance.propertiesSphericalAiming.gameObjectPrefab;
        //        break;
        //    case ShotType.BOMBDROP:
        //        enemyPrefab = Register.instance.propertiesBombDrop.gameObjectPrefab;
        //        break;
        //    case ShotType.TRAIL:
        //        enemyPrefab = Register.instance.propertiesTrail.gameObjectPrefab;
        //        break;
        //    case ShotType.DOUBLEAIMING:
        //        enemyPrefab = Register.instance.propertiesDoubleAiming.gameObjectPrefab;
        //        break;
        //}
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
            //Debug.Log("SSSSSS");
            GameObject enemy = Instantiate(enemyCreationData[dataIndex].enemyPrefab, transform.position, enemyCreationData[dataIndex].enemyPrefab.transform.rotation) as GameObject;
            Enemy enemyScript = enemy.GetComponent<Enemy>();
            //enemyScript.movementType = movementType;
            //enemyScript.shotType = shotType;
            enemyScript.isRight = isRight;
            //enemy.SetActive(false);

            //enemyCreationData[dataIndex].SetActive(true);
            dataIndex++;
        }
        else if (dataIndex >= enemyCreationData.Length)
        {
            Destroy(gameObject);
        }
    }
}
