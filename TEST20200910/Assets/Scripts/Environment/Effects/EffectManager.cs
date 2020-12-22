using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem boostParticle = null;

    [SerializeField]
    private ParticleSystem burstParticle = null;

    [SerializeField]
    private ParticleSystem gravityParticle = null;


    // Start is called before the first frame update
    void Start()
    {
        boostParticle.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        burstParticle.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        gravityParticle.Stop(true, ParticleSystemStopBehavior.StopEmitting);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetBoostParticleActive(bool active)
    {
        if (active)
            boostParticle.Play();
        else
            boostParticle.Stop(true, ParticleSystemStopBehavior.StopEmitting);
    }

    public void SetBurstParticleActive(bool active)
    {
        if (active)
            burstParticle.Play();
        else
            burstParticle.Stop(true, ParticleSystemStopBehavior.StopEmitting);
    }

    public void SetGravityParticleActive(bool active)
    {
        if (active)
            gravityParticle.Play();
        else
            gravityParticle.Stop(true, ParticleSystemStopBehavior.StopEmitting);
    }
}
