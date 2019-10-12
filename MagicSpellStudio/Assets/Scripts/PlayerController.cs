using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Player { P1, P2 }

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    private CharacterController characterController;
    private Vector3 moveDirection = Vector3.zero;
    private Vector3 trueDirection = Vector3.zero;

    private const float speed = 6.0f;
    private float gravity = 10.0f;

    private KeyCode up;
    private KeyCode down;
    private KeyCode left;
    private KeyCode right;
    private KeyCode action;

    public Player player;

    private bool holding = false;
    private List<GameObject> possiblePickups;
    private GameObject pickup;
    private Pickup pickupScript;
    private Rigidbody pickupRigidbody;

    private Vector3 pickupStartPos;
    private Vector3 pickupEndPos;
    private float lerpDeltaTime = 0;
    private float lerpTime = 0.15f;
    private WaitForFixedUpdate wait = new WaitForFixedUpdate();
    private Coroutine lerpCoroutine = null;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();

        possiblePickups = new List<GameObject>();

        switch (player)
        {
            case Player.P1:
                up = KeyCode.W;
                down = KeyCode.S;
                right = KeyCode.D;
                left = KeyCode.A;
                action = KeyCode.LeftShift;
                break;
            case Player.P2:
                up = KeyCode.P;
                down = KeyCode.Semicolon;
                right = KeyCode.Quote;
                left = KeyCode.L;
                action = KeyCode.RightShift;
                break;
        }
    }

    public void ApplyForce(Vector3 force)
    {
        characterController.Move(force);
    }

    private void Update()
    {
        moveDirection.x = Mathf.Lerp(moveDirection.x, 0, 0.9f);
        moveDirection.z = Mathf.Lerp(moveDirection.z, 0, 0.9f);
        if (characterController.isGrounded)
        {
            moveDirection.y = 0;
        }
        

        if (Input.GetKey(up))
        {
            moveDirection += Vector3.forward;
            trueDirection = Vector3.forward;
        }
        if (Input.GetKey(down)) {
            moveDirection -= Vector3.forward;
            trueDirection = -Vector3.forward;
        }
        if (Input.GetKey(right)) {
            moveDirection += Vector3.right;
            trueDirection = Vector3.right;
        }
        if (Input.GetKey(left)) {
            moveDirection -= Vector3.right;
            trueDirection = -Vector3.right;
        }

        if (Input.GetKeyDown(action))
        {
            if (holding)
            {
                Throw();
            }
            else
            {
                Pickup();
            }
        }

        moveDirection *= speed;

        moveDirection.y -= gravity * Time.deltaTime;

        // Move the controller
        characterController.Move(moveDirection * Time.deltaTime);
    }

    public void Throw()
    {
        if(holding && pickup != null)
        {
            pickupRigidbody.isKinematic = false;
            pickupScript.Throw(trueDirection);
            holding = false;
            pickup = null;
            StopCoroutine(lerpCoroutine);
        }
    }

    public void Pickup()
    {
        float closestDistance = 100;
        GameObject closest = null;

        foreach(GameObject g in possiblePickups)
        {
            float dist = (g.transform.position - transform.position).sqrMagnitude;

            if (dist < closestDistance)
            {
                closest = g;
                closestDistance = dist;
            }
        }

        if(closest != null)
        {
            holding = true;
            pickup = closest;
            pickupScript = pickup.GetComponent<Pickup>();
            pickupScript.Pick(transform);
            lerpCoroutine = StartCoroutine(PickupLerp());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Pickup") && !possiblePickups.Contains(other.gameObject))
        { 
            possiblePickups.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag.Equals("Pickup") && possiblePickups.Contains(other.gameObject))
        {
            possiblePickups.Remove(other.gameObject);
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.tag.Equals("Player"))
        {
            hit.gameObject.GetComponent<PlayerController>().ApplyForce(moveDirection * Time.deltaTime);
        }
    }
    private IEnumerator PickupLerp()
    {
        lerpDeltaTime = 0;
        pickupStartPos = pickup.transform.localPosition;
        pickupRigidbody = pickup.GetComponent<Rigidbody>();
        pickupRigidbody.isKinematic = true;
        BoxCollider pickupCol = pickup.GetComponent<BoxCollider>();
        if(pickupCol != null)
        {
            pickupEndPos = new Vector3(0, 1 - pickupCol.center.y + (pickupCol.size.y / 2), 0);
        }
        else
        {
            pickupEndPos = Vector3.up * 2;
        }
        while(lerpDeltaTime < lerpTime)
        {
            pickup.transform.localPosition = Vector3.Lerp(pickupStartPos, pickupEndPos, lerpDeltaTime / lerpTime);
            yield return wait;
            lerpDeltaTime += Time.deltaTime;
        }
        pickup.transform.localPosition = pickupEndPos;
    }
}
