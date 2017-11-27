using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public enum ShotType
//{
//    ForwardShooter,
//    Forward,
//    LaserDiagonal,
//    SphericalAiming,
//    BombDrop,
//    Trail,
//    DoubleAiming
    
//}
public static class Shots
{
    //public static void ShootForward(GameObject prefab, Transform spawnpoint, Transform rotTransform)
    //{
    //    GameObject bullet = Object.Instantiate(prefab, spawnpoint.position, rotTransform.rotation) as GameObject;
    //    //bullet.layer = layer;
    //}

    //public static void ShootLaser(GameObject prefab, Transform bulletSpawnpoint, Transform rotTransform, float width, float height)
    //{
    //    GameObject laser = Object.Instantiate(prefab, bulletSpawnpoint.position, rotTransform.rotation) as GameObject;
    //    laser.transform.localScale = new Vector3(width, height, laser.transform.localScale.z);
    //    laser.transform.SetParent(bulletSpawnpoint.parent);
    //}

    ///*public static void ShootAiming(GameObject prefab,Transform bulletSpawnpoint, Transform rotTransform)
    //{
       
    //}*/

    //public static void ShootBomb(GameObject prefab, Transform spawnpoint, Transform rotTransform)
    //{
    //    GameObject bomb = Object.Instantiate(prefab, spawnpoint.position, rotTransform.rotation);
    //}

    //public static void ShootTrail(GameObject prefab, ref GameObject trailGo,Transform spawnpoint, Transform rotTransform, ref bool canShoot)
    //{
        
    //    if(canShoot)
    //    {
    //        trailGo = Object.Instantiate(prefab, spawnpoint.position, Quaternion.Inverse(rotTransform.rotation));
    //        trailGo.transform.SetParent(spawnpoint);
    //        canShoot = false;
    //    }
        
    //}

    //public static void ShootDouble(GameObject prefab, Transform bulletspawnPoint, Transform rotTransform, Transform bulletSpawnPointOther)
    //{
    //    GameObject bullet = Object.Instantiate(prefab, bulletspawnPoint.position, rotTransform.rotation);
    //    GameObject secondBullet = Object.Instantiate(prefab, bulletSpawnPointOther.position,Quaternion.Inverse(rotTransform.rotation));
    //}

    //public static void ShootDoubleSin(GameObject prefab, Transform bulletspawnPoint, Transform rotTransform, Transform bulletSpawnPointOther)
    //{
    //    GameObject bullet = Object.Instantiate(prefab, bulletspawnPoint.position, rotTransform.rotation);
    //    GameObject secondBullet = Object.Instantiate(prefab, bulletSpawnPointOther.position, rotTransform.rotation);
    //    secondBullet.tag = "EnemyBulletInverse";
    //}

    //public static void Shoot(ShotType shotType, Quaternion barrelStartRot, Quaternion barrelInvertedRot, ref float timer, ref bool canShoot, ref bool rotateRight,ref GameObject particleTrail, Transform spawnPoint, Transform rotTransform, Transform transform, Transform spawnPointOther)
    //{
    //    switch (shotType)
    //    {
    //        case ShotType.FORWARDSHOOTER:
    //            if (timer < Register.instance.propertiesForwardShooter.fireRate)
    //            {
    //                timer += Time.deltaTime;
    //            }
    //            else
    //            {
    //                ShootForward(Register.instance.propertiesForwardShooter.bulletPrefab, spawnPoint, rotTransform);
    //                timer = 0.0f;
    //            }
    //            break;
    //        case ShotType.FORWARD:
    //            break;
    //        case ShotType.LASERDIAGONAL:
    //            if (timer < Register.instance.propertiesLaserDiagonal.waitingTime)
    //            {
    //                timer += Time.deltaTime;
    //            }
    //            else
    //            {
    //                if (timer < Register.instance.propertiesLaserDiagonal.waitingTime + Register.instance.propertiesLaserDiagonal.loadingTime)
    //                {
    //                    timer += Time.deltaTime;
    //                }
    //                else
    //                {
    //                    if (!canShoot && shotType != ShotType.TRAIL)
    //                    {
    //                        canShoot = true;
    //                    }
    //                    if (canShoot)
    //                    {
    //                        ShootLaser(Register.instance.propertiesLaserDiagonal.laserPrefab, spawnPoint, rotTransform, Register.instance.propertiesLaserDiagonal.laserWidth, Register.instance.propertiesLaserDiagonal.laserHeight);
    //                        canShoot = false;
    //                    }
    //                    if (timer < Register.instance.propertiesLaserDiagonal.waitingTime + Register.instance.propertiesLaserDiagonal.loadingTime + Register.instance.propertiesLaserDiagonal.shootingTime)
    //                    {
    //                        timer += Time.deltaTime;
    //                    }
    //                    else
    //                    {
    //                        timer = 0.0f;
    //                        //canShoot = true;
    //                    }
    //                }
    //            }
    //            break;
    //        case ShotType.SPHERICALAIMING:
    //            if(GameManager.instance.currentGameMode == GameMode.SIDESCROLL)
    //            {

    //                Vector3 playerTransform = new Vector3(Register.instance.player.transform.position.x - transform.position.x, Register.instance.player.transform.position.y + 2 - transform.position.y, 0);
    //                Vector3 barrelSpawnpointTransform = new Vector3(spawnPoint.position.x - transform.position.x, spawnPoint.position.y - transform.position.y, 0);
    //                float angle = Vector3.Angle(barrelSpawnpointTransform, playerTransform);
    //                Vector3 cross = Vector3.Cross(playerTransform, barrelSpawnpointTransform);

    //                if (angle > Register.instance.propertiesSphericalAiming.rotationDeadZone)
    //                {
    //                    if (cross.z >= 0)
    //                    {
    //                        rotTransform.RotateAround(transform.position, Vector3.forward, -Register.instance.propertiesSphericalAiming.rotationSpeed);
    //                    }
    //                    else
    //                    {
    //                        rotTransform.RotateAround(transform.position, Vector3.forward, Register.instance.propertiesSphericalAiming.rotationSpeed);
    //                    }
    //                }                  
                    
    //                if (timer < Register.instance.propertiesSphericalAiming.fireRate)
    //                {
    //                    timer += Time.deltaTime;
    //                }
    //                else
    //                { 
    //                    ShootForward(Register.instance.propertiesSphericalAiming.bulletPrefab, spawnPoint, rotTransform);
    //                    timer = 0.0f;
    //                }
    //            }
    //            else
    //            {
    //                if (rotateRight && rotTransform.rotation != barrelStartRot)
    //                {
    //                    rotTransform.rotation = barrelStartRot;
    //                }
    //                else if (!rotateRight && rotTransform.rotation != barrelInvertedRot)
    //                {
    //                    rotTransform.rotation = barrelInvertedRot;
    //                }

    //                if (Register.instance.player.transform.position.x >= transform.position.x)
    //                {
    //                    if(rotateRight)
    //                    {
    //                        rotTransform.rotation = barrelInvertedRot;
    //                        rotateRight = false;
    //                    }
    //                }
    //                else
    //                {
    //                    if(!rotateRight)
    //                    {
    //                        rotTransform.rotation = barrelStartRot;
    //                        rotateRight = true;
    //                    }
    //                }

    //                if (timer < Register.instance.propertiesSphericalAiming.fireRate)
    //                {
    //                    timer += Time.deltaTime;
    //                }
    //                else
    //                {
    //                    ShootForward(Register.instance.propertiesSphericalAiming.bulletPrefab, spawnPoint, rotTransform);
    //                    timer = 0.0f;
    //                }
    //            }             
    //            break;
    //        case ShotType.BOMBDROP:
    //            if (timer < Register.instance.propertiesBombDrop.loadingTime)
    //            {
    //                timer += Time.deltaTime;
    //            }
    //            else
    //            {
    //                ShootBomb(Register.instance.propertiesBombDrop.bombPrefab, spawnPoint, rotTransform);
    //                timer = 0.0f;
    //            }
    //            break;
    //        case ShotType.TRAIL:
    //            ShootTrail(Register.instance.propertiesTrail.trailPrefab, ref particleTrail, spawnPoint, rotTransform, ref canShoot);
    //            break;
    //        case ShotType.DOUBLEAIMING:
    //            if (GameManager.instance.currentGameMode == GameMode.SIDESCROLL)
    //            {
    //                if (timer < Register.instance.propertiesDoubleAiming.fireRate)
    //                {
    //                    timer += Time.deltaTime;
    //                }
    //                else
    //                {
    //                    ShootDouble(Register.instance.propertiesDoubleAiming.sidescrollBulletPrefab, spawnPoint, rotTransform, spawnPointOther);
    //                    timer = 0.0f;
    //                }
    //            }
    //            else
    //            {
    //                if (timer < Register.instance.propertiesDoubleAiming.fireRate)
    //                {
    //                    timer += Time.deltaTime;
    //                }
    //                else
    //                {
    //                    ShootDoubleSin(Register.instance.propertiesDoubleAiming.topdownBulletPrefab, spawnPoint, rotTransform, spawnPointOther);
    //                    timer = 0.0f;
    //                }
    //            }
    //            break;
    //    }
    //}
}
