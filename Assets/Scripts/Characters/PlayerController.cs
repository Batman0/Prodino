using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [HideInInspector]
    public Vector3 startPosition;
    [SerializeField]
    private GameObject meshGO;
    public float speed = 5.0f;
    public float jumpForce = 5.0f;
    public float upRotationAngle;
    public float downRotationAngle;
    private int enemyLayer = 12;
    public float jumpCheckRayLength;
    public float groundCheckRayLength;
    private float controllerDeadZone = 0.1f;
    //private float CheckGroundRaycastMargin = 1;
    [HideInInspector]
    public Transform aimTransform;
    public Transform bulletSpawnPoint;
    public float fireRatio = 0.10f;
    private float fireTimer;
    public float respawnTimer = 0.5f;
    public float gravity;
    public float glideSpeed;
    private Quaternion sideScrollerRotation;
    private Quaternion bulletSpawnPointStartRotation;
    private const string playerBulletTag = "PlayerBullet";
    private RaycastHit hit;
    private float angle;
    public float meleeDistance;
    private Rigidbody rb;
    public LayerMask groundMask;
    private bool canShoot = true;
    public bool canJump = true;
    private bool thereIsGround;
    private bool isDead;
    private float horizontal;
    public Transform landmark;

    //private SkinnedMeshRenderer skinnedMeshRen;

    [Header("Boundaries")]
    public float sideXMin;
    public float sideXMax;
    public float sideYMin = 5.5f;
    public float sideYMax;
    public float topXMin, topXMax, topZMin, topZMax;

    [Header("Animations")]
    public Animator ani;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        //skinnedMeshRen = GetComponentInChildren<SkinnedMeshRenderer>();
    }

    void Start()
    {
        sideScrollerRotation = transform.rotation;
        bulletSpawnPointStartRotation = bulletSpawnPoint.rotation;
    }
    void Update()
    {
        //Debug.Log(rb.velocity);
        if (!isDead)
        {
            if (!GameManager.instance.transitionIsRunning)
            {
                canJump = CheckGround(jumpCheckRayLength);
                thereIsGround = CheckGround(groundCheckRayLength);
                switch (GameManager.instance.currentGameMode)
                {
                    case GameMode.SIDESCROLL:
                        if (transform.position.x > Register.instance.xMin && Input.GetAxis("Horizontal") < -controllerDeadZone)
                        {
                            Move(Vector3.right, speed, "Horizontal");
                        }
                        else if (transform.position.x < Register.instance.xMax && Input.GetAxis("Horizontal") > controllerDeadZone)
                        {
                            Move(Vector3.right, speed, "Horizontal");
                        }
                        if (Input.GetKeyDown(KeyCode.W) && canJump)
                        {
                            Jump();
                        }
                        if (thereIsGround && !canJump)
                        {
                            ApplyGravity();
                        }
                        else if ((thereIsGround && canJump))
                        {
                            if (rb.velocity.y < 0)
                            {
                                rb.velocity = Vector3.zero;
                                transform.position = new Vector3(transform.position.x, landmark.position.y, transform.position.z);
                            }
                        }
                        if (Input.GetKey(KeyCode.W))
                        {
                            if (!canJump && rb.velocity.y < 0.0f)
                            {
                                if (rb.velocity.y < -2)
                                {
                                    StabilizeAcceleration();
                                }
                                else
                                {
                                    Glide();
                                }
                            }
                        }
                        if (transform.rotation != sideScrollerRotation)
                        {
                            transform.rotation = sideScrollerRotation;
                        }
                        Vector3 aim = aimTransform.position - bulletSpawnPoint.position;
                        float aimAngle = Vector3.Angle(Vector3.right, aim);
                        Vector3 cross = Vector3.Cross(Vector3.right, aim);
                        if (aimAngle <= upRotationAngle && cross.z >= 0)
                        {
                            TurnAroundPlayer(bulletSpawnPoint);
                        }
                        else if (aimAngle <= downRotationAngle && cross.z < 0)
                        {
                            TurnAroundPlayer(bulletSpawnPoint);
                        }
                        ClampPosition(GameMode.SIDESCROLL);

                        break;
                    case GameMode.TOPDOWN:
                        Move(Vector3.forward, speed, "Vertical");
                        Move(Vector3.right, speed, "Horizontal");
                        TurnAroundPlayer(transform);

                        ClampPosition(GameMode.TOPDOWN);

                        if (Input.GetMouseButtonDown(1))
                        {
                            StartCoroutine(Melee());
                        }

                        break;
                }

                if (fireTimer < fireRatio)
                {
                    fireTimer += Time.deltaTime;
                }
                else
                {
                    Shoot();
                    fireTimer = 0.00f;
                }
                //PlayAnimation();
            }
            else
            {
                if (GameManager.instance.currentGameMode == GameMode.SIDESCROLL && bulletSpawnPoint.rotation != bulletSpawnPointStartRotation)
                {
                    bulletSpawnPoint.rotation = bulletSpawnPointStartRotation;
                }
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == enemyLayer && !isDead)
        {
            StartCoroutine("EnableDisableMesh");

            if (other.transform.parent.tag == "EnemyBullet")
            {
                Destroy(other.transform.parent.gameObject);
            }
        }
    }

    void ApplyGravity()
    {
        rb.AddForce(Vector3.down * gravity, ForceMode.Acceleration);
    }

    void StabilizeAcceleration()
    {
        rb.AddForce(Vector3.up * Mathf.Abs((rb.velocity.y / 3)), ForceMode.Impulse);
    }

    void Jump()
    {
        rb.velocity = new Vector3(0, jumpForce, 0);
    }

    void Glide()
    {
        rb.AddForce(Vector3.up * glideSpeed, ForceMode.Force);
    }

    bool CheckGround(float rayLength)
    {
        Debug.DrawRay(new Vector3(transform.position.x, transform.position.y, transform.position.z), Vector3.down);
        Ray ray = new Ray(new Vector3 (transform.position.x, transform.position.y, transform.position.z), Vector3.down);
        if (Physics.Raycast(ray, rayLength, groundMask))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void Move(Vector3 moveVector, float speed, string moveAxis)
    {
        if (Input.GetAxis(moveAxis) < -controllerDeadZone || Input.GetAxis(moveAxis) > controllerDeadZone)
        {
            transform.Translate(moveVector * Input.GetAxis(moveAxis) * speed * Time.deltaTime, Space.World);
        }
    }

    void TurnAroundPlayer(Transform transform)
    {
        switch (GameManager.instance.currentGameMode)
        {
            case GameMode.SIDESCROLL:
                transform.LookAt(new Vector3(aimTransform.position.x, aimTransform.position.y, transform.position.z));
                break;
            case GameMode.TOPDOWN:
                transform.LookAt(new Vector3(aimTransform.position.x, transform.position.y, aimTransform.position.z));
                break;
        }
    }

    void Shoot()
    {
        if (Input.GetMouseButton(0) && canShoot)
        {
            GameObject bullet = Instantiate(Register.instance.playerBullet, bulletSpawnPoint.position, bulletSpawnPoint.rotation) as GameObject;
            //bullet.SetActive(true);
            bullet.tag = playerBulletTag;
        }
    }

    public void ClampPosition(GameMode state)
    {
        switch (state)
        {
            case (GameMode.SIDESCROLL):
                transform.position = new Vector3(
                Mathf.Clamp(transform.position.x, Register.instance.xMin + sideXMin, Register.instance.xMax - sideXMax),
                Mathf.Clamp(transform.position.y, Register.instance.yMin + sideYMin, Register.instance.yMax - sideYMax),
                0.0f
                );
                break;
            case (GameMode.TOPDOWN):
                transform.position = new Vector3(
                Mathf.Clamp(transform.position.x, Register.instance.xMin + topXMin, Register.instance.xMax - topXMax),
                startPosition.y,
                Mathf.Clamp(transform.position.z, Register.instance.zMin.Value + topZMin, Register.instance.zMax.Value - topZMax)
                );
                break;
        }
    }

    void PlayAnimation()
    {
        horizontal = Input.GetAxis("Horizontal");
        ani.SetFloat("horizontal", horizontal);
    }

    IEnumerator Melee()
    {
        angle = 0;

        while (angle < 180)
        {
            canShoot = false;
            angle += 5;
            Vector3 initDir = -bulletSpawnPoint.forward;
            Quaternion angleQ = Quaternion.AngleAxis(angle, Vector3.up);
            Vector3 newVector = angleQ * initDir;

            Ray ray = new Ray(transform.position, newVector);

            if (Physics.Raycast(ray, out hit, meleeDistance))
            {
                Destroy(hit.transform.gameObject);
            }
            Debug.DrawRay(ray.origin, ray.direction * meleeDistance, Color.magenta);

            yield return null;
        }
        canShoot = true;
    }

    IEnumerator EnableDisableMesh()
    {
        isDead = true;
        meshGO.SetActive(false);

        yield return new WaitForSeconds(respawnTimer);

        meshGO.SetActive(true);
        transform.position = new Vector3(transform.position.x, startPosition.y, transform.position.z);
        rb.velocity = new Vector3(0.0f, 0.0f, 0.0f);
        isDead = false;
    }
}
