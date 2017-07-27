using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCircle : EnemyBehaviour
{
    public float radius;
    private float distance;
    private Vector3 offset;

    void Start()
    {
        offset = transform.position;
    }

    protected override void Move()
    {
        switch (CameraController.instance.myState)
        {
            case CameraState.SIDESCROLL:
                transform.position = new Vector3(radius * Mathf.Cos(Time.time * speed) + offset.x, radius * Mathf.Sin(Time.time * speed) + offset.y, 0);
                break;
            case CameraState.TOPDOWN:
                transform.position = new Vector3(radius * Mathf.Cos(Time.time * speed) + offset.x, 0, radius * Mathf.Sin(Time.time * speed) + offset.z);
                break;
        }
    }

}
