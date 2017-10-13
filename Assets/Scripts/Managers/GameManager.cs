using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameMode
{
    SIDESCROLL,
    TOPDOWN
}

[System.Serializable]
public struct Background
{
    public GameObject background;
    public float speed;
}

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [HideInInspector]
    public GameMode currentGameMode;
    [HideInInspector]
    public bool transitionIsRunning;
    private float distanceZSurplus = 20;
    [HideInInspector]
    public float playerBulletSpawnpoointY;
    public Background[] backgrounds;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        //Register.instance.player.startPosition = Register.instance.player.transform.position;
        //Register.instance.player.aimTransform = Register.instance.aimTransform;
        playerBulletSpawnpoointY = Register.instance.player.bulletSpawnPoint.position.y;
        Register.instance.xMin = Camera.main.ScreenToWorldPoint(new Vector3(0, Camera.main.pixelHeight / 2, Camera.main.nearClipPlane + distanceZSurplus)).x;
        Register.instance.xMax = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth, Camera.main.pixelHeight / 2, Camera.main.nearClipPlane + distanceZSurplus)).x;
        Register.instance.yMin = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth / 2, 0, Camera.main.nearClipPlane + distanceZSurplus)).y;
        Register.instance.yMax = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth / 2, Camera.main.pixelHeight, Camera.main.nearClipPlane + distanceZSurplus)).y;
    }

    void Update()
    {
        if (Register.instance.zMin == null && Register.instance.zMax == null)
        {
            if (currentGameMode == GameMode.TOPDOWN)
            {
                Register.instance.zMin = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth / 2, 0, Camera.main.nearClipPlane + distanceZSurplus)).z;
                Register.instance.zMax = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth / 2, Camera.main.pixelHeight, Camera.main.nearClipPlane + distanceZSurplus)).z;
            }
        }
        MoveBackgrounds(Vector3.left);
        //Debug.Log(Time.time.ToString("#.##"));
    }

    //void RespawnPlayer(Transform restartPos)
    //{
    //    GameObject playerGO = Instantiate(playerPrefab, restartPos.position, playerPrefab.transform.rotation) as GameObject;
    //    Register.instance.player = playerGO.GetComponent<PlayerController>();
    //    Register.instance.player.startPosition = playerStartPos.position;
    //    Register.instance.player.aimTransform = Register.instance.aimTransform;
    //    playerBulletSpawnpoointY = Register.instance.player.bulletSpawnPoint.position.y;
    //}

    void MoveBackgrounds(Vector3 moveVector)
    {
        foreach (Background item in backgrounds)
        {
            item.background.transform.Translate(moveVector * item.speed * Time.deltaTime, Space.World);
        }
    }
}
