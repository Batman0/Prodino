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
    public Vector3 originalPos;

    protected virtual void Start()
    {
        Debug.Log("SONO UNA MIGNOTTA");
        Register.instance.numberOfTransitableObjects++;
    }

    void Update()
    {
        Move();
        DestroyGameobject();
        ChangePerspective();
    }

    /*void OnTriggerEnter(Collider other)
    {
        if (gameObject.tag == "PlayerBullet" && other.gameObject.tag == "Enemy")
        {
           other.gameObject.
        }
    }*/

    void OnDestroy()
    {
        Register.instance.numberOfTransitableObjects--;
    }

    void DestroyGameobject()
    {
        if (transform.position.x < GameManager.instance.leftBound.x - destructionMargin || transform.position.x > GameManager.instance.rightBound.x + destructionMargin || transform.position.y < GameManager.instance.downBound.y - destructionMargin || transform.position.y > GameManager.instance.upBound.y + destructionMargin)
        {
            Destroy(gameObject);
        }
    }

    void ChangePerspective()
    {
        if (Register.instance.canStartTransitions)
        {
            switch (GameManager.instance.currentGameMode)
            {
                case GameMode.SIDESCROLL:
                        if (transform.position != new Vector3(transform.position.x, transform.position.y, originalPos.z))
                        {
                            transform.position = new Vector3(transform.position.x, transform.position.y, originalPos.z);
                        }
                    break;
                case GameMode.TOPDOWN:
                        if (transform.position != new Vector3(transform.position.x, originalPos.y, transform.position.z))
                        {
                            transform.position = new Vector3(transform.position.x, originalPos.y, transform.position.z);
                        }
                    break;
            }
            Register.instance.translatedObjects++;
            if (Register.instance.translatedObjects == Register.instance.numberOfTransitableObjects)
            {
                Register.instance.translatedObjects = 0;
                Register.instance.canStartTransitions = false;
            }
        }
        else if (Register.instance.canEndTransitions)
        {
            switch (GameManager.instance.currentGameMode)
            {
                case GameMode.TOPDOWN:
                        if (transform.position != new Vector3(transform.position.x, GameManager.instance.playerBulletSpawnPos.y, originalPos.z))
                        {
                            transform.position = new Vector3(transform.position.x, GameManager.instance.playerBulletSpawnPos.y, originalPos.z);
                        }
                    break;
                case GameMode.SIDESCROLL:
                        if (transform.position != new Vector3(transform.position.x, originalPos.y, 0))
                        {
                            transform.position = new Vector3(transform.position.x, originalPos.y, 0);
                        }
                    break;
            }
            Register.instance.translatedObjects++;
            if (Register.instance.translatedObjects == Register.instance.numberOfTransitableObjects)
            {
                Register.instance.translatedObjects = 0;
                Register.instance.canEndTransitions = false;
            }
        }
    }

    protected virtual void Move()
    {

    }
}
