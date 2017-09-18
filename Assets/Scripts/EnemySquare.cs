using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySquare : Enemy {
    //public float length;
    //public float distance;
    //   private Vector3 offset;
    //   //public float lifeTime = 5;
    //   private float timer = 0.0f;
    //   public float maxTimer = 2.0f;
    //   private float angleSquare = 0;
    //   private bool straightwayIsRunning=false;
    [HideInInspector]
    public Transform[] targets;
    [HideInInspector]
    public float[] speeds;
    [HideInInspector]
    public float[] waitingTimes;
    //public Vector3? actualTarget;
    public float pathLength;
    private float pathDone;
    public float stopTime;
    private float stopTimer;
    private int pathIndex;
    //private bool isFollowingPath;


 //   void Start()
	//{
	//	//length = Camera.main.orthographicSize;
	//	offset = transform.position;
	//	//Destroy(gameObject, lifeTime);
	//}

    protected override void Move()
    {
        if (/*actualTarget != null && */pathDone < pathLength)
        {
            switch (GameManager.instance.currentGameMode)
            {
                case GameMode.SIDESCROLL:
                    transform.position = Vector3.MoveTowards(transform.position, new Vector3(targets[pathIndex].position.x, targets[pathIndex].position.y, 0), speed * Time.deltaTime);
                    break;
                case GameMode.TOPDOWN:
                    transform.position = Vector3.MoveTowards(transform.position, new Vector3(targets[pathIndex].position.x, GameManager.instance.playerBulletSpawnPos.y, targets[pathIndex].position.z), speeds[pathIndex] * Time.deltaTime);
                    break;
            }
            pathDone += speeds[pathIndex] * Time.deltaTime;
        }
        else
        {
            if (/*actualTarget != null && */stopTimer < waitingTimes[pathIndex])
            {
                stopTimer += Time.deltaTime;
            }
            else
            {
                //actualTarget = targets[pathIndex].position;
                if (pathIndex < targets.Length - 1)
                {
                    pathIndex++;
                }
                else
                {
                    pathIndex = 0;
                }
                pathDone = 0.0f;
                stopTimer = 0.0f;
            }
        }

        //if (!isFollowingPath)
        //{
        //    isFollowingPath = true;
        //    FollowPath();
        //}
    }

    //protected override void ChangePerspective()
    //{
    //    if (Register.instance.canStartEnemyTransition)
    //    {
    //        switch (GameManager.instance.currentGameMode)
    //        {
    //            case GameMode.SIDESCROLL:
    //                if (transform.position != new Vector3(transform.position.x, transform.position.y, originalPos.z))
    //                {
    //                    transform.position = new Vector3(transform.position.x, transform.position.y, originalPos.z);
    //                }
    //                break;
    //            case GameMode.TOPDOWN:
    //                if (transform.position != new Vector3(transform.position.x, originalPos.y, transform.position.z))
    //                {
    //                    transform.position = new Vector3(transform.position.x, originalPos.y, transform.position.z);
    //                }
    //                break;
    //        }
    //        Register.instance.translatedEnemies++;
    //        if (Register.instance.translatedEnemies == Register.instance.numberOfEnemies)
    //        {
    //            Register.instance.translatedEnemies = 0;
    //            Register.instance.canStartEnemyTransition = false;
    //        }
    //    }
    //    else if (Register.instance.canEndEnemyTransition)
    //    {
    //        switch (GameManager.instance.currentGameMode)
    //        {
    //            case GameMode.TOPDOWN:
    //                if (transform.position != new Vector3(transform.position.x, GameManager.instance.playerBulletSpawnPos.y, originalPos.z))
    //                {
    //                    transform.position = new Vector3(transform.position.x, GameManager.instance.playerBulletSpawnPos.y, originalPos.z);
    //                }
    //                break;
    //            case GameMode.SIDESCROLL:
    //                if (transform.position != new Vector3(transform.position.x, originalPos.y, 0))
    //                {
    //                    transform.position = new Vector3(transform.position.x, originalPos.y, 0);
    //                }
    //                break;
    //        }
    //        Register.instance.translatedEnemies++;
    //        if (Register.instance.translatedEnemies == Register.instance.numberOfEnemies)
    //        {
    //            Register.instance.translatedEnemies = 0;
    //            Register.instance.canEndEnemyTransition = false;
    //        }
    //    }
    //}

   
}
