using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator), typeof(Collider2D))]
public class PlayerController : MonoBehaviour
{
    [Header("General")]
    public float movementVelocity = 10f;
    public float inAirMovementModifier = 0.5f;

    [Header("Dropkick")]
    public float dropkickForce = 100f;
    public Vector2 dropkickBoost = Vector2.zero;

    [Header("Jump")]
    public float jumpVelocity = 12f;
    public float jumpFallModifier = 1.2f;
    public float jumpHoldModifier = 0.05f;
    public float gravityModifier = 1.2f;
    public float groundCheckOffset = 0.11f;

    [Header("Keybindings")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode dropkickKey = KeyCode.E;

    public event Action OnLandOnGround;

    public bool IsHoldingJump { get; private set; }
    public bool IsFalling { get; private set; }
    public bool OnGround { get; private set; }

    private bool isDropkicking = false;
    private Vector2 input;

    private Collider2D col;
    private Animator animator;
    public Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        col = GetComponent<Collider2D>();

        OnLandOnGround += PlayerController_OnLandOnGround;
    }

    private void Reset()
    {
        var rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
    }

    private void Update()
    {
        ComputeOnGround();
        UpdateInput();

        UpdateMovement();
        UpdateJump();
        UpdateDropkick();

        UpdateAnimator();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDropkicking)
        {
            collision.collider.attachedRigidbody.AddForce(rb.velocity * dropkickForce);
        }
    }

    private void PlayerController_OnLandOnGround()
    {
        isDropkicking = false;
    }

    private void UpdateDropkick()
    {
        if (Input.GetKeyDown(dropkickKey) && OnGround)
        {
            isDropkicking = true;
            DoJump();
            rb.velocity += new Vector2(input.x, 1) * dropkickBoost;
        }
    }

    private void UpdateMovement()
    {
        rb.velocity += Vector2.right * (input * movementVelocity).x * Time.fixedDeltaTime;
    }

    private void DoJump()
    {
        rb.velocity += (Vector2.up * jumpVelocity);
    }

    private void UpdateJump()
    {
        IsFalling = rb.velocity.y < 0;
        float jumpModifier = IsFalling ? jumpFallModifier : (IsHoldingJump ? jumpHoldModifier : 1);
        rb.AddForce(Physics2D.gravity * rb.mass * gravityModifier * jumpModifier);

        if (Input.GetKeyDown(jumpKey) && OnGround)
            DoJump();
    }

    private void ComputeOnGround()
    {
        float d2g = col.bounds.extents.y + groundCheckOffset;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, d2g);

        bool oldOnGround = OnGround;
        OnGround = hit.collider != null;

        if (!oldOnGround && OnGround)
            OnLandOnGround?.Invoke();

        Debug.DrawRay(transform.position, Vector2.down * hit.distance, OnGround ? Color.green : Color.red);
    }

    private void UpdateInput()
    {
        input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        IsHoldingJump = Input.GetKey(jumpKey);
    }

    private void UpdateAnimator()
    {
        animator.SetFloat("VelocityX", rb.velocity.x);
        animator.SetFloat("VelocityY", rb.velocity.y);
        animator.SetFloat("VelocityMag", rb.velocity.magnitude);
        animator.SetBool("OnGround", OnGround);
        animator.SetBool("IsHoldingJump", IsHoldingJump);
        animator.SetFloat("InputX", input.x);
        animator.SetFloat("InputY", input.y);
        animator.SetBool("IsDropkicking", isDropkicking);
    }

}
