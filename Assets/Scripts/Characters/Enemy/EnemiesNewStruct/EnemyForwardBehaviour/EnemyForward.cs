using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyForward : Enemy
{
	//public override void Update()
	//{
	//	base.Update ();	
	//}

    public virtual void FixedUpdate()
    {
        Move();
    }

    public override void Move()
    {
        base.Move();

            transform.Translate(Vector3.forward * xSpeedAdjustable * Time.fixedDeltaTime, Space.Self);

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