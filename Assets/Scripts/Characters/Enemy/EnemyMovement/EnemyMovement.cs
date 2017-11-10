using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement
{

    protected Enemy enemyInstance;
    protected float speed;

    public virtual void Init(Enemy enemy)
    {
        enemyInstance = enemy;
    }

    public virtual void Movement(Enemy enemy)
    {

    }

    public virtual void MoveTopdown(Enemy enemy)
    {

    }

}
