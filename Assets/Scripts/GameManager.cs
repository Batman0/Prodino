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
    public bool cameraTransitionIsRunning = false;
    public float backgroundSpeed;
    public State cameraState;
    [HideInInspector]
    public Vector3 playerPosition;
    public Vector3 leftBound;
    public Vector3 rightBound;
    public GameObject[] backgrounds;

    void Awake()
    {
        instance = this;
        cameraState = State.SIDESCROLL;
    }

    // Use this for initialization
    void Start ()
    {
        leftBound = Camera.main.ScreenToWorldPoint(new Vector3(0, Camera.main.pixelHeight / 2, Camera.main.nearClipPlane));
        rightBound = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth, Camera.main.pixelHeight / 2, Camera.main.nearClipPlane));
	}

    // Update is called once per frame
    void Update()
    {
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
