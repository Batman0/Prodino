﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameMode
{
    SIDESCROLL,
    TOPDOWN
}
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject playerPrefab;
    public Transform playerStartPos;
    public GameMode currentGameMode;
    public bool transitionIsRunning;
    public float backgroundSpeed;
    public PlayerController player;
    public Vector3 leftBound;
    public Vector3 rightBound;
    public Vector3 downBound;
    public Vector3 upBound;
    public Vector3 playerBulletSpawnPos;
    public GameObject[] backgrounds;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        RespawnPlayer(playerStartPos);
        leftBound = Camera.main.ScreenToWorldPoint(new Vector3(0, Camera.main.pixelHeight / 2, Camera.main.nearClipPlane));
        rightBound = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth, Camera.main.pixelHeight / 2, Camera.main.nearClipPlane));
        downBound = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth / 2, 0, Camera.main.nearClipPlane));
        upBound = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth / 2, Camera.main.pixelHeight, Camera.main.nearClipPlane));
    }

    void Update()
    {
        MoveBackgrounds(backgrounds, Vector3.left, backgroundSpeed);
        //Debug.Log(Time.time.ToString("#.##"));
    }

    void RespawnPlayer(Transform restartPos)
    {
        GameObject playerGO = Instantiate(playerPrefab, restartPos.position, playerPrefab.transform.rotation) as GameObject;
        player = playerGO.GetComponent<PlayerController>();
        player.startPosition = playerStartPos.position;
        player.aimTransform = Register.instance.aimTransform;
        playerBulletSpawnPos = player.bulletSpawnPoint.position;
    }

    void MoveBackgrounds(GameObject[] backgrounds, Vector3 moveVector, float speed)
    {
        foreach (GameObject background in backgrounds)
        {
            background.transform.Translate(moveVector * speed * Time.deltaTime, Space.World);
        }
    }
}