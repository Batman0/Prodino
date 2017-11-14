using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSystemManager : MonoBehaviour
{

    ParticleSystem[] particles;
    bool isPlayingAll;
    void Awake()
    {
        particles = GetComponentsInChildren<ParticleSystem>();
    }
    /// <summary>
    /// Plays all the systems until they get stopped
    /// </summary>
    public void PlayAll()
    {
        foreach (ParticleSystem particle in particles)
        {
            particle.Play();
        }
    }
    /// <summary>
    /// Plays all the systems for a specified time
    /// </summary>
    /// <param name="playTime">The systems' playing duration</param>
    public void PlayAll(float playTime)
    {
        if (!isPlayingAll)
        {
            isPlayingAll = true;
            foreach (ParticleSystem particle in particles)
            {
                particle.Play();
            }
            StartCoroutine(StopAll(playTime));
        }
    }

    /// <summary>
    /// Plays all the systems continuously
    /// </summary>
    /// <param name="loop">Defines wether the systems should play continuously</param>
    public void PlayAll(bool looping)
    {
        if (looping)
        {
            if (!isPlayingAll)
            {
                isPlayingAll = true;
                foreach (ParticleSystem particle in particles)
                {
                    particle.Play();
                }
            }
        }
        else
        {
            PlayAll();
        }
    }
    /// <summary>
    /// Stops all the systems
    /// </summary>
    public void StopAll()
    {
        foreach (ParticleSystem particle in particles)
        {
            particle.Stop();
        }
        StopAllCoroutines();
    }
    /// <summary>
    /// Stops all the systems that are playing continuously
    /// </summary>
    /// <param name="looping">States if the system is playing continuously</param>
    public void StopAll(bool looping)
    {
        if (looping)
        {
            if (isPlayingAll)
            {
                foreach (ParticleSystem particle in particles)
                {
                    particle.Stop();
                }
                isPlayingAll = false;
            }
            StopAllCoroutines();
        }
        else
        {
            StopAll();
        }
    }
    IEnumerator StopAll(float playTime)
    {
        yield return new WaitForSeconds(playTime);
        isPlayingAll = false;
        foreach (ParticleSystem particle in particles)
        {
            particle.Stop();
        }
    }
    //IEnumerator StopAll(float playTime, bool loop)
    //{
    //    yield return new WaitForSeconds(playTime);
    //    isPlayingAll = false;
    //    foreach (ParticleSystem particle in particles)
    //    {
    //        particle.Stop();
    //        ParticleSystem.MainModule main = particle.main;
    //        main.loop = loop;
    //    }
    //}

}
