using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cauldron : Pickup
{
    int red = 0;
    int blue = 0;

    private void Start()
    {
        cauldron = this;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.Equals("Pickup"))
        {
            Player pt = collision.gameObject.GetComponent<Pickup>().PlayerType;

            switch (pt)
            {
                case Player.P1:
                    red++;
                    break;
                case Player.P2:
                    blue++;
                    break;
            }

            Debug.Log(red + " " + blue);

            Destroy(collision.gameObject);
        }
    }
}
