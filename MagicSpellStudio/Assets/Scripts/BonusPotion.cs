using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusPotion : Pickup
{
    public Material blueMat;
    public Material redMat;

    private MeshRenderer render;

    public override void Awake()
    {
        base.Awake();
        render = GetComponent<MeshRenderer>();
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
