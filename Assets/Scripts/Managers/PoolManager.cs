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

    public class PoolEnv
    {
        public int envAmount;
        public List<GameObject> pooledItems;
        public int index;
        public GameObject[] roadTypeObject;

        public PoolEnv(GameObject[] _roadTypeObject,int _envAmount)
        {
            envAmount = _envAmount;
            roadTypeObject = new GameObject[_roadTypeObject.Length];
           
            for (int i=0; i < _roadTypeObject.Length; i++)
            {
                roadTypeObject[i] = _roadTypeObject[i];
            }

            pooledItems = new List<GameObject>();

            for (int i = 0; i < roadTypeObject.Length; i++)
            {
                for (int j = 0; j < envAmount; j++)
                {
                    GameObject road = Instantiate(_roadTypeObject[j]) as GameObject;
                    road.SetActive(false);
                    pooledItems.Add(road);
                }
            }
        }

        public GameObject GetPooledEnv(int index)
        {
            return (pooledItems[index]);
        }

    }

    public static PoolManager instance;
    private int pooledBulletAmount;
    private int pooledEnemiesAmount;
    public Dictionary<string, PoolEnemy> pooledEnemyClass;
    public Dictionary<string, PoolBullet> pooledBulletClass;

    [Header("Bullets")]

	public int pooledPlayerBulletAmount;

	private int doubleAimingBulletAmount;

	private int doubleAimingSinusoideBulletAmount;
	
	private int forwardShooterBulletAmount;
	
	private int sphericalAimingBulletAmount;

	private int trailBulletAmount;
	
	private int laserBulletAmount;
	
	private int bombDropBulletAmount;

    [Header("Enemies")]
    public int forwardEnemyAmount = 10;
    public int shooterForwardEnemyAmount = 10;
    public int doubleAimingEnemyAmount = 10;
    public int sphericalAimingEnemyAmount = 10;
    public int trailEnemyAmount = 10;
    public int laserEnemyAmount = 10;
    public int bombDropEnemyAmount = 10;

    [Header("Env")]
    public GameObject[] roads;
    public int roadsAmount = 3;

    void Awake()
    {
        instance = this;

		MaxBulletSetter ();

        pooledEnemyClass = new Dictionary<string, PoolEnemy>();
        pooledBulletClass = new Dictionary<string, PoolBullet>();

        DictionaryEnemyInitialization();
        DictionaryBulletInitialization();

        PoolEnv env = new PoolEnv(roads, roadsAmount);
                
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

	public int MaxBulletsCalculator(int enemyAmount, float enemySpeed, float enemyRateOfFire, float minLimit, float maxLimit)
	{
		float maxBulletAmount;
		float enemyLifeTime;
		float maxDistance;
		float totalLifeTime;
		float enemyBulletAmount;
		maxDistance = Mathf.Abs( maxLimit - minLimit);
		enemyLifeTime = Mathf.Round (maxDistance / enemySpeed);
		enemyBulletAmount = enemyLifeTime / enemyRateOfFire;
		maxBulletAmount = enemyBulletAmount * enemyAmount;
		return (int)maxBulletAmount;
	}

	public void MaxBulletSetter()
	{
		float xMax = Register.instance.xMax;
		float xMin = Register.instance.xMin;

		forwardShooterBulletAmount = MaxBulletsCalculator(
			shooterForwardEnemyAmount,
			Register.instance.propertiesForwardShooter.xSpeed,
			Register.instance.propertiesForwardShooter.fireRate,
			xMax,
			xMin);

		doubleAimingBulletAmount = MaxBulletsCalculator (
			doubleAimingEnemyAmount,
			Register.instance.propertiesDoubleAiming.xSpeed,
			Register.instance.propertiesDoubleAiming.fireRate,
			xMax,
			xMin);

		doubleAimingSinusoideBulletAmount = MaxBulletsCalculator (
			doubleAimingEnemyAmount,
			Register.instance.propertiesDoubleAiming.xSpeed,
			Register.instance.propertiesDoubleAiming.fireRate,
			xMax,
			xMin);

		sphericalAimingBulletAmount = MaxBulletsCalculator (
			sphericalAimingEnemyAmount,
			Register.instance.propertiesSphericalAiming.xSpeed,
			Register.instance.propertiesSphericalAiming.fireRate,
			xMax,
			xMin);

		trailBulletAmount = trailEnemyAmount;

		laserBulletAmount = MaxBulletsCalculator (
			laserEnemyAmount,
			Register.instance.propertiesLaserDiagonal.xSpeed,
			Register.instance.propertiesLaserDiagonal.loadingTime + Register.instance.propertiesLaserDiagonal.shootingTime + Register.instance.propertiesLaserDiagonal.waitingTime,
			xMax,
			xMin);

		bombDropBulletAmount = MaxBulletsCalculator (
			bombDropEnemyAmount,
			Register.instance.propertiesBombDrop.xSpeed,
			Register.instance.propertiesBombDrop.loadingTime + Register.instance.propertiesBombDrop.bombLifeTime,
			xMax,
			xMin);
		
//		Debug.Log ("Forward" + forwardShooterBulletAmount);
//		Debug.Log ("DoubleAiming" + doubleAimingBulletAmount);
//		Debug.Log ("DoubleAimingSin" + doubleAimingSinusoideBulletAmount);
//		Debug.Log ("SphericalAiming" + sphericalAimingBulletAmount);
//		Debug.Log ("Trail" + trailBulletAmount);
//		Debug.Log ("Laser" + laserBulletAmount);
//		Debug.Log ("BombDrop" + bombDropBulletAmount);
	}
}