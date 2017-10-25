using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBullet : MonoBehaviour
{
    public Collider sideCollider;
    public Collider topCollider;
    protected Vector3 direction;
    protected Quaternion? sidescrollRotation;

    protected virtual void OnEnable()
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

    //protected virtual void Start()
    //{
    //    //transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);

    //}

    protected virtual void Update()
    {
        if (!sidescrollRotation.HasValue)
            if (GameManager.instance.currentGameMode == GameMode.SIDESCROLL)
                sidescrollRotation = transform.rotation;
        ChangePerspective();
    }

    protected virtual void DestroyGameobject(float destructionMargin)
    {
        if (transform.position.x < Register.instance.xMin - destructionMargin || transform.position.x > Register.instance.xMax + destructionMargin || transform.position.y < Register.instance.yMin - destructionMargin || transform.position.y > Register.instance.yMax + destructionMargin)
        {
            gameObject.SetActive(false);
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
                    if (sidescrollRotation.HasValue)
                        transform.rotation = sidescrollRotation.Value;
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

    protected virtual void Move()
    {

    }

    protected virtual void OnDisable()
    {
        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
    }
}
