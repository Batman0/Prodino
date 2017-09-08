using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStraight : Enemy {

    public float destructionMargin;

    protected override void Move()
    {
        if (isRight)
        {
            transform.Translate(Vector3.left * speed * Time.deltaTime, Space.World);
        }
        else
        {
            transform.Translate(Vector3.right * speed * Time.deltaTime, Space.World);
        }
    }

    protected override void DestroyGameobject()
    {
        if (isRight)
        {
            if (transform.position.x <= GameManager.instance.leftBound.x - destructionMargin)
            {
                Destroy(gameObject);
            }
        }
        else
        {
            if (transform.position.x >= GameManager.instance.rightBound.x + destructionMargin)
            {
                Destroy(gameObject);
            }
        }
    }
}
