using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CameraState
{
    SIDESCROLL,
    TOPDOWN
}

public class CameraController : MonoBehaviour
{
    public float timeScaleValueLerping = 0.0f;
    public float timeScaleValueNotLerping = 1.0f;
    private Vector3 lerp;
    private Quaternion slerp;
    public Transform topDownCameraPosition;
    public Transform sideScrollCameraPosition;
    public float lerpSpeed = 1.0f;
    public float lerpDistance = 0.01f;
    [HideInInspector]
    public bool isLerpingCamera = false;
    public CameraState myState;

    void Start()
    {
        myState = CameraState.SIDESCROLL;
    }

    // Update is called once per frame
    void Update ()
    {
		if(Input.GetKeyDown(KeyCode.Space) && !isLerpingCamera)
        {
            isLerpingCamera = true;
            StartCoroutine("LerpCamera");
        }
	}

    IEnumerator LerpCamera()
    {
        Time.timeScale = timeScaleValueLerping;
        switch (myState)
        {
            case CameraState.SIDESCROLL:
                myState = CameraState.TOPDOWN;
                while (Vector3.Distance(transform.position,topDownCameraPosition.position)>= lerpDistance)
                {
                    lerp = Vector3.Lerp(transform.position, topDownCameraPosition.position, lerpSpeed);
                    transform.position = lerp;
                    slerp = Quaternion.Slerp(transform.rotation, topDownCameraPosition.rotation, lerpSpeed);
                    transform.rotation = slerp;
                    yield return null;
                }
                transform.position = topDownCameraPosition.position;
                break;

            case CameraState.TOPDOWN:

                myState = CameraState.SIDESCROLL;

                while (Vector3.Distance(transform.position,sideScrollCameraPosition.position) >= lerpDistance)
                {
                    lerp = Vector3.Lerp(transform.position, sideScrollCameraPosition.position, lerpSpeed);
                    transform.position = lerp;
                    slerp = Quaternion.Slerp(transform.rotation, sideScrollCameraPosition.rotation, lerpSpeed);
                    transform.rotation = slerp;
                    yield return null;
                }
                transform.position = sideScrollCameraPosition.position;
                break;
        }
        Time.timeScale = timeScaleValueNotLerping;
        isLerpingCamera = false;
    }
}
