﻿using System.Collections;
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
    public float backgroundSpeed;
    public State cameraState;
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
        if (Input.GetKeyDown(KeyCode.Space) && !isLerpingCamera)
        {
            isLerpingCamera = true;
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
