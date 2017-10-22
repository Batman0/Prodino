using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotBombDrop : EnemyShot
{

    private float loadingTime;
    private float timer;
    private GameObject prefab;
    private PropertiesBombDrop properties;

    public override void Init()
    {
        base.Init();
        properties = Register.instance.propertiesBombDrop;
        loadingTime = properties.loadingTime;
        timer = 0;
        prefab = properties.bombPrefab;
    }

    public override void ShootSidescroll(Enemy enemy)
    {
        if (timer < loadingTime)
        {
            timer += Time.deltaTime;
        }
        else
        {
            GameObject bomb = Object.Instantiate(prefab, enemy.bulletSpawnpoint.position, enemy.transform.rotation);
            timer = 0.0f;
        }
    }

    public override void ShootTopdown(Enemy enemy)
    {
        if (timer < loadingTime)
        {
            timer += Time.deltaTime;
        }
        else
        {
            GameObject bomb = Object.Instantiate(prefab, enemy.bulletSpawnpoint.position, enemy.transform.rotation);
            timer = 0.0f;
        }
    }

}
