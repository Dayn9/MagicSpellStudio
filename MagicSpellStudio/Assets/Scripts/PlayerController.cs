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

    private const float speed = 5.0f;
    private float gravity = 30.0f;

    private KeyCode up;
    private KeyCode down;
    private KeyCode left;
    private KeyCode right;
    private KeyCode action;

    public Player player;

    private bool holding = false;
    private List<GameObject> possiblePickups;

    private GameObject pickup;
    private bool cauldronInBounds = false;
    private Pickup pickupScript;
    private Rigidbody pickupRigidbody;

    private Vector3 pickupStartPos;
    private Vector3 pickupEndPos;
    private float lerpDeltaTime = 0;
    private float lerpTime = 0.15f;
    private WaitForFixedUpdate wait = new WaitForFixedUpdate();
    private Coroutine lerpCoroutine = null;

    private List<GameObject> deletedObjectsToRemove = new List<GameObject>();

    private MeshFilter model;
    [SerializeField] private Mesh Mup = null;
    [SerializeField] private Mesh Mdown = null;

    public bool Holding
    {
        set
        {
            holding = value;
            if (!holding)
            {
                StopCoroutine(lerpCoroutine);
            }
        }
    }


    private void Awake()
    {
        characterController = GetComponent<CharacterController>();

        possiblePickups = new List<GameObject>();

        model = GetComponentInChildren<MeshFilter>();

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

    private void Update()
    {
        moveDirection.x = Mathf.Lerp(moveDirection.x, 0, 0.9f);
        moveDirection.z = Mathf.Lerp(moveDirection.z, 0, 0.9f);
        if (characterController.isGrounded)
        {
            moveDirection.y = 0;
        }

        Vector3 move = Vector3.zero;
        if (Input.GetKey(up))
        {
            move += Vector3.forward;
        }
        if (Input.GetKey(down)) {
            move -= Vector3.forward;
        }
        if (Input.GetKey(right)) {
            move += Vector3.right;
        }
        if (Input.GetKey(left)) {
            move -= Vector3.right;
        }
        if(move != Vector3.zero)
        {
            trueDirection = move;
        }

        moveDirection += move.normalized;

        if(Time.timeScale != 0)
        {
            model.transform.LookAt(gameObject.transform.position + new Vector3(-moveDirection.z, 0, moveDirection.x));
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

        if(holding && pickupScript != null)
        {
            moveDirection.x *= speed - pickupScript.Mass;
            moveDirection.z *= speed - pickupScript.Mass;
            moveDirection.y -= (gravity + pickupScript.Mass * 10) * Time.deltaTime;
        }
        else
        {
            moveDirection.x *= speed;
            moveDirection.z *= speed;
            moveDirection.y -= gravity * Time.deltaTime;

            if (pickup != null)
            {
                pickup = null;
                pickupScript = null;
                model.mesh = Mdown;
                StopCoroutine(lerpCoroutine);
            }
        }

        // Move the controller
        characterController.Move(moveDirection * Time.deltaTime);
    }

    public void Throw()
    {
        if(holding && pickup != null)
        {
            pickupRigidbody.isKinematic = false;
            pickupScript.Throw(trueDirection, cauldronInBounds);
            //possiblePickups.Remove(pickup);
            holding = false;
            pickup = null;

            if (pickupScript.GetType().Equals(typeof(Cauldron)))
            {
                cauldronInBounds = false;
            }

            StopCoroutine(lerpCoroutine);

            model.mesh = Mdown;
        }
    }

    public void Pickup()
    {
        float closestDistance = 100;
        GameObject closest = null;

        foreach(GameObject g in possiblePickups)
        {
            if(g == null)
            {
                deletedObjectsToRemove.Add(g);
            }
            else
            {
                float dist = (g.transform.position - transform.position).sqrMagnitude;

                if (dist < closestDistance)
                {
                    closest = g;
                    closestDistance = dist;
                }
            }
        }

        for(int i = 0; i < deletedObjectsToRemove.Count; i++)
        {
            possiblePickups.Remove(deletedObjectsToRemove[i]);
        }
        deletedObjectsToRemove.Clear();

        if(closest != null)
        {
            holding = true;
            pickup = closest;
            model.mesh = Mup;
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
            if(other.gameObject.GetComponent<Cauldron>() != null)
            {
                cauldronInBounds = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag.Equals("Pickup") && possiblePickups.Contains(other.gameObject))
        {
            possiblePickups.Remove(other.gameObject);
            if (other.gameObject.GetComponent<Cauldron>() != null)
            {
                cauldronInBounds = false;
            }
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
            pickupEndPos = new Vector3(0, 0.85f - pickupCol.center.y + (pickupCol.size.y / 2), 0);
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
