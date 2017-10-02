using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//2 funzioni movimento per le 2 modalità
//Sparo
//Melee in top down
[System.Serializable]
public class BoundarySideScroll
{
    public float xMin, xMax, yMin, yMax;
}


//GURRA Class?
[System.Serializable]
public class BoundaryTopDown
{
    //GURRA Quanto è preciso questos sistema? Se facessi uno zoom/movimento di camera?
    public float xMin, xMax, zMin, zMax;
}

public class PlayerController : MonoBehaviour
{
    [HideInInspector]
    public Vector3 startPosition;
    [SerializeField]
    private GameObject mesh;
    public float speed = 5.0f;
    public float jumpForce = 5.0f;
    public float upRotationAngle;
    public float downRotationAngle;
    //GURRA Cioè?
    //Carlo sono gli angoli massimi di rotazione del player in sideScroll
    public float canJumpLength;
    public float isGroundLength;
    private float controllerDeadZone = 0.1f;
    //private float CheckGroundRaycastMargin = 1;
    [HideInInspector]
    public Transform aimTransform;
    //public GameObject myBullet;
    //public PlayerBullet myBulletScript;
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
    //GURRA  
    public bool canJump = true;
    private bool isGround;
    private bool isDead;
    private float horizontal;

    //private SkinnedMeshRenderer skinnedMeshRen;

    [Header("Boundaries")]
    public float sidexMin;
    public float sidexMax, sideyMin, sideyMax;
    public float topxMin, topxMax, topzMin, topzMax;

    [Header("Animations")]
    public Animator ani;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        //skinnedMeshRen = GetComponentInChildren<SkinnedMeshRenderer>();
    }

    void Start()
    {
        //transform.position = startPosition;
        fireTimer = fireRatio;
        sideScrollerRotation = transform.rotation;
        bulletSpawnPointStartRotation = bulletSpawnPoint.rotation;
        //myBulletScript.speed = bulletProperties.p_Speed;
        //myBulletScript.destructionMargin = bulletProperties.p_DestructionMargin;
        //myBulletScript.originalPos = startPosition;
    }

    void Update()
    {
        //Debug.Log(rb.velocity);
        if (!isDead)
        {
            if (!GameManager.instance.transitionIsRunning)
            {
                canJump = CheckGround(canJumpLength);
                isGround = CheckGround(isGroundLength);
                switch (GameManager.instance.currentGameMode)
                {
                    case GameMode.SIDESCROLL:
                        if (transform.position.x > sidexMin && Input.GetAxis("Horizontal") < -controllerDeadZone)
                        {
                            Move(Vector3.right, speed, "Horizontal");
                        }
                        else if (transform.position.x < sidexMax && Input.GetAxis("Horizontal") > controllerDeadZone)
                        {
                            Move(Vector3.right, speed, "Horizontal");
                        }
                        if (Input.GetKeyDown(KeyCode.W) && canJump)
                        {
                            Jump();
                        }
                        if (isGround && !canJump)
                        {
                            ApplyGravity();
                        }
                        else if ((isGround && canJump))
                        {
                            if (rb.velocity.y < 0)
                            {
                                rb.velocity = Vector3.zero;
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
                        //Debug.Log("aimAngle: " + aimAngle);
                        //Debug.Log("cross: " + Vector3.Cross(Vector3.right, aim));
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

                //GURRA WHAAAAAT? Quindi se il player non preme il mouse il timer non viene aggiornato???
                //Il timer veniva aggiornato lo stesso ma comunque ho sistemato 
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
        if (!isDead)
        {
            StartCoroutine("BlinkMeshRen");

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
        //GURRA istanzia a run time i proiettili? Non sarebbe meglio avere un pool?
        //Carlo intendi un set già fatto?
        if (Input.GetMouseButton(0) && canShoot)
        {
            GameObject bullet = Instantiate(Register.instance.playerBullet, bulletSpawnPoint.position, bulletSpawnPoint.rotation) as GameObject;
            bullet.SetActive(true);
            bullet.tag = playerBulletTag;
        }
    }

    //GURRA cosa fa questo metodo?
    //CARLO Blocca la posizione del player tra due limiti.Cioè se sei in side hai un certo limite così da non poter superare lo schermo lo stesso in Top
    public void ClampPosition(GameMode state)
    {
        switch (state)
        {
            case (GameMode.SIDESCROLL):
                transform.position = new Vector3(
                Mathf.Clamp(transform.position.x, sidexMin, sidexMax),
                Mathf.Clamp(transform.position.y, sideyMin, sideyMax),
                0.0f
                );
                break;
            case (GameMode.TOPDOWN):
                transform.position = new Vector3(
                Mathf.Clamp(transform.position.x, topxMin, topxMax),
                startPosition.y,
                Mathf.Clamp(transform.position.z, topzMin, topzMax)
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

    //GURRA perché il metodo che gestisce la morte del player si chiama blinkmeshren?
    //CARLO Effettivamente bisogna cambiarlo ma perchè si è modificato il metodo base provvediamo subito 
    IEnumerator BlinkMeshRen()
    {
        isDead = true;
        mesh.SetActive(false);

        yield return new WaitForSeconds(respawnTimer);

        mesh.SetActive(true);
        transform.position = new Vector3(transform.position.x, startPosition.y, transform.position.z);
        rb.velocity = new Vector3(0.0f, 0.0f, 0.0f);
        isDead = false;
    }
}
