using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator), typeof(Collider2D))]
public class PlayerController : MonoBehaviour
{
    [Header("General")]
    public float movementSpeed = 4f;
    public float gravity = 16f;
    public float maxVelocity = 15f;

    [Header("Jump")]
    public float jumpVelocity = 3f;
    public float jumpFallMultiplier = 0.1f;
    public float jumpHoldModifier = 0.7f;
    public KeyCode jumpKey = KeyCode.Space;

    public bool OnGround { get; private set; }
    public Vector2 Velocity => _velocity;
    public Vector2 VelocityPerSecond => _velocity / Time.fixedDeltaTime;

    private bool prevOnGround = false;
    private Vector2 _velocity;
    private Vector2 input;

    private Collider2D col;
    private Animator animator;
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        col = GetComponent<Collider2D>();
    }

    private void Reset()
    {
        var rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        rb.freezeRotation = true;
    }

    private void Update()
    {
        ComputeOnGround();
        UpdateInput();

        UpdateMovement();
        UpdateJump();

        UpdateAnimator();
    }

    private void FixedUpdate()
    {
        UpdatePhysics();
    }

    private void UpdatePhysics()
    {
        if (!OnGround)
            _velocity += Vector2.down * gravity * Time.fixedDeltaTime;
        else if (!prevOnGround && OnGround)
            _velocity.y = 0;

        if (VelocityPerSecond.magnitude > maxVelocity)
            _velocity = _velocity.normalized * maxVelocity * Time.fixedDeltaTime;

        rb.MovePosition(transform.position + (Vector3)_velocity);
    }

    private void UpdateMovement()
    {
        _velocity.x = (input * movementSpeed).x * Time.fixedDeltaTime;
    }

    private void UpdateJump()
    {
        if (Input.GetKeyDown(jumpKey) && OnGround)
        {
            _velocity += Vector2.up * jumpVelocity;
        }
    }

    private void ComputeOnGround()
    {
        Vector2 offset = Vector2.down * (col.bounds.extents.y + 0.1f);
        Vector2 origin = (Vector2)transform.position + offset;
        Vector2 direction = Vector2.up;
        RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.down, 0.1f);

        prevOnGround = OnGround;
        OnGround = hit.collider;

        if (Input.GetKeyDown(KeyCode.E))
            Debug.Log(hit.collider.name);

        Debug.DrawRay(origin, origin + direction * hit.distance, OnGround ? Color.green : Color.red);
    }

    private void UpdateInput()
    {
        input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    }

    private void UpdateAnimator()
    {
        animator.SetFloat("VelocityMag", _velocity.magnitude);
        animator.SetFloat("MovementX", input.x);
        animator.SetFloat("MovementY", input.y);
    }

}
