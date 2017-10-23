using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [HideInInspector]
    public Vector3 startPosition;
    [SerializeField]
    private GameObject playerModel;
    public float speed = 5.0f;
    public float jumpForce = 5.0f;
    public float upRotationAngle;
    public float downRotationAngle;
    private int enemyLayer = 12;
    public float jumpCheckRayLength;
    public float groundCheckRayLength;
    private float controllerDeadZone = 0.1f;
    public Transform bulletSpawnPoints;
    public Transform bulletSpawnPointLx;
    public Transform bulletSpawnPointRx;
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
    private Rigidbody rb;
    public LayerMask groundMask;
    public bool canShootAndMove = true;
    public bool canJump = true;
    private bool thereIsGround;
    private bool isDead;
    private float horizontal;
    public Transform landmark;
    public Collider sideBodyCollider;
    public Collider topBodyCollider;
    public Collider topTailCollider;
    private Quaternion sideBodyColliderStartRot;
    private Quaternion topBodyColliderStartRot;
    private Quaternion topTailBodyColliderStartRot;

    [Header("BulletPool")]
    private int indexOfBullet=0;


    [Header("Guns")]
    public GameObject armRx;
    public Transform gunRx;
    public GameObject armLx;
    public Transform gunLx;
    public float maxAngleRotation = 30;
    private float angleS = 0;

    [Header("Aim")]
    private float intersectionPoint;
    private Vector3 aimVector;
    private Plane? sidescrollPlane;
    private Plane? topDownPlane;
    private Ray aimRay;
    public GameObject aimTransformPrefab;
    private GameObject aimTransform;

    [Header("Boundaries")]
    public float sideXMin;
    public float sideXMax;
    public float sideYMin = 5.5f;
    public float sideYMax;
    public float topXMin, topXMax, topZMin, topZMax;

    [Header("Animations")]
    private bool sideScroll;
    private bool glide = false;

    [Header("TailMelee")]
    public float topdownSpeed;

	[Header("BiteMelee")]
	public float biteATKSpeed;
	public bool biteCoolDownActive;
	public float biteCoolDown;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        Register.instance.player = this;
    }

    void Start()
    {
        sideBodyColliderStartRot = sideBodyCollider.transform.rotation;
        topBodyColliderStartRot = topBodyCollider.transform.rotation;
        sideScrollerRotation = transform.rotation;
        bulletSpawnPointStartRotation = bulletSpawnPointLx.rotation;
        startPosition = transform.position;
        aimTransform = Instantiate(aimTransformPrefab, Vector3.zero, aimTransformPrefab.transform.rotation) as GameObject;
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
                Aim();

                switch (GameManager.instance.currentGameMode)
                {
				case GameMode.SIDESCROLL:
                    sideScroll = true;

                    if ((transform.position.x > Register.instance.xMin && Input.GetAxis ("Horizontal") < -controllerDeadZone) || (transform.position.x < Register.instance.xMax && Input.GetAxis ("Horizontal") > controllerDeadZone))
                    {
						Move (Vector3.right, speed, "Horizontal");
					}
					if (Input.GetKeyDown (KeyCode.W) && canJump)
                    {
						Jump ();
					}
					if (thereIsGround && !canJump)
                    {
						ApplyGravity ();
					}
                    else if ((thereIsGround && canJump))
                    {
				      if (rb.velocity.y < 0)
                       {
							rb.velocity = Vector3.zero;
							transform.position = new Vector3 (transform.position.x, landmark.position.y, transform.position.z);
					   }
					}
					if (Input.GetKey (KeyCode.W))
                    {
						if (!canJump && rb.velocity.y < 0.0f)
                         {
							if (rb.velocity.y < -2)
                            {
								StabilizeAcceleration ();
							}
                            else
                            {
								Glide ();
						    }
					     }
                    }
					if (transform.rotation != sideScrollerRotation)
                    {
						transform.rotation = sideScrollerRotation;
					}
                    Vector3 aim = aimTransform.transform.position - bulletSpawnPointLx.position;
					float aimAngle = Vector3.Angle (Vector3.right, aim);
					Vector3 cross = Vector3.Cross (Vector3.right, aim);
					if (aimAngle <= upRotationAngle && cross.z >= 0)
                    {
						TurnAroundGO(bulletSpawnPoints);
                    }
                    else if (aimAngle <= downRotationAngle && cross.z < 0)
                    {
                        TurnAroundGO(bulletSpawnPoints);
                    }

					ClampPosition (GameMode.SIDESCROLL);

					if (canShootAndMove && Input.GetMouseButtonDown (1) && !biteCoolDownActive && canJump)
                    {
                            StartCoroutine ("BiteAttack");
                    }
                    break;
                case GameMode.TOPDOWN:
                        sideScroll = false;
                        Move(Vector3.forward, speed, "Vertical");
                        Move(Vector3.right, speed, "Horizontal");
                        if (canShootAndMove)
                        {
                            TurnAroundGO(transform);
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
                if (GameManager.instance.currentGameMode == GameMode.SIDESCROLL && bulletSpawnPointLx.rotation != bulletSpawnPointStartRotation && bulletSpawnPointRx.rotation != bulletSpawnPointStartRotation)
                {
                    bulletSpawnPointLx.rotation = bulletSpawnPointStartRotation;
                    bulletSpawnPointRx.rotation = bulletSpawnPointStartRotation;
                }
                ChangePerspective();
            }
        }
    }

    void LateUpdate()
    {
        sideBodyCollider.transform.rotation = sideBodyColliderStartRot;
        topBodyCollider.transform.rotation = topBodyColliderStartRot;

    }

    void OnTriggerEnter(Collider other)
    {
		if (!topTailCollider.enabled && !biteCoolDownActive)
        {
            if (other.gameObject.layer == enemyLayer && !isDead)
            {
                StartCoroutine("EnableDisableMesh");

                if (other.transform.tag.StartsWith("EnemyBullet"))
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
        glide = false;
    }

    //void MoveArms()
    //{
    //    Vector3 aimtransform = new Vector3(aimTransform.transform.position.x - gunRx.position.x, aimTransform.transform.position.y - gunRx.position.y, aimTransform.transform.position.z - gunRx.position.z);
    //    float aimAngle = Vector3.Angle(Vector3.right, aimtransform);
    //    Vector3 cross = Vector3.Cross(Vector3.right, aimtransform);
    //    if(cross.z >= 0 && angleS >= -maxAngleRotation)
    //    {
    //        gunRx.RotateAround(gunRx.position, gunRx.forward, -2);
    //        angleS += -2;
    //    }
    //    if(cross.z < 0 && angleS < maxAngleRotation)
    //    {
    //        gunRx.RotateAround(gunRx.position, gunRx.forward, 2);
    //        angleS += 2;
    //    }
    //}
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

    void TurnAroundGO(Transform transform)
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
            if(GameManager.instance.currentGameMode == GameMode.SIDESCROLL)
            {
               GameObject bullet = PoolManager.instance.GetpooledBullet(PoolManager.instance.playerBulletpool);
               bullet.transform.position = bulletSpawnPointLx.position;
               bullet.transform.rotation = bulletSpawnPointLx.rotation;
               bullet.SetActive(true);
               PoolManager.instance.playerBulletpool.index++;

               if(PoolManager.instance.playerBulletpool.index >= PoolManager.instance.pooledPlayerBulletAmount)
               {
                  PoolManager.instance.playerBulletpool.index = 0;
               }
            }
            else
            {
                //GameObject bullet = Instantiate(Register.instance.propertiesPlayer.bulletPrefab, bulletSpawnPointLx.position, bulletSpawnPointLx.rotation) as GameObject;
                //GameObject Bullet = Instantiate(Register.instance.propertiesPlayer.bulletPrefab, bulletSpawnPointRx.position, bulletSpawnPointRx.rotation) as GameObject;
            } 
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
        AnimationManager.instance.GetAnimation("horizontal", horizontal);
        AnimationManager.instance.GetAnimation("sideScroll", sideScroll);
        AnimationManager.instance.GetAnimation("glide", glide);
        //AnimationManager.instance.GetAnimation("attack", sideScroll? biteAttack : tailAttack);
    }

    IEnumerator TailAttack()
    {
        canShootAndMove = false;
        angle = 0;
        topTailCollider.enabled = true;
      
        while (angle < 360)
        {
            angle += topdownSpeed;
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
        playerModel.SetActive(false);

        yield return new WaitForSeconds(respawnTimer);

        playerModel.SetActive(true);
        transform.position = new Vector3(transform.position.x, startPosition.y, transform.position.z);
        rb.velocity = new Vector3(0.0f, 0.0f, 0.0f);
        isDead = false;
    }
}
