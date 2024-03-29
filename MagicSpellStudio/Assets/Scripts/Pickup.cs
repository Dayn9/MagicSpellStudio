﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    private Rigidbody rb;

    private const int throwForce = 8;

    protected static Cauldron cauldron;

    public bool PickedUp { get; set; }

    private bool toCauldron = false;

    private float lerpDeltaTime = 0;
    private float lerpTime = 0.15f;
    private Vector3 startLerp = new Vector3();
    private Vector3 endLerp = new Vector3();
    private WaitForFixedUpdate wait = new WaitForFixedUpdate();

    public Player PlayerType { get; set; }
    public float Mass { get { return rb.mass; } }
    public bool FirstPickedUp { get; private set; }
    public bool isPurple = false;

    public virtual void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public virtual void Pick(Transform player)
    {
        
        if (transform.parent != null)
        {
            PlayerController currentPlayer = GetComponentInParent<PlayerController>();
            if(currentPlayer != null)
            {
                currentPlayer.Holding = false;
            }
        }

        rb.useGravity = false;
        transform.SetParent(player);

        toCauldron = false;

        PickedUp = true;
        FirstPickedUp = true;
    }

    public virtual void Throw(Vector3 direction, bool cauldronVisible)
    {
        rb.useGravity = true;
        transform.SetParent(null);

        if (cauldronVisible && this != cauldron)
        {
            toCauldron = true;
            StartCoroutine(PickupLerp());
        }
        else
        {
            rb.AddForce(new Vector3(direction.x, 1, direction.z) * throwForce, ForceMode.Impulse);
        }

        PickedUp = false;
    }

    private IEnumerator PickupLerp()
    {
        lerpDeltaTime = 0;
        startLerp = gameObject.transform.position;
        endLerp = cauldron.transform.position;
        while (lerpDeltaTime < lerpTime)
        {
            gameObject.transform.position = Vector3.Lerp(startLerp, endLerp, lerpDeltaTime / lerpTime);
            yield return wait;
            lerpDeltaTime += Time.deltaTime;
        }
        gameObject.transform.position = endLerp;
    }

}
