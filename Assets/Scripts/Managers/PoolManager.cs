using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct listStruct
{
    public List<GameObject> pooledItems;
    public int index;
}


public class PoolManager : MonoBehaviour
{
    public class PoolEnemy
    {
        public string enemyName;
        public int enemyAmount;
        public List<GameObject> pooledItems;
        public int index;
        public GameObject enemyTypeObject;

        public PoolEnemy(string _enemyName, GameObject _enemyTypeObject, int _enemyAmount)
        {
            enemyTypeObject = _enemyTypeObject;
            enemyAmount = _enemyAmount;
            enemyName = _enemyName;

            pooledItems = new List<GameObject>();
            
            for (int i = 0; i < enemyAmount; i++)
            {
                GameObject enemy = Instantiate(_enemyTypeObject) as GameObject;
                enemy.SetActive(false);
                pooledItems.Add(_enemyTypeObject);
            }
        }

        public GameObject GetpooledEnemy(PoolEnemy classEnemy)
        {
            if (!classEnemy.pooledItems[classEnemy.index].activeInHierarchy)
            {
                if (classEnemy.index < classEnemy.pooledItems.Count - 1)
                {
                    classEnemy.index++;
                    classEnemy.pooledItems[classEnemy.index].SetActive(true);
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
    }

    public static PoolManager instance;
    private int pooledBulletAmount;
    private int pooledEnemiesAmount;
    public Dictionary<string, PoolEnemy> pooledEnemyClass;

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

    void ListInitialization()
    {
        playerBulletpool.pooledItems = new List<GameObject>();
        
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
        PoolEnemy ForwardShooter = new PoolEnemy(Register.instance.enemyProperties["ForwardShooter"].ToString(), Register.instance.enemyProperties["ForwardShooter"].gameObjectPrefab, shooterForwardEnemyAmount);
        PoolEnemy Forward = new PoolEnemy(Register.instance.enemyProperties["Forward"].ToString(), Register.instance.enemyProperties["Forward"].gameObjectPrefab, forwardEnemyAmount);
        PoolEnemy LaserDiagonal = new PoolEnemy(Register.instance.enemyProperties["LaserDiagonal"].ToString(), Register.instance.enemyProperties["LaserDiagonal"].gameObjectPrefab, laserEnemyAmount);
        PoolEnemy SphericalAiming = new PoolEnemy(Register.instance.enemyProperties["SphericalAiming"].ToString(), Register.instance.enemyProperties["SphericalAiming"].gameObjectPrefab, sphericalAimingEnemyAmount);
        PoolEnemy BombDrop = new PoolEnemy(Register.instance.enemyProperties["BombDrop"].ToString(), Register.instance.enemyProperties["BombDrop"].gameObjectPrefab, bombDropEnemyAmount);
        PoolEnemy Trail = new PoolEnemy(Register.instance.enemyProperties["Trail"].ToString(), Register.instance.enemyProperties["Trail"].gameObjectPrefab, trailEnemyAmount);
        PoolEnemy DoubleAiming = new PoolEnemy(Register.instance.enemyProperties["DoubleAiming"].ToString(), Register.instance.enemyProperties["DoubleAiming"].gameObjectPrefab, doubleAimingEnemyAmount);

        pooledEnemyClass.Add("ForwardShooter", ForwardShooter);
        pooledEnemyClass.Add("Forward", Forward);
        pooledEnemyClass.Add("LaserDiagonal", LaserDiagonal);
        pooledEnemyClass.Add("SphericalAiming", SphericalAiming);
        pooledEnemyClass.Add("BombDrop", BombDrop);
        pooledEnemyClass.Add("Trail", Trail);
        pooledEnemyClass.Add("DoubleAiming", DoubleAiming);
    }
}
