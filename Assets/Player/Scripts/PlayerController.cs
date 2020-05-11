using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator), typeof(Collider2D))]
public class PlayerController : MonoBehaviour
{
    [Header("General")]
    public float movementVelocity = 10f;
    public float runModifier = 1.5f;
    public float inAirMovementModifier = 0.5f;

    [Header("Dropkick")]
    public float dropkickCooldown = 0.5f;
    public float dropkickTorque = 1f;
    public float dropkickSpeedModifier = 1.2f;
    public Vector2 dropkickForce = new Vector2(30, 30);
    public Vector2 dropkickBoost = Vector2.zero;

    [Header("Effects")]
    public GameObject dropkickEffect;
    public GameObject landingEffect;
    public float dropkickSlowTimeDuration = 0.15f;
    public float dropkickTimeModifier = 0.4f;

    [Header("Jump")]
    public float jumpVelocity = 12f;
    public float jumpFallModifier = 1.2f;
    public float jumpHoldModifier = 0.05f;
    public float gravityModifier = 1.2f;
    public float groundCheckOffset = 0.11f;

    [Header("Keybindings")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode dropkickKey = KeyCode.E;
    public KeyCode runKey = KeyCode.LeftShift;

    public event Action OnLandOnGround;

    public static PlayerController Instance { get; private set; }

    public bool IsRunning { get; private set; }
    public bool IsHoldingJump { get; private set; }
    public bool IsFalling { get; private set; }
    public bool OnGround { get; private set; }
    public Rigidbody2D Rigidbody { get; private set; }

    public float DropkickCooldownPercentage => Mathf.Min((Time.time - dropkickCooldownTime), dropkickCooldown) / dropkickCooldown;
    public bool CanDropkick => Time.time - dropkickCooldownTime >= dropkickCooldown;
    private bool CanApplyDropkick => isDropkicking && !hasKicked;

    private float dropkickCooldownTime;
    private bool hasKicked = false;
    private bool isDropkicking = false;
    private Vector2 input;

    private Collider2D col;
    private Animator animator;

    private void Start()
    {
        Instance = this;

        Rigidbody = GetComponent<Rigidbody2D>();
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
        if (CanApplyDropkick && collision.collider.attachedRigidbody)
        {
            hasKicked = true;

            var rb = collision.collider.attachedRigidbody;

            Vector2 dir = new Vector2(Rigidbody.velocity.normalized.x, 1);
            rb.AddForce(dropkickForce * dir * dropkickSpeedModifier, ForceMode2D.Impulse);
            rb.AddTorque(-dropkickTorque * dir.x * dropkickSpeedModifier, ForceMode2D.Impulse);

            GameObject fx = Instantiate(dropkickEffect);
            fx.transform.position = fx.transform.position.SetPosition(collision.contacts[0].point);

            this.SlowTime(dropkickSlowTimeDuration, dropkickTimeModifier);
        }
    }

    private void PlayerController_OnLandOnGround()
    {
        if (isDropkicking)
            dropkickCooldownTime = Time.time;

        isDropkicking = false;

        GameObject fx = Instantiate(landingEffect);
        fx.transform.position = fx.transform.position.SetPosition(transform.position + Vector3.down * col.bounds.extents.y);
    }

    private void UpdateDropkick()
    {
        if (Input.GetKeyDown(dropkickKey) && OnGround && CanDropkick)
        {
            isDropkicking = true;
            hasKicked = false;

            DoJump();
            Rigidbody.velocity += new Vector2(input.x, 1) * dropkickBoost;
        }
    }

    private void UpdateMovement()
    {
        Rigidbody.velocity += Vector2.right * (input * movementVelocity).x * Time.fixedDeltaTime * (OnGround ? 1 : inAirMovementModifier) * (IsRunning ? runModifier : 1);
    }

    private void DoJump()
    {
        Rigidbody.velocity += (Vector2.up * jumpVelocity);
    }

    private void UpdateJump()
    {
        IsFalling = Rigidbody.velocity.y < -0.01f && !OnGround;
        float jumpModifier = IsFalling ? jumpFallModifier : (IsHoldingJump ? jumpHoldModifier : 1);
        Rigidbody.AddForce(Physics2D.gravity * Rigidbody.mass * gravityModifier * jumpModifier);

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
        IsRunning = Input.GetKey(runKey);
    }

    private void UpdateAnimator()
    {
        animator.SetFloat("VelocityX", Rigidbody.velocity.x);
        animator.SetFloat("VelocityY", Rigidbody.velocity.y);
        animator.SetFloat("VelocityMag", Rigidbody.velocity.magnitude);
        animator.SetBool("OnGround", OnGround);
        animator.SetBool("IsFalling", IsFalling);
        animator.SetBool("IsHoldingJump", IsHoldingJump);
        animator.SetFloat("InputX", input.x);
        animator.SetFloat("InputY", input.y);
        animator.SetFloat("InputMag", input.magnitude);
        animator.SetBool("IsDropkicking", isDropkicking);
        animator.SetBool("IsRunning", IsRunning);
    }

}
