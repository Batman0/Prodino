using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinusoideBullet : BaseBullet
{

    protected override void Move()
    {
        transform.Translate(Vector3.right * Mathf.Sin(Time.time * Register.instance.properties.e_Speed),Space.Self);
    }
}
