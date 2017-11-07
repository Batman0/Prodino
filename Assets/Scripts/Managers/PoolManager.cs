using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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
                pooledItems.Add(enemy);
            }
        }

        public GameObject GetpooledEnemy()
        {
            int currentIndex = index;

            if(currentIndex >= pooledItems.Count - 1)
            {
                 index = 0;
            }

            index++;
            return (pooledItems[currentIndex]);
        }
    }

    public class PoolBullet
    {
        public int bulletAmount;
        public List<GameObject> pooledItems;
        public int index;
        public GameObject bulletTypeObject;

        public PoolBullet(GameObject _bulletTypeObject, int _bulletAmount)
        {
            bulletAmount = _bulletAmount;
            bulletTypeObject = _bulletTypeObject;

            pooledItems = new List<GameObject>();

            for (int i = 0; i < _bulletAmount; i++)
            {
                GameObject bullet = Instantiate(_bulletTypeObject) as GameObject;
                bullet.SetActive(false);
                pooledItems.Add(bullet);
            }
        }

        public GameObject GetpooledBullet()
        {
            int currentIndex = index;

            if (currentIndex >= pooledItems.Count - 1)
            {
                index = 0;
            }

            index++;
            return (pooledItems[currentIndex]);
        }
    }

    public static PoolManager instance;
    private int pooledBulletAmount;
    private int pooledEnemiesAmount;
    public Dictionary<string, PoolEnemy> pooledEnemyClass;
    public Dictionary<string, PoolBullet> pooledBulletClass;

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

    void Awake()
    {
        instance = this;

        pooledEnemiesAmount = forwardEnemyAmount + shooterForwardEnemyAmount + doubleAimingEnemyAmount + sphericalAimingEnemyAmount + trailEnemyAmount + laserEnemyAmount + bombDropEnemyAmount;
        pooledBulletAmount = doubleAimingBulletAmount + doubleAimingSinusoideBulletAmount + forwardShooterBulletAmount + trailBulletAmount + laserBulletAmount + bombDropBulletAmount;

        pooledEnemyClass = new Dictionary<string, PoolEnemy>();
        pooledBulletClass = new Dictionary<string, PoolBullet>();

        DictionaryEnemyInitialization();
        DictionaryBulletInitialization();


    }
    
    void DictionaryEnemyInitialization()
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

    void DictionaryBulletInitialization()
    {
        PoolBullet PlayerBullet = new PoolBullet(Register.instance.propertiesPlayer.bulletPrefab, pooledPlayerBulletAmount);
        PoolBullet DoubleAimingBullet = new PoolBullet(Register.instance.propertiesDoubleAiming.sidescrollBulletPrefab, doubleAimingBulletAmount);
        PoolBullet DoubleAimingSinusoideBullet = new PoolBullet(Register.instance.propertiesDoubleAiming.topdownBulletPrefab, doubleAimingSinusoideBulletAmount);
        PoolBullet ForwardShooterBullet = new PoolBullet(Register.instance.propertiesForwardShooter.bulletPrefab, forwardShooterBulletAmount);
        PoolBullet SphericalAimingBullet = new PoolBullet(Register.instance.propertiesSphericalAiming.bulletPrefab, sphericalAimingBulletAmount);
        PoolBullet TrailBullet = new PoolBullet(Register.instance.propertiesTrail.trailPrefab, trailBulletAmount);
        PoolBullet LaserBullet = new PoolBullet(Register.instance.propertiesLaserDiagonal.laserPrefab, laserBulletAmount);
        PoolBullet BombDropBullet = new PoolBullet(Register.instance.propertiesBombDrop.bombPrefab, bombDropBulletAmount);

        pooledBulletClass.Add("PlayerBullet", PlayerBullet);
        pooledBulletClass.Add("DoubleAimingBullet", DoubleAimingBullet);
        pooledBulletClass.Add("DoubleAimingSinusoideBullet", DoubleAimingSinusoideBullet);
        pooledBulletClass.Add("ForwardShooterBullet", ForwardShooterBullet);
        pooledBulletClass.Add("SphericalAimingBullet", SphericalAimingBullet);
        pooledBulletClass.Add("TrailBullet", TrailBullet);
        pooledBulletClass.Add("LaserBullet", LaserBullet);
        pooledBulletClass.Add("BombDropBullet", BombDropBullet);
    }
}
