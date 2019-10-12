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

    private MeshFilter model;
    [SerializeField] private Mesh Mup;
    [SerializeField] private Mesh Mdown;


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

        model.transform.LookAt(gameObject.transform.position + new Vector3(moveDirection.z, 0, moveDirection.x));

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

        moveDirection.x *= speed;
        moveDirection.z *= speed;

        moveDirection.y -= gravity * Time.deltaTime;

        // Move the controller
        characterController.Move(moveDirection * Time.deltaTime);

        if (holding && pickup != null)
        {
            pickup.transform.localPosition = Vector3.Lerp(pickup.transform.localPosition, Vector3.up * 2, 0.3f);
        }
    }

    public void Throw()
    {
        if(holding && pickup != null)
        {
            pickup.GetComponent<Pickup>().Throw(trueDirection, cauldronInBounds);

            holding = false;
            pickup = null;

            model.mesh = Mdown;
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
            pickup.GetComponent<Pickup>().Pick(transform);

            model.mesh = Mup;
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

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.tag.Equals("Player"))
        {
            hit.gameObject.GetComponent<PlayerController>().ApplyForce(moveDirection * Time.deltaTime);
        }
    }
}
