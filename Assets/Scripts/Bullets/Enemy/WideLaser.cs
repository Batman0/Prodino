﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WideLaser : BaseBullet
{

    protected override void Start()
    {
        Destroy(gameObject, Register.instance.propertiesTrail.fadeTime);
    }

    protected override void Update()
    {
        Extend();
    }

    private void Extend()
    {
        if (transform.localScale.z - Mathf.Abs(transform.position.x) < Register.instance.xMax)
        {
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z + Register.instance.propertiesTrail.trailSpeed * Time.deltaTime);
        }
    }

}