using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : BaseBullet
{

    protected override void Move()
    {
       transform.Translate(Vector3.right * speed * Time.deltaTime, Space.Self);
    }

}
