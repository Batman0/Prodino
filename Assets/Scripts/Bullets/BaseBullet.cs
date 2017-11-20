using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseBullet : MonoBehaviour
{
    protected float speed;
    public Collider sideCollider;
    public Collider topCollider;
    protected Quaternion? sidescrollRotation;

    protected virtual void OnEnable()
    {
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
        if (!sidescrollRotation.HasValue)
        {
            if (GameManager.instance.currentGameMode == GameMode.SIDESCROLL)
            {
                sidescrollRotation = transform.rotation;
            }       
        }
        ChangePerspective();
    }

    protected abstract void ChangePerspective();

    private void OnDisable()
    {
        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
    }
}
