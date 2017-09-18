using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCircle : Enemy
{
    public float radius;
    private float distance;
    private Vector3 offset;
    public float lifeTime = 5;

    void Start()
    {
        radius = Camera.main.orthographicSize;
        offset = transform.position;
        Destroy(gameObject, lifeTime);
    }

    protected override void Move()
    {
        switch (GameManager.instance.currentGameMode)
        {
            case GameMode.SIDESCROLL:
                if (isRight)
                {
                    transform.position = new Vector3(radius * Mathf.Cos(Time.time * speed) + originalPos.x, radius * Mathf.Sin(Time.time * speed) + originalPos.y, 0);
                }
                else
                {
                    transform.position = new Vector3(-radius * Mathf.Cos(Time.time * speed) + originalPos.x, radius * Mathf.Sin(Time.time * speed) + originalPos.y, 0);
                }
                //transform.position = new Vector3(radius * Mathf.Cos(Time.time * speed) + offset.x, radius * Mathf.Sin(Time.time * speed) + offset.y, 0);
                break;
            case GameMode.TOPDOWN:
                if (isRight)
                {
                    transform.position = new Vector3(radius * Mathf.Cos(Time.time * speed) + originalPos.x, 0, radius * Mathf.Sin(Time.time * speed) + originalPos.z);
                }
                else
                {
                    transform.position = new Vector3(-radius * Mathf.Cos(Time.time * speed) + originalPos.x, 0, radius * Mathf.Sin(Time.time * speed) + originalPos.z);
                }
                break;
        }
    }

}
