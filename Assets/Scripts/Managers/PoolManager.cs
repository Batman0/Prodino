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
    public int pooledPlayerBulletAmount = 15;
    public int pooledBulletAmount = 10;
    private int i = 0;
    private int indexOfBullet;

    public listStruct playerBulletpool;
    public listStruct bulletDoubleAimingPool;
    public listStruct bulletDoubleAimingSinusoidePool;
    public listStruct bulletForwardShooterPool;
    public listStruct bulletSphericalAimingPool;
    public listStruct bulletTrailPool;
    public listStruct bulletLaserPool;

    void Awake()
    {
        instance = this;
        playerBulletpool.pooledItems = new List<GameObject>();
        bulletDoubleAimingPool.pooledItems = new List<GameObject>();
        bulletDoubleAimingSinusoidePool.pooledItems = new List<GameObject>();
        bulletForwardShooterPool.pooledItems = new List<GameObject>();
        bulletSphericalAimingPool.pooledItems = new List<GameObject>();
        bulletTrailPool.pooledItems = new List<GameObject>();
        bulletLaserPool.pooledItems = new List<GameObject>();

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

    public GameObject GetpooledBullet(listStruct items)
    {
        if(!items.pooledItems[items.index].activeInHierarchy)
        {
           return (items.pooledItems[items.index]);
        }
        return null;
    }

    void InstantiateBulletEnemy()
    {
        GameObject doubleAimingBullet = Instantiate(Register.instance.propertiesDoubleAiming.sidescrollBulletPrefab) as GameObject;
        GameObject doubleAimingSinusoideBullet = Instantiate(Register.instance.propertiesDoubleAiming.topdownBulletPrefab) as GameObject;
        GameObject forwardShooterBullet = Instantiate(Register.instance.propertiesForwardShooter.bulletPrefab) as GameObject;
        GameObject sphericalAimingBullet = Instantiate(Register.instance.propertiesSphericalAiming.bulletPrefab) as GameObject;
        GameObject trailBullet = Instantiate(Register.instance.propertiesTrail.trailPrefab) as GameObject;
        GameObject laserBullet = Instantiate(Register.instance.propertiesLaserDiagonal.laserPrefab);

        doubleAimingBullet.SetActive(false);
        doubleAimingSinusoideBullet.SetActive(false);
        forwardShooterBullet.SetActive(false);
        sphericalAimingBullet.SetActive(false);
        trailBullet.SetActive(false);
        laserBullet.SetActive(false);

        bulletDoubleAimingPool.pooledItems.Add(doubleAimingBullet);
        bulletDoubleAimingSinusoidePool.pooledItems.Add(doubleAimingSinusoideBullet);
        bulletForwardShooterPool.pooledItems.Add(forwardShooterBullet);
        bulletSphericalAimingPool.pooledItems.Add(sphericalAimingBullet);
        bulletTrailPool.pooledItems.Add(trailBullet);
        bulletLaserPool.pooledItems.Add(laserBullet);
    }
}
