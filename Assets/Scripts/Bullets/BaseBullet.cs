using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBullet : MonoBehaviour
{
    /// <summary>
    /// How much far must the bullet be from the near clipping plane to be destroyed?
    /// </summary>
    public float destructionMargin;
    public float speed;

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
        if (transform.position.x < GameManager.instance.leftBound.x - destructionMargin || transform.position.x > GameManager.instance.rightBound.x + destructionMargin || transform.position.y < GameManager.instance.downBound.y - destructionMargin || transform.position.y > GameManager.instance.upBound.y + destructionMargin)
        {
            Destroy(gameObject);
        }
    }

    protected virtual void Move()
    {

    }
}
