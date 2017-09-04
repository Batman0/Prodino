using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySquare : Enemy {
	public float length;
	public float distance;
    private Vector3 offset;
    //public float lifeTime = 5;
    private float timer = 0.0f;
    public float maxTimer = 2.0f;
    private float angleSquare = 0;
    private bool straightwayIsRunning=false;


    void Start()
	{
		//length = Camera.main.orthographicSize;
		offset = transform.position;
		//Destroy(gameObject, lifeTime);
	}

	protected override void Move()
	{
		switch (GameManager.instance.cameraState)
		{
		case State.SIDESCROLL:
                if(!straightwayIsRunning)
                {
                    straightwayIsRunning = true;
                    StartCoroutine("STRAIGHTWAY");
                }

			/*transform.position = new Vector3(
				length* Mathf.Cos(Time.time * speed) / Mathf.Max(Mathf.Abs(Mathf.Sin(Time.time * speed)),Mathf.Abs(Mathf.Cos(Time.time * speed))) + offset.x, 
				length* Mathf.Sin(Time.time * speed)/ Mathf.Max(Mathf.Abs(Mathf.Cos(Time.time * speed)),Mathf.Abs(Mathf.Sin(Time.time * speed))) + offset.y, 
				0);*/
			break;
		case State.TOPDOWN:
			//transform.position = new Vector3(length * Mathf.Cos(Time.time * speed) + offset.x, 0, length * Mathf.Sin(Time.time * speed) + offset.z);
			break;
		}
	}

    IEnumerator STRAIGHTWAY()
    {
            float x = (length * Mathf.Cos(Time.time * speed) / Mathf.Max(Mathf.Abs(Mathf.Sin(Time.time * speed)), Mathf.Abs(Mathf.Cos(Time.time * speed))) + offset.x);
            transform.position = new Vector3(x, transform.position.y, 0);
            distance += x;
            yield return null;
    }
}
