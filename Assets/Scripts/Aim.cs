using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aim : MonoBehaviour
{
    private float intersectionPoint;
    private Vector3 aimVector;
    private Plane aimPlane;
    private Ray aimRay;

    void Update()
    {
        if (GameManager.instance.cameraState == State.TOPDOWN)
        {
            aimPlane = new Plane(-Camera.main.transform.forward, Vector3.zero);
        }
        aimRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(aimPlane.Raycast(aimRay,out intersectionPoint))
        {
            aimVector = aimRay.GetPoint(intersectionPoint);
            transform.position = aimVector;
        }
    }
}