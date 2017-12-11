using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FortressBossWaveCollider : MonoBehaviour
{

    private bool playerInContact;
    private SphereCollider waveSphereCollider;
    private BoxCollider waveSafezoneCollider;
    private float initialRadius;
    private Vector3 initialSize;
    private Vector3 initialCenter;

    public bool PlayerInContact
    {
        get { return playerInContact; }
    }

    void Awake()
    {
        Collider waveCollider = GetComponent<Collider>();
        if (waveCollider is SphereCollider)
        {
            waveSphereCollider = GetComponent<SphereCollider>();
            initialRadius = waveSphereCollider.radius;
        }
        else
        {
            waveSafezoneCollider = GetComponent<BoxCollider>();
            initialSize = waveSafezoneCollider.size;
            initialCenter = waveSafezoneCollider.center;
        }
    }

    void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.layer == Register.instance.PlayerLayer)
        {
            playerInContact = true;
        }
    }

    void OnTriggerStay(Collider coll)
    {
        if (coll.gameObject.layer == Register.instance.PlayerLayer)
        {
            playerInContact = true;
        }
    }

    void LateUpdate()
    {
        playerInContact = false;
    }

    public void ExpandCollider(float expansionFactor)
    {
        if (waveSphereCollider != null)
        {
            waveSphereCollider.radius += expansionFactor * Time.fixedDeltaTime;
        }
        else
        {
            waveSafezoneCollider.center = new Vector3(waveSafezoneCollider.center.x - expansionFactor * Time.fixedDeltaTime, waveSafezoneCollider.center.y, waveSafezoneCollider.center.z);
            waveSafezoneCollider.size = new Vector3(waveSafezoneCollider.size.x + expansionFactor * Time.fixedDeltaTime, waveSafezoneCollider.size.y + expansionFactor * Time.fixedDeltaTime, waveSafezoneCollider.size.z + expansionFactor * Time.fixedDeltaTime);
        }
    }

    public void Reset()
    {
        if (waveSphereCollider != null)
        {
            waveSphereCollider.radius = initialRadius;
        }
        else
        {
            waveSafezoneCollider.center = initialCenter;
            waveSafezoneCollider.size = initialSize;
        }
    }

}
