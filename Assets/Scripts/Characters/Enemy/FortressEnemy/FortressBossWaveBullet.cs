using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FortressBossWaveBullet : MonoBehaviour
{

    public FortressBossWaveCollider[] colliders;
    public ParticleSystem waveParticle;
    public float expansionFactor;
    public float waveDuration = 7;
    private bool shot;
    void Awake()
    {
        waveParticle = GetComponentInChildren<ParticleSystem>();
        Shoot();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (shot)
        {
            CheckTrevorContact();
            foreach (FortressBossWaveCollider c in colliders)
            {
                c.ExpandCollider(expansionFactor);
            }
        }
    }

    void CheckTrevorContact()
    {
        if (colliders[0].PlayerInContact && !colliders[1].PlayerInContact && !colliders[2].PlayerInContact)
        {
            //AmmazzaTrevor
            Debug.Log("Trevor Hit");
        }
    }

    public void Shoot()
    {
        shot = true;
        waveParticle.Play();
        transform.Rotate(0, Random.Range(0f, 360f), 0);
        StartCoroutine(Reset());
    }


    public IEnumerator Reset()
    {
        yield return new WaitForSeconds(waveDuration);
        foreach (FortressBossWaveCollider c in colliders)
        {
            c.Reset();
        }
        shot = false;
        waveParticle.Stop();
        waveParticle.Clear();
        Shoot();
    }
}