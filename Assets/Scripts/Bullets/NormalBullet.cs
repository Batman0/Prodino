using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NormalBullet : BaseBullet
{
    
    protected float destructionMargin;
    protected float xMin, xMax, yMin, yMax, zMin, zMax;
    protected Vector3 direction;
    private Register register;

    protected virtual void Awake()
    {
        register = Register.instance;
        xMin = register.xMin;
        xMax = register.xMax;
        yMin = register.yMin;
        yMax = register.yMax;
        zMin = register.zMin.Value;
        zMax = register.zMax.Value;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        direction = transform.forward;
    }

    protected override void Update()
    {
        base.Update();
        Move();
    }

    protected override void ChangePerspective()
    {
        if (GameManager.instance.transitionIsRunning)
        {
            if (GameManager.instance.currentGameMode == GameMode.TOPDOWN)
            {
                if (!sideCollider.enabled)
                {
                    topCollider.enabled = false;
                    sideCollider.enabled = true;
                    if (sidescrollRotation.HasValue)
                    {
                        transform.rotation = sidescrollRotation.Value;
                    }
                }
            }
            else
            {
                if (!topCollider.enabled)
                {
                    sideCollider.enabled = false;
                    topCollider.enabled = true;
                    transform.rotation = Quaternion.identity;
                }
            }
        }
    }

    protected abstract void Move();

    protected virtual void DisableGameobject()
    {
        if (transform.position.x < xMin - destructionMargin || transform.position.x > xMax + destructionMargin || transform.position.y < yMin - destructionMargin || transform.position.y > yMax + destructionMargin || transform.position.z < zMin - destructionMargin || transform.position.z > zMax + destructionMargin)
        {
            gameObject.SetActive(false);
        }
    }
}
