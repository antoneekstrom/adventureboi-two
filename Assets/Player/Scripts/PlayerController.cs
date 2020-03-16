using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator), typeof(Collider2D))]
public class PlayerController : MonoBehaviour
{
    [Header("General")]
    public float movementSpeed = 4f;
    public float mass = 2f;
    public float gravity = 16f;
    public float maxVelocity = 15f;

    [Header("Jump")]
    public float initialJumpForce = 100f;
    public float jumpImpactDuration = 0.1f;
    public float jumpGravityModifier = 0.7f;
    public KeyCode jumpKey = KeyCode.Space;

    private bool jumpFrame = false;
    private bool jumpDown = false;
    private bool canJump = true;
    private float jumpStartTime;

    public Vector2 Velocity => velocity;
    public Vector2 VelocityPerSecond => velocity / Time.fixedDeltaTime;
    public bool JumpGravModEnable => jumpDown && velocity.y > 0.01;
    private float JumpTime => Time.time - jumpStartTime;

    private Vector2 input;
    private Vector2 velocity;

    private Animator animator;
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Reset()
    {
        var rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        rb.freezeRotation = true;
    }

    private void Update()
    {
        UpdateInput();
        UpdateAnimator();
    }

    private void FixedUpdate()
    {
        UpdatePhysics();
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        velocity.y = 0;
        canJump = true;
    }

    private void UpdatePhysics()
    {
        rb.MovePosition(transform.position + (Vector3)ComputeVelocity());
    }

    private void UpdateInput()
    {
        input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        jumpDown = Input.GetKey(jumpKey);
        jumpFrame = Input.GetKeyDown(jumpKey);

        if (jumpFrame && canJump)
        {
            canJump = false;
            jumpStartTime = Time.time;
        }
    }

    private void UpdateAnimator()
    {
        animator.SetFloat("VelocityMag", velocity.magnitude);
        animator.SetFloat("MovementX", input.x);
        animator.SetFloat("MovementY", input.y);
    }

    private float ComputeJumpAcceleration()
    {
        return JumpTime <= jumpImpactDuration ?
            (initialJumpForce / mass) * Time.fixedDeltaTime : 0;
    }

    private Vector2 ComputeVelocity()
    {
        float vx = (input * movementSpeed).x * Time.fixedDeltaTime;

        float dvy =
            ComputeJumpAcceleration()
            -ComputeGravityAcceleration();
        float vy = velocity.y + dvy * Time.fixedDeltaTime;

        velocity = new Vector2(vx, vy);

        if (VelocityPerSecond.magnitude > maxVelocity)
            velocity = velocity.normalized * maxVelocity * Time.fixedDeltaTime;

        return velocity;
    }

    private float ComputeGravityAcceleration()
    {
        return gravity * Time.fixedDeltaTime * (JumpGravModEnable ? jumpGravityModifier : 1);
    }

}
