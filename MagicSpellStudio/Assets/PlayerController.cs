using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Player { P1, P2 }

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    private CharacterController characterController;
    private Vector3 moveDirection = Vector3.zero;

    private const float speed = 6.0f;
    private float gravity = 10.0f;

    public KeyCode up;
    public KeyCode down;
    public KeyCode left;
    public KeyCode right;
    public KeyCode action;

    public Player player;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
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
            moveDirection += Vector3.forward;
        if (Input.GetKey(down))
            moveDirection -= Vector3.forward;
        if (Input.GetKey(right))
            moveDirection += Vector3.right;
        if (Input.GetKey(left))
            moveDirection -= Vector3.right;

        moveDirection *= speed;

        moveDirection.y -= gravity * Time.deltaTime;

        // Move the controller
        characterController.Move(moveDirection * Time.deltaTime);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.tag.Equals("Player"))
        {
            hit.gameObject.GetComponent<PlayerController>().ApplyForce(moveDirection * Time.deltaTime);
        }
    }
}
