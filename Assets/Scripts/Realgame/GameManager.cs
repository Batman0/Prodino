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
    public GameObject playerPrefab;
    public Transform playerStartPos;
    public GameMode currentGameMode;
    public bool transitionIsRunning;
    public float backgroundSpeed;
    public PlayerControllerJump player;
    public Vector3 leftBound;
    public Vector3 rightBound;
    public Vector3 playerBulletSpawnPos;
    public GameObject[] backgrounds;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        Time.timeScale = 0.3f;
        RespawnPlayer(playerStartPos);
        leftBound = Camera.main.ScreenToWorldPoint(new Vector3(0, Camera.main.pixelHeight / 2, Camera.main.nearClipPlane));
        rightBound = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth, Camera.main.pixelHeight / 2, Camera.main.nearClipPlane));
    }

    void Update()
    {
        MoveBackgrounds(backgrounds, Vector3.left, backgroundSpeed);
    }

    void RespawnPlayer(Transform restartPos)
    {
        GameObject playerGO = Instantiate(playerPrefab, restartPos.position, playerPrefab.transform.rotation) as GameObject;
        player = playerGO.GetComponent<PlayerControllerJump>();
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
