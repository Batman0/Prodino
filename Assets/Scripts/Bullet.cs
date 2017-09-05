using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    protected bool isRight;
    protected bool isLeft;
    protected bool isCenter;
    public float bulletSpeed = 10.0f;
    public float lifeTime = 4.0f;
    /// <summary>
    /// How much far must the bullet be from the near clipping plane to be destroyed?
    /// </summary>
    public float destructionMargin;


    private void Awake()
    {
        AssignDirection();
    }

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

    void AssignDirection()
    {
        if (transform.position.x > GameManager.instance.player.transform.position.x)
        {
            isRight = true;
            isLeft = false;
            isCenter = false;
        }
        else if (transform.position.x < GameManager.instance.player.transform.position.x)
        {
            isRight = false;
            isLeft = true;
            isCenter = false;
        }
        else if (transform.position.x == GameManager.instance.player.transform.position.x)
        {
            isRight = false;
            isLeft = false;
            isCenter = true;
        }
    }

    protected virtual void Move()
    {

    }
}
