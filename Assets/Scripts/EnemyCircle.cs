using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCircle : EnemyBehaviour
{
    public float radius;
    private float distance;
    private Vector3 offset;
    public float lifeTime = 5;

    void Start()
    {
        radius = Camera.main.orthographicSize;
        offset = transform.position;
        Destroy(this.gameObject, lifeTime);
    }

    protected override void Move()
    {
        switch (GameManager.instance.cameraState)
        {
            case State.SIDESCROLL:
                transform.position = new Vector3(radius * Mathf.Cos(Time.time * speed) + offset.x, radius * Mathf.Sin(Time.time * speed) + offset.y, 0);
                break;
            case State.TOPDOWN:
                transform.position = new Vector3(radius * Mathf.Cos(Time.time * speed) + offset.x, 0, radius * Mathf.Sin(Time.time * speed) + offset.z);
                break;
        }
    }

}
