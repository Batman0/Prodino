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
    private float distanceZSurplus = 20;
    [HideInInspector]
    public float playerBulletSpawnpoointY;
    public GameObject[] backgrounds;

    [Header("Aim")]
    private float intersectionPoint;
    private Vector3 aimVector;
    private Plane? sidescrollPlane;
    private Plane? topDownPlane;
    private Ray aimRay;
    private GameObject aimTransform;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        playerBulletSpawnpoointY = Register.instance.player.bulletSpawnPointLx.position.y;
        aimTransform = Instantiate(Register.instance.aimTransform, Vector3.zero, Register.instance.aimTransform.transform.rotation) as GameObject;
        Register.instance.player.aimTransform = aimTransform;
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
        MoveBackgrounds(backgrounds, Vector3.left, backgroundSpeed);

        Aim();
    }


    void MoveBackgrounds(GameObject[] backgrounds, Vector3 moveVector, float speed)
    {
        foreach (GameObject background in backgrounds)
        {
            background.transform.Translate(moveVector * speed * Time.deltaTime, Space.World);
        }
    }

    void Aim()
    {
        if (sidescrollPlane == null && GameManager.instance.currentGameMode == GameMode.SIDESCROLL)
        {
            sidescrollPlane = new Plane(-Camera.main.transform.forward, Vector3.zero);
        }
        if (topDownPlane == null && GameManager.instance.currentGameMode == GameMode.TOPDOWN)
        {
            topDownPlane = new Plane(-Camera.main.transform.forward, Vector3.zero);
        }
        if (topDownPlane != null && GameManager.instance.currentGameMode == GameMode.TOPDOWN)
        {
            aimRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (topDownPlane.Value.Raycast(aimRay, out intersectionPoint))
            {
                aimVector = aimRay.GetPoint(intersectionPoint);
                aimTransform.transform.position = aimVector;
            }
        }
        if (sidescrollPlane != null && GameManager.instance.currentGameMode == GameMode.SIDESCROLL)
        {
            aimRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (sidescrollPlane.Value.Raycast(aimRay, out intersectionPoint))
            {
                aimVector = aimRay.GetPoint(intersectionPoint);
                aimTransform.transform.position = aimVector;
            }
        }
    }
}
