using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusPotion : Pickup
{
    public Material blueMat;
    public Material redMat;

    private MeshRenderer render;
    public ParticleSystem spawnParticles;

    public override void Awake()
    {
        base.Awake();
        render = GetComponent<MeshRenderer>();
        spawnParticles.startColor = new Color(128 * 0.85f, 0, 128 * 0.85f, 1);
        spawnParticles.Emit(20);
    }

    public override void Pick(Transform player)
    {
        base.Pick(player);
        PlayerType = player.GetComponent<PlayerController>().player;
        render.material = PlayerType == Player.P1 ? redMat : blueMat;
    }

    public override void Throw(Vector3 direction, bool cauldronVisible)
    {
        base.Throw(direction, cauldronVisible);

    }
}
