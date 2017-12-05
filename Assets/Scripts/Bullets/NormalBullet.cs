using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NormalBullet : BaseBullet
{
    [SerializeField]
    protected float destructionMargin;
    protected float xMin, xMax, yMin, yMax, zMin, zMax;
    protected Vector3 direction;
    protected Register register;
    protected int myTargetLayer;

    protected virtual void Awake()
    {
        register = Register.instance;
        xMin = register.xMin;
        xMax = register.xMax;
        yMin = register.yMin;
        yMax = register.yMax;
        zMin = register.zMin;
        zMax = register.zMax;
        myTargetLayer = Register.instance.PlayerLayer;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        direction = transform.forward;
    }

    protected override void Update()
    {
        base.Update();
        DisableGameobject();
    }

    private void FixedUpdate()
    {
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

    protected virtual void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.layer == myTargetLayer)
        {
            StartCoroutine(DeactivateBullet());
        }
    }

    protected virtual IEnumerator DeactivateBullet()
    {
        yield return new WaitForEndOfFrame();
        gameObject.SetActive(false);
    }

    protected virtual void DisableGameobject()
    {
        if (transform.position.x < xMin - destructionMargin || transform.position.x > xMax + destructionMargin || transform.position.y < yMin - destructionMargin || transform.position.y > yMax + destructionMargin || transform.position.z < zMin - destructionMargin || transform.position.z > zMax + destructionMargin)
        {
            gameObject.SetActive(false);
        }
    }
}
