using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour
{

    protected Enemy enemyInstance;
    protected float speed;

    public virtual void Init(Enemy enemy)
    {
        enemyInstance = enemy;
    }

    public virtual void Move()
    {

    }

    public virtual void Shoot()
    {

    }
}
