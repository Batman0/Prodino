using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBullet : MonoBehaviour
{
    public GameObject bulletGO;
    [HideInInspector]
    public bool iCanRotate;
    [HideInInspector]
    /// <summary>
    /// How much far must the bullet be from the near clipping plane to be destroyed?
    /// </summary>
    public float destructionMargin;
    [HideInInspector]
    public float speed;
    //[HideInInspector]
    //public Vector3 originalPos;

    protected virtual void Start()
    {
        transform.rotation = Quaternion.identity;
        transform.Rotate(Vector3.up, 90, Space.World);
        if (GameManager.instance.currentGameMode == GameMode.TOPDOWN)
        {
            transform.Rotate(Vector3.forward, 90, Space.Self);
        }
        Register.instance.numberOfTransitableObjects++;
        iCanRotate = true;
    }

    void Update()
    {
        Move();
        DestroyGameobject();
        ChangePerspective();
    }

    void OnDestroy()
    {
        Register.instance.numberOfTransitableObjects--;
    }

    void DestroyGameobject()
    {
        if (transform.position.x < Register.instance.xMin - destructionMargin || transform.position.x > Register.instance.xMax + destructionMargin || transform.position.y < Register.instance.yMin - destructionMargin || transform.position.y > Register.instance.yMax + destructionMargin)
        {
            Destroy(bulletGO);
        }
    }

    void ChangePerspective()
    {
        if (Register.instance.bulletsCanRotate && iCanRotate)
        {
            transform.Rotate(Vector3.forward, 90, Space.Self);
            Register.instance.translatedObjects++;
            iCanRotate = false;
        }
        if (Register.instance.translatedObjects == Register.instance.numberOfTransitableObjects)
        {
            Register.instance.translatedObjects = 0;
            Register.instance.bulletsCanRotate = false;
        }
        if (!Register.instance.bulletsCanRotate && !iCanRotate)
        {
            iCanRotate = true;
        }
    }

    protected virtual void Move()
    {

    }
}
