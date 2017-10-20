using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public static PoolManager instance;
    public int pooledPlayerBulletAmount = 20;
    private GameObject playerBullet;
    private int i = 0;
    [HideInInspector]
    public List<GameObject> playerBulletpool;

    void Awake()
    {
        instance = this;
        playerBullet = Register.instance.propertiesPlayer.bulletPrefab;
        playerBulletpool = new List<GameObject>();
        for (i = 0; i < pooledPlayerBulletAmount; i++)
        {
            GameObject bullet = Instantiate(Register.instance.propertiesPlayer.bulletPrefab) as GameObject;
            bullet.SetActive(false);
            playerBulletpool.Add(bullet);
        }
    }

    public GameObject GetpooledBullet(int i)
    {
         if(!playerBulletpool[i].activeInHierarchy)
         {
            return (playerBulletpool[i]);
         }

        return null;
    }
}
