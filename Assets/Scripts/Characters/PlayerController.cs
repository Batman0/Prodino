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
    public GameObject aimTransform;
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
    //public float meleeDistance;
    private Rigidbody rb;
    public LayerMask groundMask;
    public bool canShootAndMove = true;
    public bool canJump = true;
    private bool thereIsGround;
    private bool isDead;
    private float horizontal;
    public Transform landmark;
    public Collider sideBodyCollider;
    //public Collider sideTailCollider;
    public Collider topBodyCollider;
    public Collider topTailCollider;

    //private SkinnedMeshRenderer skinnedMeshRen;

    [Header("Boundaries")]
    public float sideXMin;
    public float sideXMax;
    private float sideYMin = 3.8f;
    public float sideYMax;
    public float topXMin, topXMax, topZMin, topZMax;

    [Header("Animations")]
    public Animator ani;

    [Header("TailMelee")]
    public float topdownSpeed;

	//bite attack
	[Header("BiteMelee")]
	public float biteATKSpeed;
	public bool biteCoolDownActive;
	public float biteCoolDown;


    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        //skinnedMeshRen = GetComponentInChildren<SkinnedMeshRenderer>();
        Register.instance.player = this;
    }

    void Start()
    {
        sideScrollerRotation = transform.rotation;
        bulletSpawnPointStartRotation = bulletSpawnPoint.rotation;
        startPosition = transform.position;
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
					if ((transform.position.x > Register.instance.xMin && Input.GetAxis ("Horizontal") < -controllerDeadZone) || (transform.position.x < Register.instance.xMax && Input.GetAxis ("Horizontal") > controllerDeadZone)) {
						Move (Vector3.right, speed, "Horizontal");
					}
					if (Input.GetKeyDown (KeyCode.W) && canJump) {
						Jump ();
					}
					if (thereIsGround && !canJump) {
						ApplyGravity ();
					} else if ((thereIsGround && canJump)) {
						if (rb.velocity.y < 0) {
							rb.velocity = Vector3.zero;
							transform.position = new Vector3 (transform.position.x, landmark.position.y, transform.position.z);
						}
					}
					if (Input.GetKey (KeyCode.W)) {
						if (!canJump && rb.velocity.y < 0.0f) {
							if (rb.velocity.y < -2) {
								StabilizeAcceleration ();
							} else {
								Glide ();
							}
						}
					}
					if (transform.rotation != sideScrollerRotation) {
						transform.rotation = sideScrollerRotation;
					}
					Vector3 aim = aimTransform.transform.position - bulletSpawnPoint.position;
					float aimAngle = Vector3.Angle (Vector3.right, aim);
					Vector3 cross = Vector3.Cross (Vector3.right, aim);
					if (aimAngle <= upRotationAngle && cross.z >= 0) {
						TurnAroundPlayer (bulletSpawnPoint);
					} else if (aimAngle <= downRotationAngle && cross.z < 0) {
						TurnAroundPlayer (bulletSpawnPoint);
					}

					ClampPosition (GameMode.SIDESCROLL);


						
					if (canShootAndMove && Input.GetMouseButtonDown (1) && !biteCoolDownActive) {
							StartCoroutine ("BiteAttack");
						}
						


                        break;
                case GameMode.TOPDOWN:
                        Move(Vector3.forward, speed, "Vertical");
                        Move(Vector3.right, speed, "Horizontal");
                        if (canShootAndMove)
                        {
                            TurnAroundPlayer(transform);
                        }

                        ClampPosition(GameMode.TOPDOWN);

                        if (canShootAndMove && Input.GetMouseButtonDown(1))
                        {
                            StartCoroutine("TailAttack");
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
                PlayAnimation();
            }
            else
            {
                if (GameManager.instance.currentGameMode == GameMode.SIDESCROLL && bulletSpawnPoint.rotation != bulletSpawnPointStartRotation)
                {
                    bulletSpawnPoint.rotation = bulletSpawnPointStartRotation;
                }
                ChangePerspective();
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
		if (!topTailCollider.enabled && !biteCoolDownActive)
        {
            if (other.gameObject.layer == enemyLayer && !isDead)
            {
                StartCoroutine("EnableDisableMesh");

                if (other.transform.tag == "EnemyBullet")
                {
                    Destroy(other.transform.gameObject);
                }
            }
        }
        else if (topTailCollider.enabled)
        {
            if (other.gameObject.layer == enemyLayer && !isDead)
            {
                Destroy(other.transform.gameObject);
            }
        }

		if (biteCoolDownActive) {
			if (other.gameObject.layer == enemyLayer && !isDead)
			{
				Destroy(other.transform.gameObject);
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
                transform.LookAt(new Vector3(aimTransform.transform.position.x, aimTransform.transform.position.y, transform.position.z));
                break;
            case GameMode.TOPDOWN:
                transform.LookAt(new Vector3(aimTransform.transform.position.x, transform.position.y, aimTransform.transform.position.z));
                break;
        }
    }

    void Shoot()
    {
        if (Input.GetMouseButton(0) && canShootAndMove)
        {
            GameObject bullet = Instantiate(Register.instance.propertiesPlayer.bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation) as GameObject;
            //bullet.SetActive(true);
            //bullet.tag = playerBulletTag;
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

    void ChangePerspective()
    {
        if (GameManager.instance.transitionIsRunning)
        {
            if (GameManager.instance.currentGameMode == GameMode.TOPDOWN)
            {
                if (!sideBodyCollider.enabled)
                {
                    topBodyCollider.enabled = false;
                    sideBodyCollider.enabled = true;
                }
            }
            else
            {
                if (!topBodyCollider.enabled)
                {
                    sideBodyCollider.enabled = false;
                    topBodyCollider.enabled = true;
                }
            }
        }
    }

    void PlayAnimation()
    {
        horizontal = Input.GetAxis("Horizontal");
        ani.SetFloat("horizontal", horizontal);
    }

    IEnumerator TailAttack()
    {
        canShootAndMove = false;
        angle = 0;
        topTailCollider.enabled = true;

        while (angle < 360)
        {
            angle += topdownSpeed;
            //Vector3 initDir = -bulletSpawnPoint.forward;
            //Quaternion angleQ = Quaternion.AngleAxis(angle, Vector3.up);
            //Vector3 newVector = angleQ * initDir;

            //Ray ray = new Ray(transform.position, newVector);

            //if (Physics.Raycast(ray, out hit, meleeDistance))
            //{
            //    Destroy(hit.transform.gameObject);
            //}
            //Debug.DrawRay(ray.origin, ray.direction * meleeDistance, Color.magenta);

            transform.Rotate(Vector3.up, topdownSpeed, Space.World);

            yield return null;
        }
        topTailCollider.enabled = false;
        canShootAndMove = true;
    }

	IEnumerator BiteAttack()
	{
		canShootAndMove = false;
		rb.velocity = new Vector3(0, biteATKSpeed, 0);
		biteCoolDownActive = true;

		yield return new WaitForSeconds (biteCoolDown);

		biteCoolDownActive = false;
		canShootAndMove = true;
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
