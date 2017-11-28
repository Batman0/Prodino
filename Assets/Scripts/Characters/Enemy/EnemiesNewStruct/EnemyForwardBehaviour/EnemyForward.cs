using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyForward : Enemy
{
    protected float speed = 0;
    protected float destructionMargin;

	public override void Update()
	{
		base.Update ();	
	}

    public virtual void FixedUpdate()
    {
        Move();
    }

    public override void Move()
    {
        base.Move();

            transform.Translate(Vector3.forward * speed * Time.fixedDeltaTime, Space.Self);

            if (isRight)
            {
                if (transform.position.x <= xMin - destructionMargin)
                {
                    gameObject.SetActive(false);
                }
            }
            else
            {
                if (transform.position.x >= xMax + destructionMargin)
                {
                    gameObject.SetActive(false);
                }
            }
    }

}