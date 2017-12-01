using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PoolManager : MonoBehaviour
{
    public class PoolEnemy
    {
        public int enemyAmount;
        public List<GameObject> pooledItems;
        public int index;
        public GameObject enemyTypeObject;
        public Enemy enemyScript;
        public GameObject enemyObject;



        public PoolEnemy(GameObject _enemyTypeObject, int _enemyAmount)
        {
            enemyTypeObject = _enemyTypeObject;
            enemyAmount = _enemyAmount;

            pooledItems = new List<GameObject>();

            for (int i = 0; i < enemyAmount; i++)
            {
                GameObject enemy = Instantiate(enemyTypeObject) as GameObject;
                //enemy.GetComponent<Enemy>().SetProperty(property);
                enemy.SetActive(false);
                pooledItems.Add(enemy);
            }
        }
        //TO REVIEW THIS FUNCTION. WHEN INDEX IS POOLDEITEMS.COUNT - 1 IT BOTH GETS 0 AND INCREASES OF 1, SKIPPING THE 0.
        public GameObject GetpooledEnemy()
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
                Debug.Log(_bulletTypeObject.name);
                GameObject bullet = Instantiate(_bulletTypeObject) as GameObject;
                bullet.SetActive(false);
                pooledItems.Add(bullet);
            }
        }
        //TO REVIEW THIS FUNCTION. WHEN INDEX IS POOLDEITEMS.COUNT - 1 IT BOTH GETS 0 AND INCREASES OF 1, SKIPPING THE 0.
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

        public PoolEnv(GameObject[] _roadTypeObject, int _envAmount)
        {
            envAmount = _envAmount;
            roadTypeObject = new GameObject[_roadTypeObject.Length];

            for (int i = 0; i < _roadTypeObject.Length; i++)
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
    public int doubleAimingBulletAmount;
    public int doubleAimingSinusoideBulletAmount;
    public int forwardShooterBulletAmount;
    public int sphericalAimingBulletAmount;
    public int trailBulletAmount;
    public int laserBulletAmount;
    public int bombDropBulletAmount;
    public Dictionary<string, int> bulletAmount = new Dictionary<string, int>();


    [Header("Enemies")]
    public int forwardEnemyAmount = 10;
    public int shooterForwardEnemyAmount = 10;
    public int doubleAimingEnemyAmount = 10;
    public int sphericalAimingEnemyAmount = 10;
    public int trailEnemyAmount = 10;
    public int laserEnemyAmount = 10;
    public int bombDropEnemyAmount = 10;
    public Dictionary<string, int> enemyAmount = new Dictionary<string, int>();

    [Header("Env")]
    public GameObject[] roads;
    public int roadsAmount = 3;

    void Awake()
    {
        instance = this;

        EnemyAmountDictionary();
        BulletAmountDictionary();

       // MaxBulletSetter();

        pooledEnemyClass = new Dictionary<string, PoolEnemy>();
        pooledBulletClass = new Dictionary<string, PoolBullet>();

        DictionaryEnemyInitialization();
        DictionaryBulletInitialization();

        PoolEnv env = new PoolEnv(roads, roadsAmount);

    }

    void DictionaryEnemyInitialization()
    {
        Enemy enemyScript;

        for (int i = 0; i < Register.instance.enemyScripts.Length; i++)
        {
            enemyScript = Register.instance.enemyScripts[i];
            PoolEnemy enemyPool = new PoolEnemy(enemyScript.gameObject, enemyAmount[enemyScript.enemyName]);
            pooledEnemyClass.Add(enemyScript.enemyName, enemyPool);

        }
    }

    void DictionaryBulletInitialization()
    {
        Enemy enemyScript;

        PoolBullet PlayerBullet = new PoolBullet(Register.instance.propertiesPlayer.bulletPrefab, bulletAmount["PlayerBullet"]);
        pooledBulletClass.Add("PlayerBullet", PlayerBullet);

        for (int i = 0; i < Register.instance.enemyScripts.Length; i++)
        {
            if (Register.instance.enemyScripts[i].enemyName != "Forward" )
            {
                enemyScript = Register.instance.enemyScripts[i];
                PoolBullet bulletPool = new PoolBullet(enemyScript.bulletPrefab, bulletAmount[enemyScript.enemyName]);
                pooledBulletClass.Add(enemyScript.enemyName, bulletPool);
            }
        }
    }

    public int MaxBulletsCalculator(int enemyAmount, float enemySpeed, float enemyRateOfFire, float minLimit, float maxLimit)
    {
        float maxBulletAmount;
        float enemyLifeTime;
        float maxDistance;
        float totalLifeTime;
        float enemyBulletAmount;
        maxDistance = Mathf.Abs(maxLimit - minLimit);
        enemyLifeTime = Mathf.Round(maxDistance / enemySpeed);
        enemyBulletAmount = enemyLifeTime / enemyRateOfFire;
        maxBulletAmount = enemyBulletAmount * enemyAmount;
        return (int)maxBulletAmount;
    }

      public void MaxBulletSetter()
      {
        //float xMax = Register.instance.xMax;
        //float xMin = Register.instance.xMin;


        //forwardShooterBulletAmount = MaxBulletsCalculator(
        //    shooterForwardEnemyAmount,
        //    Register.instance.propertiesForwardShooter.xSpeed,
        //    Register.instance.propertiesForwardShooter.fireRate,
        //    xMax,
        //    xMin);

        //doubleAimingBulletAmount = MaxBulletsCalculator(
        //    doubleAimingEnemyAmount,
        //    Register.instance.propertiesDoubleAiming.xSpeed,
        //    Register.instance.propertiesDoubleAiming.fireRate,
        //    xMax,
        //    xMin);

        //sphericalAimingBulletAmount = MaxBulletsCalculator(
        //    sphericalAimingEnemyAmount,
        //    Register.instance.propertiesSphericalAiming.xSpeed,
        //    Register.instance.propertiesSphericalAiming.fireRate,
        //    xMax,
        //    xMin);

        //trailBulletAmount = trailEnemyAmount;

        //laserBulletAmount = MaxBulletsCalculator(
        //    laserEnemyAmount,
        //    Register.instance.propertiesLaserDiagonal.xSpeed,
        //    Register.instance.propertiesLaserDiagonal.loadingTime + Register.instance.propertiesLaserDiagonal.shootingTime + Register.instance.propertiesLaserDiagonal.waitingTime,
        //    xMax,
        //    xMin);

        //bombDropBulletAmount = MaxBulletsCalculator(
        //    bombDropEnemyAmount,
        //    Register.instance.propertiesBombDrop.xSpeed,
        //    Register.instance.propertiesBombDrop.loadingTime + Register.instance.propertiesBombDrop.bombLifeTime,
        //    xMax,
        //    xMin);

        //Debug.Log("Forward" + forwardShooterBulletAmount);
        //Debug.Log("DoubleAiming" + doubleAimingBulletAmount);
        //Debug.Log("DoubleAimingSin" + doubleAimingSinusoideBulletAmount);
        //Debug.Log("SphericalAiming" + sphericalAimingBulletAmount);
        //Debug.Log("Trail" + trailBulletAmount);
        //Debug.Log("Laser" + laserBulletAmount);
        //Debug.Log("BombDrop" + bombDropBulletAmount);
    }

    void EnemyAmountDictionary()
    {
        enemyAmount.Add("ForwardShooter", shooterForwardEnemyAmount);
        enemyAmount.Add("Forward", forwardEnemyAmount);
        enemyAmount.Add("LaserDiagonal", laserEnemyAmount);
        enemyAmount.Add("SphericalAiming", sphericalAimingEnemyAmount);
        enemyAmount.Add("BombDrop", bombDropEnemyAmount);
        enemyAmount.Add("Trail", trailEnemyAmount);
        enemyAmount.Add("DoubleAiming", doubleAimingEnemyAmount);
    }

    void BulletAmountDictionary()
    {
        bulletAmount.Add("PlayerBullet", pooledPlayerBulletAmount);
        bulletAmount.Add("ForwardShooter", forwardShooterBulletAmount);
        bulletAmount.Add("LaserDiagonal", laserBulletAmount);
        bulletAmount.Add("SphericalAiming", sphericalAimingBulletAmount);
        bulletAmount.Add("BombDrop", doubleAimingBulletAmount);
        bulletAmount.Add("Trail", trailBulletAmount);
        bulletAmount.Add("DoubleAiming", doubleAimingBulletAmount);
    }
}