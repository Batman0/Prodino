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
    public bool isLerpingCamera = false;
    public State cameraState;

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
        if (Input.GetKeyDown(KeyCode.Space) && !isLerpingCamera)
        {
            isLerpingCamera = true;
        }
    }
}
