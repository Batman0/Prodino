using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalBullet : BaseBullet
{

    protected float destructionMargin;
    protected float xMin, xMax, yMin, yMax, zMin, zMax;
    protected Vector3 direction;

    protected virtual void Awake()
    {
        xMin = Register.instance.xMin;
        xMax = Register.instance.xMax;
        yMin = Register.instance.yMin;
        yMax = Register.instance.yMax;
        //zMin = Register.instance.zMin.Value;
        //zMax = Register.instance.zMax.Value;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        direction = transform.forward;
    }

    // Update is called once per frame
    protected override void Update ()
    {
        base.Update();	
	}

    protected virtual void DisableGameobject()
    {
        if (transform.position.x < xMin - destructionMargin || transform.position.x > xMax + destructionMargin || transform.position.y < yMin - destructionMargin || transform.position.y > yMax + destructionMargin)
        {
            gameObject.SetActive(false);
        }
    }
}
