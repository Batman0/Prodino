using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDefault : Enemy {

    public float destructionMargin;

    public override void Move()
    {
        transform.Translate(Vector3.right * -speed * Time.deltaTime, Space.World);
    }

    protected override void DestroyGameobject()
    {
        if (transform.position.x <= GameManager.instance.leftBound.x - destructionMargin)
        {
            Destroy(gameObject);
        }
    }
}
