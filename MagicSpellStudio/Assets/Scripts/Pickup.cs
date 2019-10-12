using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    private Rigidbody rb;

    private const int throwForce = 6;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Pick(Transform player)
    {
        rb.useGravity = false;
        transform.SetParent(player);
    }

    public void Throw(Vector3 direction)
    {
        Debug.Log(new Vector3(direction.x, 1, direction.z) * 10 / rb.mass);
        rb.useGravity = true;
        transform.SetParent(null);
        rb.AddForce(new Vector3(direction.x, 1, direction.z) * throwForce / rb.mass, ForceMode.Impulse);
    }

}
