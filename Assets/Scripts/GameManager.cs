using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum State
{
    SIDESCROLL,
    TOPDOWN
}

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [HideInInspector]
    public bool canChangeState = false;
    //public bool canMoveEnemies;
    public float backgroundSpeed;
    public State cameraState;
    [HideInInspector]
    public Vector3 playerPosition;
    public GameObject[] backgrounds;

    void Awake()
    {
        instance = this;
    }

    // Use this for initialization
    void Start ()
    {
        cameraState = State.SIDESCROLL;
	}

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !canChangeState)
        {
            canChangeState = true;
        }
        MoveBackgrounds(backgrounds, Vector3.left, backgroundSpeed);
    }

    void MoveBackgrounds(GameObject[] backgrounds, Vector3 moveVector, float speed)
    {
        foreach(GameObject background in backgrounds)
        {
            background.transform.Translate(moveVector * speed * Time.deltaTime, Space.World);
        }
    }
}
