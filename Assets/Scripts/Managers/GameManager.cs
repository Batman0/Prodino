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
    public float delayToRespawnEnemy;
    [Header("Bosses")]
    public bool isBossAlive;

    public float currentTime;
    
	public Transform playerHandle;

    private void Awake()
    {
		float distanceFromCamera = playerHandle.position.z - Camera.main.transform.position.z;
        instance = this;
        register = Register.instance;
		register.xMin = Camera.main.ViewportToWorldPoint(new Vector3(0, 0.5f, distanceFromCamera )).x;
		register.xMax = Camera.main.ViewportToWorldPoint(new Vector3(1, 0.5f, distanceFromCamera )).x;
		register.yMin = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0, distanceFromCamera )).y;
		register.yMax = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 1, distanceFromCamera )).y;
        register.zMin = register.yMin;
        register.zMax = register.yMax;
    }

    void Start()
    {
        playerBulletSpawnpointY = Register.instance.player.bulletSpawnPoints[0].position.y;
    }

    void Update()
    {
        if (!isBossAlive)
        {
            currentTime += Time.deltaTime;
        }

        if (currentGameMode == GameMode.TOPDOWN)
        {
            Register.instance.zMin = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth / 2, 0, Camera.main.farClipPlane + distanceZSurplus)).z;
            Register.instance.zMax = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth / 2, Camera.main.pixelHeight, Camera.main.farClipPlane + distanceZSurplus)).z;
        }
        MoveBackgrounds(Vector3.left);

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
