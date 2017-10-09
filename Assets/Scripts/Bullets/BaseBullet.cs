using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBullet : MonoBehaviour
{
    //[HideInInspector]
    /// <summary>
    /// How much far must the bullet be from the near clipping plane to be destroyed?
    /// </summary>
    //public float destructionMargin;
    //[HideInInspector]
    //public float speed;
    //[HideInInspector]
    //public Vector3 originalPos;
    public Collider sideCollider;
    public Collider topCollider;
    protected Vector3 direction;

    protected virtual void Start()
    {
        direction = transform.forward;

        if (GameManager.instance.currentGameMode == GameMode.SIDESCROLL)
        {
            if (!sideCollider.enabled || topCollider.enabled)
            {
                topCollider.enabled = false;
                sideCollider.enabled = true;
            }
        }
        else
        {
            if (!topCollider.enabled || sideCollider.enabled)
            {
                sideCollider.enabled = false;
                topCollider.enabled = true;
            }
        }
    }

    protected virtual void Update()
    {
        Move();
        ChangePerspective();
    }

    protected void DestroyGameobject(float destructionMargin)
    {
        if (transform.position.x < Register.instance.xMin - destructionMargin || transform.position.x > Register.instance.xMax + destructionMargin || transform.position.y < Register.instance.yMin - destructionMargin || transform.position.y > Register.instance.yMax + destructionMargin)
        {
            Destroy(gameObject);
        }
    }

    void ChangePerspective()
    {
        if (GameManager.instance.transitionIsRunning)
        {
            if (GameManager.instance.currentGameMode == GameMode.TOPDOWN)
            {
                if (!sideCollider.enabled)
                {
                    topCollider.enabled = false;
                    sideCollider.enabled = true;
                }
            }
            else
            {
                if (!topCollider.enabled)
                {
                    sideCollider.enabled = false;
                    topCollider.enabled = true;
                }
            }
        }
    }

    protected virtual void Move()
    {

    }
}
