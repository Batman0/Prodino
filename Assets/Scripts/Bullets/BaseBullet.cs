using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class BaseBullet : MonoBehaviour
{
    [SerializeField]
    protected float speed;
    public Collider sideCollider;
    public Collider topCollider;
    protected Quaternion? sidescrollRotation;

    protected virtual void OnEnable()
    {
        EventManager.changeState += ChangeGameState;
        if (GameManager.instance.currentGameMode == GameMode.SIDESCROLL)
        {
            topCollider.enabled = false;
            sideCollider.enabled = true;
        }
        else
        {
            sideCollider.enabled = false;
            topCollider.enabled = true;
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
    }

    protected abstract void ChangeGameState(GameMode currentState);

    protected void OnDisable()
    {
        EventManager.changeState -= ChangeGameState;
        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
    }
}
