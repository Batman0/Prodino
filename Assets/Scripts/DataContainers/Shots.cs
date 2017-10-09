using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ShotType
{
    FORWARDSHOOTER,
    FORWARD,
    LASERDIAGONAL,
    SPHERICALAIMING,
    BOMBDROP,
    TRAIL,
    DOUBLEAIMING
    
}
public static class Shots
{
    public static void ShootForward(GameObject prefab, Transform spawnpoint, Transform rotTransform)
    {
        GameObject bullet = Object.Instantiate(prefab, spawnpoint.position, rotTransform.rotation) as GameObject;
        //bullet.layer = layer;
    }

    public static void ShootLaser(GameObject prefab, Transform bulletSpawnpoint, Transform rotTransform, float width, float height)
    {
        GameObject laser = Object.Instantiate(prefab, bulletSpawnpoint.position, rotTransform.rotation) as GameObject;
        laser.transform.localScale = new Vector3(width, height, laser.transform.localScale.z);
        laser.transform.SetParent(bulletSpawnpoint.parent);
    }

    /*public static void ShootAiming(GameObject prefab,Transform bulletSpawnpoint, Transform rotTransform)
    {
       
    }*/

    public static void ShootBomb(GameObject prefab, Transform spawnpoint, Transform rotTransform)
    {
        GameObject bomb = Object.Instantiate(prefab, spawnpoint.position, rotTransform.rotation);
    }

    public static void ShootTrail(GameObject prefab,ref GameObject gameObjectParticle,Transform spawnpoint, Transform rotTransform, bool canShoot)
    {
        if(!gameObjectParticle)
        {
            gameObjectParticle = Object.Instantiate(prefab, spawnpoint.position, rotTransform.rotation);
            Debug.Log(gameObjectParticle);
            ParticleSystem ps = gameObjectParticle.GetComponent<ParticleSystem>();
            var main = ps.main;
            main.startLifetime = Register.instance.propertiesTrail.fadeTime;
            gameObjectParticle.transform.SetParent(spawnpoint);
            gameObjectParticle.SetActive(false);
            Debug.Log(canShoot);
        }
        
        if(canShoot)
        {
            gameObjectParticle.SetActive(true);
        }
        else
        {
            gameObjectParticle.SetActive(false);
        }
    }

    //public static void ShootDouble(GameObject prefab,Transform)

    public static void Shoot(ShotType shotType, Quaternion barrelStartRot, Quaternion barrelInvertedRot, ref float timer, ref bool canShoot, ref bool rotateRight,ref GameObject particleTrail, Transform spawnPoint, Transform rotTransform, Transform transform)
    {
        switch (shotType)
        {
            case ShotType.FORWARDSHOOTER:
                if (timer < Register.instance.propertiesForwardShooter.fireRate)
                {
                    timer += Time.deltaTime;
                }
                else
                {
                    ShootForward(Register.instance.propertiesForwardShooter.bulletPrefab, spawnPoint, rotTransform);
                    timer = 0.0f;
                }
                break;
            case ShotType.FORWARD:
                break;
            case ShotType.LASERDIAGONAL:
                if (timer < Register.instance.propertiesLaserDiagonal.waitingTime)
                {
                    timer += Time.deltaTime;
                }
                else
                {
                    if (timer < Register.instance.propertiesLaserDiagonal.waitingTime + Register.instance.propertiesLaserDiagonal.loadingTime)
                    {
                        timer += Time.deltaTime;
                    }
                    else
                    {
                        if (!canShoot && shotType != ShotType.TRAIL)
                        {
                            canShoot = true;
                        }
                        if (canShoot)
                        {
                            ShootLaser(Register.instance.propertiesLaserDiagonal.laserPrefab, spawnPoint, rotTransform, Register.instance.propertiesLaserDiagonal.laserDepth, Register.instance.propertiesLaserDiagonal.laserHeight);
                            canShoot = false;
                        }
                        if (timer < Register.instance.propertiesLaserDiagonal.waitingTime + Register.instance.propertiesLaserDiagonal.loadingTime + Register.instance.propertiesLaserDiagonal.shootingTime)
                        {
                            timer += Time.deltaTime;
                        }
                        else
                        {
                            timer = 0.0f;
                            //canShoot = true;
                        }
                    }
                }
                break;
            case ShotType.SPHERICALAIMING:
                if(GameManager.instance.currentGameMode == GameMode.SIDESCROLL)
                {

                    Vector3 playerTransform = new Vector3(Register.instance.player.transform.position.x - transform.position.x, Register.instance.player.transform.position.y - transform.position.y, 0);
                    Vector3 barrelSpawnpointTransform = new Vector3(spawnPoint.position.x - transform.position.x, spawnPoint.position.y - transform.position.y, 0);
                    float angle = Vector3.Angle(barrelSpawnpointTransform, playerTransform);
                    Vector3 cross = Vector3.Cross(playerTransform, barrelSpawnpointTransform);

                    if (angle > Register.instance.propertiesSphericalAiming.rotationDeadZone)
                    {
                        if (cross.z >= 0)
                        {
                            rotTransform.RotateAround(transform.position, Vector3.forward, -Register.instance.propertiesSphericalAiming.rotationSpeed);
                        }
                        else
                        {
                            rotTransform.RotateAround(transform.position, Vector3.forward, Register.instance.propertiesSphericalAiming.rotationSpeed);
                        }
                    }                  
                    
                    if (timer < Register.instance.propertiesSphericalAiming.fireRate)
                    {
                        timer += Time.deltaTime;
                    }
                    else
                    { 
                        ShootForward(Register.instance.propertiesSphericalAiming.bulletPrefab, spawnPoint, rotTransform);
                        timer = 0.0f;
                    }
                }
                else
                {
                    if (rotateRight && rotTransform.rotation != barrelStartRot)
                    {
                        rotTransform.rotation = barrelStartRot;
                    }
                    else if (!rotateRight && rotTransform.rotation != barrelInvertedRot)
                    {
                        rotTransform.rotation = barrelInvertedRot;
                    }

                    if (Register.instance.player.transform.position.x >= transform.position.x)
                    {
                        if(rotateRight)
                        {
                            rotTransform.rotation = barrelInvertedRot;
                            rotateRight = false;
                        }
                    }
                    else
                    {
                        if(!rotateRight)
                        {
                            rotTransform.rotation = barrelStartRot;
                            rotateRight = true;
                        }
                    }

                    if (timer < Register.instance.propertiesSphericalAiming.fireRate)
                    {
                        timer += Time.deltaTime;
                    }
                    else
                    {
                        ShootForward(Register.instance.propertiesSphericalAiming.bulletPrefab, spawnPoint, rotTransform);
                        timer = 0.0f;
                    }
                }             
                break;
            case ShotType.BOMBDROP:
                if (timer < Register.instance.propertiesBombDrop.loadingTime)
                {
                    timer += Time.deltaTime;
                }
                else
                {
                    ShootBomb(Register.instance.propertiesBombDrop.bombPrefab, spawnPoint, rotTransform);
                    timer = 0.0f;
                }
                break;
            case ShotType.TRAIL:
                ShootTrail(Register.instance.propertiesTrail.trailPrefab, ref particleTrail, spawnPoint, rotTransform, canShoot);
                break;
            case ShotType.DOUBLEAIMING:
               /* if(GameManager.instance.currentGameMode == GameMode.SIDESCROLL)
                {

                }*/
                break;
        }
    }
}
