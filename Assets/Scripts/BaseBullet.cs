﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBullet : MonoBehaviour
{
    public float bulletSpeed = 10.0f;
    public float lifeTime = 4.0f;
    /// <summary>
    /// How much far must the bullet be from the near clipping plane to be destroyed?
    /// </summary>
    public float destructionMargin;

    void FixedUpdate()
    {
        Move();
        DestroyGameobject();
    }

    void OnTriggerEnter(Collider other)
    {
        if (gameObject.tag == "PlayerBullet" && other.gameObject.tag == "Enemy")
        {
            Destroy(other.gameObject);
        }
    }

    void DestroyGameobject()
    {
        if (transform.position.x <= GameManager.instance.leftBound.x - destructionMargin || transform.position.x > GameManager.instance.rightBound.x + destructionMargin)
        {
            Destroy(gameObject);
        }
    }

    protected virtual void Move()
    {

    }
}