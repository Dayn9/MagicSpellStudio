using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerType : MonoBehaviour
{
    public Player playerType;

    // Start is called before the first frame update
    void Start()
    {
        Pickup pickup = gameObject.GetComponent<Pickup>();
        pickup.PlayerType = playerType;
    }
}
