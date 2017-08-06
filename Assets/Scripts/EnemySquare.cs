using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySquare : Enemy {
	public float length;
	private float distance;
    private Vector3 offset;
	public float lifeTime = 5;

	void Start()
	{
		//length = Camera.main.orthographicSize;
		offset = transform.position;
		Destroy(gameObject, lifeTime);
	}

	protected override void Move()
	{
		switch (GameManager.instance.cameraState)
		{
		case State.SIDESCROLL:
			transform.position = new Vector3(
				length* Mathf.Cos(Time.time * speed) / Mathf.Max(Mathf.Abs(Mathf.Sin(Time.time * speed)),Mathf.Abs(Mathf.Cos(Time.time * speed))) + offset.x, 
				length* Mathf.Sin(Time.time * speed)/ Mathf.Max(Mathf.Abs(Mathf.Cos(Time.time * speed)),Mathf.Abs(Mathf.Sin(Time.time * speed))) + offset.y, 
				0);
			break;
		case State.TOPDOWN:
			transform.position = new Vector3(length * Mathf.Cos(Time.time * speed) + offset.x, 0, length * Mathf.Sin(Time.time * speed) + offset.z);
			break;
		}
	}
}
