using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [Header("Aim")]
    private float intersectionPoint;
    private Vector3 aimVector;
    private Plane? aimPlane;
    private Ray aimRay;
    public Transform aimTransform;

    void Update()
    {
        Aim();
    }

    void Aim()
    {
        if (GameManager.instance.cameraState == State.TOPDOWN)
        {
            if (aimPlane == null)
            {
                aimPlane = new Plane(-Camera.main.transform.forward, Vector3.zero);
            }
            aimRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (aimPlane.Value.Raycast(aimRay, out intersectionPoint))
            {
                aimVector = aimRay.GetPoint(intersectionPoint);
                aimTransform.position = aimVector;
            }
        }
    }
}