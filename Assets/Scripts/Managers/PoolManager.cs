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
    public static PoolManager instance;
    private int pooledBulletAmount;

    [Header("Bullets")]
    public int pooledPlayerBulletAmount = 15;
    public int doubleAimingBulletAmount = 10;
    public int doubleAimingSinusoideBulletAmount = 10;
    public int forwardShooterBulletAmount = 10;
    public int sphericalAimingBulletAmount = 10;
    public int trailBulletAmount = 10;
    public int laserBulletAmount = 10;

    [Header("Enemies")]
    public int pooledEnemiesAmount = 10;


    private int i = 0;
    public listStruct playerBulletpool;

    //Enemies'list
    public listStruct enemyShooterForwardPool;

    //Bullet Enemies' list
    public listStruct bulletDoubleAimingPool;
    public listStruct bulletDoubleAimingSinusoidePool;
    public listStruct bulletForwardShooterPool;
    public listStruct bulletSphericalAimingPool;
    public listStruct bulletTrailPool;
    public listStruct bulletLaserPool;

    void Awake()
    {
        instance = this;
        pooledBulletAmount = doubleAimingBulletAmount + doubleAimingSinusoideBulletAmount + forwardShooterBulletAmount + trailBulletAmount + laserBulletAmount;
        ListInitialization();

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
    
    public GameObject GetpooledBullet(ref listStruct items, ref int pooledBullet)
    {
        if(!items.pooledItems[items.index].activeInHierarchy)
        {
            if(items.index < pooledBullet-1)
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
        GameObject forwardShooterEnemy = Instantiate(Register.instance.propertiesForwardShooter.gameObjectPrefab);
        forwardShooterEnemy.SetActive(false);
        enemyShooterForwardPool.pooledItems.Add(forwardShooterEnemy);
    }

    void ListInitialization()
    {
        playerBulletpool.pooledItems = new List<GameObject>();

        enemyShooterForwardPool.pooledItems = new List<GameObject>();

        //Enemies Bullet list
        bulletDoubleAimingPool.pooledItems = new List<GameObject>();
        bulletDoubleAimingSinusoidePool.pooledItems = new List<GameObject>();
        bulletForwardShooterPool.pooledItems = new List<GameObject>();
        bulletSphericalAimingPool.pooledItems = new List<GameObject>();
        bulletTrailPool.pooledItems = new List<GameObject>();
        bulletLaserPool.pooledItems = new List<GameObject>();


    }
}
