using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] float velocityScaler = 6f;
    [SerializeField] float jumpSpeed = 5f;
    [SerializeField] float climbSpeed = 3f;
    [SerializeField] Vector2 deathKick = new Vector2(20f,20f);
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform gun;

    CapsuleCollider2D myCapsuleCollider;
    BoxCollider2D feetCollider;

    Animator animatorReference;
    Vector2 moveInput;
    float gravity;

    Rigidbody2D playerRigidBody;


    bool isAlive = true;

    void Start()
    {
        playerRigidBody = GetComponent<Rigidbody2D>();
        animatorReference = GetComponent<Animator>();
        myCapsuleCollider = GetComponent<CapsuleCollider2D>();
        feetCollider = GetComponent<BoxCollider2D>();
        gravity = playerRigidBody.gravityScale;
    }

    void Update()
    {
        if (!isAlive) { return; }
        Run();
        FlipSprite();
        ClimbLadder();
        Die();
    }

    private void FlipSprite()
    {
        bool playerHasHorizontalSpeed = MathF.Abs(playerRigidBody.velocity.x) > Mathf.Epsilon;

        if (playerHasHorizontalSpeed)
        {
            transform.localScale = new Vector2(MathF.Sign(playerRigidBody.velocity.x), 1f);
        }
    }

    private void Run()
    {
        Vector2 playerVelocity = new Vector2(moveInput.x * velocityScaler, playerRigidBody.velocity.y);
        playerRigidBody.velocity = playerVelocity;

        bool playerHasHorizontalSpeed = MathF.Abs(playerRigidBody.velocity.x) > Mathf.Epsilon;
        animatorReference.SetBool("isRunning", playerHasHorizontalSpeed);

    }

    void OnMove( InputValue value)
    {
        if (!isAlive) { return; }
        moveInput = value.Get<Vector2>();
    }

    void OnJump(InputValue value)
    {
        if (!isAlive) { return; }
        if (!feetCollider.IsTouchingLayers(LayerMask.GetMask("Ground"))) { return; }
        if (value.isPressed)
        {
            playerRigidBody.velocity += new Vector2(0, jumpSpeed);
        }
    }

    void OnFire(InputValue value)
    {
        if (!isAlive) { return; }
        if (value.isPressed)
        {
            Instantiate(bulletPrefab, gun.position, transform.rotation);
        }
    }

    void ClimbLadder()
    {
        if (!feetCollider.IsTouchingLayers(LayerMask.GetMask("Ladder"))) {
            playerRigidBody.gravityScale = gravity;
            return;
        }
        
        playerRigidBody.gravityScale = 0f;

        bool playerHasVerticalSpeed = MathF.Abs(playerRigidBody.velocity.y) > Mathf.Epsilon;
        animatorReference.SetBool("isClimbing", playerHasVerticalSpeed);

        Vector2 climbVelocity = new Vector2(playerRigidBody.velocity.x, moveInput.y * climbSpeed);
        playerRigidBody.velocity = climbVelocity;

    }

    void Die()
    {

        if(!myCapsuleCollider.IsTouchingLayers(LayerMask.GetMask("Hazards", "Enemy"))) { return; }
        isAlive = false;
        animatorReference.SetTrigger("Dying");
        playerRigidBody.velocity = deathKick;
        FindObjectOfType<GameSession>().ProcessPlayerDeath();
    }
}
