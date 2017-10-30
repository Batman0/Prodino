using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct listStruct
{
    public List<GameObject> pooledItems;
    public int index;
}

public class PoolEnemy
{
    public List<GameObject> pooledItems;
    public int index;
}


public class PoolManager : MonoBehaviour
{
    public static PoolManager instance;
    private int pooledBulletAmount;
    private int pooledEnemiesAmount;
    public Dictionary<string, PoolEnemy> pooledEnemyClass;
    //public Dictionary<string, listStruct> enemyListDictionary;
    //public Dictionary<string, int> bulletAmountDictionary;

    [Header("Bullets")]
    public int pooledPlayerBulletAmount = 15;
    public int doubleAimingBulletAmount = 10;
    public int doubleAimingSinusoideBulletAmount = 10;
    public int forwardShooterBulletAmount = 10;
    public int sphericalAimingBulletAmount = 10;
    public int trailBulletAmount = 10;
    public int laserBulletAmount = 10;
    public int bombDropBulletAmount = 10;

    [Header("Enemies")]
    public int forwardEnemyAmount = 10;
    public int shooterForwardEnemyAmount = 10;
    public int doubleAimingEnemyAmount = 10;
    public int sphericalAimingEnemyAmount = 10;
    public int trailEnemyAmount = 10;
    public int laserEnemyAmount = 10;
    public int bombDropEnemyAmount = 10;

    //Enemies'list
    public PoolEnemy enemyForwardPool;
    public PoolEnemy enemyShooterForwardPool;
    public PoolEnemy enemyDoubleAimingPool;
    public PoolEnemy enemySphericalAimingPool;
    public PoolEnemy enemyTrailPool;
    public PoolEnemy enemyLaserPool;
    public PoolEnemy enemyBombDropPool;

    private int i = 0;

    public listStruct playerBulletpool;

    //Bullet Enemies' list
    public listStruct bulletDoubleAimingPool;
    public listStruct bulletDoubleAimingSinusoidePool;
    public listStruct bulletForwardShooterPool;
    public listStruct bulletSphericalAimingPool;
    public listStruct bulletTrailPool;
    public listStruct bulletLaserPool;
    public listStruct bulletBombDropPool;

    void Awake()
    {
        instance = this;

        pooledEnemiesAmount = forwardEnemyAmount + shooterForwardEnemyAmount + doubleAimingEnemyAmount + sphericalAimingEnemyAmount + trailEnemyAmount + laserEnemyAmount + bombDropEnemyAmount;
        pooledBulletAmount = doubleAimingBulletAmount + doubleAimingSinusoideBulletAmount + forwardShooterBulletAmount + trailBulletAmount + laserBulletAmount + bombDropBulletAmount;

        ListInitialization();

        pooledEnemyClass = new Dictionary<string, PoolEnemy>();

        DictionaryInitialization();

        for (i = 0; i < pooledPlayerBulletAmount; i++)
        {
            GameObject bullet = Instantiate(Register.instance.propertiesPlayer.bulletPrefab) as GameObject;
            bullet.SetActive(false);
            playerBulletpool.pooledItems.Add(bullet);
        }

        for(i = 0; i < pooledEnemiesAmount; i++)
        {
            InstantiateEnemy();
        }

        for(i = 0; i < pooledBulletAmount; i++)
        {
            InstantiateBulletEnemy();
        }

    }
    
    public GameObject GetpooledBullet(ref listStruct items, int pooledBullet)
    {
        if(!items.pooledItems[items.index].activeInHierarchy)
        {
            if(items.index < pooledBullet -1)
            {
                items.index++;
                return (items.pooledItems[items.index - 1]);
            }
            else
            {
                items.index = 0;
                return (items.pooledItems[pooledBullet - 1]);
            }  
        }
        return null;
    }

    public GameObject GetpooledEnemies(PoolEnemy classEnemy)
    {
        if(!classEnemy.pooledItems[classEnemy.index].activeInHierarchy)
        {
            if(classEnemy.index < classEnemy.pooledItems.Count - 1)
            {
                classEnemy.index++;
                return (classEnemy.pooledItems[classEnemy.index - 1]);
            }
            else
            {
                classEnemy.index = 0;
                return (classEnemy.pooledItems[classEnemy.pooledItems.Count - 1]);
            }
        }
        return null;
    }

    void InstantiateBulletEnemy()
    {
        if(i < doubleAimingBulletAmount)
        {
            GameObject doubleAimingBullet = Instantiate(Register.instance.propertiesDoubleAiming.sidescrollBulletPrefab) as GameObject;
            doubleAimingBullet.SetActive(false);
            bulletDoubleAimingPool.pooledItems.Add(doubleAimingBullet);
        }
        if (i < bombDropBulletAmount)
        {
            GameObject bombDropBullet = Instantiate(Register.instance.propertiesBombDrop.bombPrefab) as GameObject;
            bombDropBullet.SetActive(false);
            bulletBombDropPool.pooledItems.Add(bombDropBullet);
        }
        if(i < doubleAimingSinusoideBulletAmount)
        {
            GameObject doubleAimingSinusoideBullet = Instantiate(Register.instance.propertiesDoubleAiming.topdownBulletPrefab) as GameObject;
            doubleAimingSinusoideBullet.SetActive(false);
            bulletDoubleAimingSinusoidePool.pooledItems.Add(doubleAimingSinusoideBullet);
        }

        if(i < forwardShooterBulletAmount)
        {
            GameObject forwardShooterBullet = Instantiate(Register.instance.propertiesForwardShooter.bulletPrefab) as GameObject;
            forwardShooterBullet.SetActive(false);
            bulletForwardShooterPool.pooledItems.Add(forwardShooterBullet);
        }
        
        if(i < sphericalAimingBulletAmount)
        {
            GameObject sphericalAimingBullet = Instantiate(Register.instance.propertiesSphericalAiming.bulletPrefab) as GameObject;
            sphericalAimingBullet.SetActive(false);
            bulletSphericalAimingPool.pooledItems.Add(sphericalAimingBullet);
        }
        
        if(i < trailBulletAmount)
        {
            GameObject trailBullet = Instantiate(Register.instance.propertiesTrail.trailPrefab) as GameObject;
            trailBullet.SetActive(false);
            bulletTrailPool.pooledItems.Add(trailBullet);
        }

        if(i < laserBulletAmount)
        {
            GameObject laserBullet = Instantiate(Register.instance.propertiesLaserDiagonal.laserPrefab) as GameObject;
            laserBullet.SetActive(false);
            bulletLaserPool.pooledItems.Add(laserBullet);
        }
    }

    void InstantiateEnemy()
    {
        if(i < forwardEnemyAmount)
        {
            GameObject forwardEnemy = Instantiate(Register.instance.propertiesForward.gameObjectPrefab);
            forwardEnemy.SetActive(false);
            enemyForwardPool.pooledItems.Add(forwardEnemy);
        }

        if(i < shooterForwardEnemyAmount)
        {
            GameObject forwardShooterEnemy = Instantiate(Register.instance.propertiesForwardShooter.gameObjectPrefab) as GameObject;
            forwardShooterEnemy.SetActive(false);
            enemyShooterForwardPool.pooledItems.Add(forwardShooterEnemy);
        }

        if(i < doubleAimingEnemyAmount)
        {
            GameObject doubleAimingEnemy = Instantiate(Register.instance.propertiesDoubleAiming.gameObjectPrefab) as GameObject;
            doubleAimingEnemy.SetActive(false);
            enemyDoubleAimingPool.pooledItems.Add(doubleAimingEnemy);
        }
        if(i < bombDropEnemyAmount)
        {
            GameObject bombDropEnemy = Instantiate(Register.instance.propertiesBombDrop.gameObjectPrefab)as GameObject;
            bombDropEnemy.SetActive(false);
            enemyBombDropPool.pooledItems.Add(bombDropEnemy);
        }

        if(i < sphericalAimingEnemyAmount)
        {
            GameObject sphericalAimingEnemy = Instantiate(Register.instance.propertiesSphericalAiming.gameObjectPrefab) as GameObject;
            sphericalAimingEnemy.SetActive(false);
            enemySphericalAimingPool.pooledItems.Add(sphericalAimingEnemy);
        }

        if(i < trailEnemyAmount)
        {
            GameObject trailEnemy = Instantiate(Register.instance.propertiesTrail.gameObjectPrefab) as GameObject;
            trailEnemy.SetActive(false);
            enemyTrailPool.pooledItems.Add(trailEnemy);
        }

        if(i < laserEnemyAmount)
        {
            GameObject laserEnemy = Instantiate(Register.instance.propertiesLaserDiagonal.gameObjectPrefab) as GameObject;
            laserEnemy.SetActive(false);
            enemyLaserPool.pooledItems.Add(laserEnemy);
        }
    }

    void ListInitialization()
    {
        playerBulletpool.pooledItems = new List<GameObject>();

        //Enemeis list
        enemyForwardPool = new PoolEnemy();
        enemyShooterForwardPool = new PoolEnemy();
        enemyDoubleAimingPool = new PoolEnemy();
        enemySphericalAimingPool = new PoolEnemy();
        enemyTrailPool = new PoolEnemy();
        enemyLaserPool = new PoolEnemy();
        enemyBombDropPool = new PoolEnemy();

        enemyForwardPool.pooledItems = new List<GameObject>();
        enemyShooterForwardPool.pooledItems = new List<GameObject>();
        enemyDoubleAimingPool.pooledItems = new List<GameObject>();
        enemySphericalAimingPool.pooledItems = new List<GameObject>();
        enemyTrailPool.pooledItems = new List<GameObject>();
        enemyLaserPool.pooledItems = new List<GameObject>();
        enemyBombDropPool.pooledItems = new List<GameObject>();

        //Enemies Bullet list
        bulletDoubleAimingPool.pooledItems = new List<GameObject>();
        bulletDoubleAimingSinusoidePool.pooledItems = new List<GameObject>();
        bulletForwardShooterPool.pooledItems = new List<GameObject>();
        bulletSphericalAimingPool.pooledItems = new List<GameObject>();
        bulletTrailPool.pooledItems = new List<GameObject>();
        bulletLaserPool.pooledItems = new List<GameObject>();
        bulletBombDropPool.pooledItems = new List<GameObject>();
    }

    void DictionaryInitialization()
    {
        pooledEnemyClass.Add("ForwardShooter", enemyShooterForwardPool);
        pooledEnemyClass.Add("Forward", enemyForwardPool);
        pooledEnemyClass.Add("LaserDiagonal", enemyLaserPool);
        pooledEnemyClass.Add("SphericalAiming", enemySphericalAimingPool);
        pooledEnemyClass.Add("BombDrop", enemyBombDropPool);
        pooledEnemyClass.Add("Trail", enemyTrailPool);
        pooledEnemyClass.Add("DoubleAiming", enemyDoubleAimingPool);
    }
}
