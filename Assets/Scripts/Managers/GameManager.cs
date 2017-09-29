using System.Collections;
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
    [HideInInspector]
    public GameMode currentGameMode;
    [HideInInspector]
    public bool transitionIsRunning;
    public float backgroundSpeed;
    [HideInInspector]
    public Vector3 leftBound;
    [HideInInspector]
    public Vector3 rightBound;
    [HideInInspector]
    public Vector3 downBound;
    [HideInInspector]
    public Vector3 upBound;
    [HideInInspector]
    public float playerBulletSpawnpoointY;
    public GameObject[] backgrounds;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        Register.instance.player.startPosition = Register.instance.player.transform.position;
        Register.instance.player.aimTransform = Register.instance.aimTransform;
        playerBulletSpawnpoointY = Register.instance.player.bulletSpawnPoint.position.y;
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

    //void RespawnPlayer(Transform restartPos)
    //{
    //    GameObject playerGO = Instantiate(playerPrefab, restartPos.position, playerPrefab.transform.rotation) as GameObject;
    //    Register.instance.player = playerGO.GetComponent<PlayerController>();
    //    Register.instance.player.startPosition = playerStartPos.position;
    //    Register.instance.player.aimTransform = Register.instance.aimTransform;
    //    playerBulletSpawnpoointY = Register.instance.player.bulletSpawnPoint.position.y;
    //}

    void MoveBackgrounds(GameObject[] backgrounds, Vector3 moveVector, float speed)
    {
        foreach (GameObject background in backgrounds)
        {
            background.transform.Translate(moveVector * speed * Time.deltaTime, Space.World);
        }
    }
}
