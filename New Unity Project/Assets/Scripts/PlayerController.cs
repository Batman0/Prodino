using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//2 funzioni movimento per le 2 modalità
//Sparo
//Melee in top down



public class PlayerController : MonoBehaviour
{
    public float speed = 5.0f;
    private float controllerDeadZone = 0.1f;
    public Transform aim;
    public GameObject bulletPrefab;
    public Transform bulletSpawnPoint;
    public float fireRatio = 0.10f;
    private float fireTimer;
    public CameraController cameraInstance;
  

    void Start()
    {
       cameraInstance.myState = CameraState.SIDESCROLL;
        fireTimer = fireRatio;
    }

    void Update()
    {
        switch (cameraInstance.myState)
        {
            case CameraState.SIDESCROLL:
                Move(Vector3.up, "Vertical");
                Move(Vector3.right, "Horizontal");
                break;
            case CameraState.TOPDOWN:
                Move(Vector3.right, "Vertical");
                Move(Vector3.back, "Horizontal");
                TurnAroundPlayer();
                break;
        }
        if (Input.GetMouseButton(0))
        {
            if (fireTimer < fireRatio)
            {
                fireTimer += Time.deltaTime;
            }
            else
            {
                Shoot();
                fireTimer = 0.00f;
            }
        }
    }

    void Move(Vector3 moveVector, string axis)
    {
        if (Input.GetAxis(axis) < -controllerDeadZone || Input.GetAxis(axis) > controllerDeadZone)
        {
            transform.Translate(moveVector * Input.GetAxis(axis) * speed * Time.deltaTime, Space.World);
        }
    }

    void TurnAroundPlayer()
    {
        
        transform.LookAt(new Vector3(aim.position.x, transform.position.y, aim.position.z));
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, transform.rotation) as GameObject;
    }
}
