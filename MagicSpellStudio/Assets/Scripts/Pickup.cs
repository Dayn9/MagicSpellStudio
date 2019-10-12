using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    private Rigidbody rb;

    private const int throwForce = 6;

    protected static Cauldron cauldron;

    public bool PickedUp { get; set; }

    private bool toCauldron = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (toCauldron)
        {
            transform.position = Vector3.Lerp(transform.position, cauldron.transform.position, 0.5f);
        }    
    }

    public void Pick(Transform player)
    {
        rb.useGravity = false;
        transform.SetParent(player);

        toCauldron = false;

        PickedUp = true;
    }

    public void Throw(Vector3 direction, bool cauldronVisible)
    {
        rb.useGravity = true;
        transform.SetParent(null);

        if (cauldronVisible)
        {
            toCauldron = true;
            rb.AddForce(new Vector3(direction.x, 1, direction.z) * throwForce / rb.mass, ForceMode.Impulse);
        }
        else
        {
            rb.AddForce(new Vector3(direction.x, 1, direction.z) * throwForce / rb.mass, ForceMode.Impulse);
        }

        PickedUp = false;
    }

}
