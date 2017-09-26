using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : BaseBullet
{

    protected override void Move()
    {
       bulletGO.transform.Translate(Vector3.forward * speed * Time.deltaTime, Space.Self);
    }

}
