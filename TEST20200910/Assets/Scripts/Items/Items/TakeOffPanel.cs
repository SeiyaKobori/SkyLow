using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeOffPanel : ItemBase
{
    protected override int itemId { get; set; } = 0;

    private const float BoostValue = 30;
    [SerializeField]
    private ParticleSystem particle = null;

    private void Awake()
    {

    }

    public override void ObtainItem(PlayerManager player)
    {
        player.SetAirBoostActive(true);
        player.AddBoost(BoostValue);
        particle.gameObject.SetActive(true);
        particle.Play();
        Invoke("StopParticle", 2);
    }

    private void StopParticle()
    {
        particle.Stop();
        particle.gameObject.SetActive(false);
    }
}
