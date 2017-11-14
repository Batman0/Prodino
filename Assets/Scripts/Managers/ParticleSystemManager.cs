using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSystemManager : MonoBehaviour
{

    ParticleSystem[] particles;

    void Awake()
    {
        particles = GetComponentsInChildren<ParticleSystem>();
    }

    public void PlayAll()
    {
        foreach (ParticleSystem particle in particles)
        {
            particle.Play();
        }
    }
    public void StopAll()
    {
        foreach (ParticleSystem particle in particles)
        {
            particle.Stop();
        }
    }


}
