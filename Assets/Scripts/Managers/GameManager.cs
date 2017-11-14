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
    private Register register;
    [HideInInspector]
    public GameMode currentGameMode;
    [HideInInspector]
    public bool transitionIsRunning;
    private float distanceZSurplus = 20;
    [HideInInspector]
    public float playerBulletSpawnpointY;
    public Background[] backgrounds;
    public bool Isbossfight = false;

    private void Awake()
    {
        instance = this;
        register = Register.instance;
        register.xMin = Camera.main.ScreenToWorldPoint(new Vector3(0, Camera.main.pixelHeight / 2, Camera.main.nearClipPlane + distanceZSurplus)).x;
        register.xMax = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth, Camera.main.pixelHeight / 2, Camera.main.nearClipPlane + distanceZSurplus)).x;
        register.yMin = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth / 2, 0, Camera.main.nearClipPlane + distanceZSurplus)).y;
        register.yMax = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth / 2, Camera.main.pixelHeight, Camera.main.nearClipPlane + distanceZSurplus)).y;
        register.zMin = register.yMin;
        register.zMax = register.yMax;
    }

    void Start()
    {
        playerBulletSpawnpointY = Register.instance.player.bulletSpawnPoints[0].position.y;
        //Register.instance.player.startPosition = Register.instance.player.transform.position;
        //Register.instance.player.aimTransform = Register.instance.aimTransform;
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

    public void BossFight()
    {
        if(Isbossfight)
        {
            Time.timeScale = 0.0f;
        }
    }
}
