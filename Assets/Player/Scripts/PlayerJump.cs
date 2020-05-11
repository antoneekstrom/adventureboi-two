using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerJump : MonoBehaviour
{
    public PlayerJumpSettings settings;

    public bool IsFalling { get; private set; }
    public bool OnGround { get; private set; }
    public float TimeOfLanding { get; private set; }
    public bool IsHoldingJump => Input.GetKey(props.keybindings.jumpKey);

    public event Action OnContactGround;

    private Collider2D col;
    private Player props;
    private Rigidbody2D rb;
    private Animator animator;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        props = GetComponent<Player>();
        col = GetComponent<Collider2D>();
    }

    private void Start()
    {
        animator = props.Animator;
    }

    private void Update()
    {
        ComputeOnGround();
        ComputeIsFalling();
        UpdateJump();

        animator.SetBool("OnGround", OnGround);
        animator.SetBool("IsFalling", IsFalling);
        animator.SetBool("IsHoldingJump", IsHoldingJump);
    }

    private void UpdateJump()
    {
        float jumpModifier = IsFalling ? settings.jumpFallModifier : (IsHoldingJump ? settings.jumpHoldModifier : 1);
        rb.AddForce(Physics2D.gravity * rb.mass * settings.gravityModifier * jumpModifier);

        if (Input.GetKeyDown(props.keybindings.jumpKey) && OnGround)
            Jump();
    }

    private void ComputeIsFalling() => IsFalling = rb.velocity.y < -0.01f && !OnGround;

    public void Jump()
    {
        rb.velocity += (Vector2.up * settings.jumpVelocity);
    }

    private void ComputeOnGround()
    {
        float d2g = col.bounds.extents.y + settings.groundCheckOffset;
        RaycastHit2D hitLeft = Physics2D.Raycast(rb.position - Vector2.right * (col.bounds.extents.x / 2), Vector2.down, d2g);
        RaycastHit2D hitRight = Physics2D.Raycast(rb.position + Vector2.right * (col.bounds.extents.x / 2), Vector2.down, d2g);

        bool oldOnGround = OnGround;
        OnGround = (hitRight.collider != null && !hitRight.collider.isTrigger) || (hitLeft.collider != null && !hitLeft.collider.isTrigger);

        if (!oldOnGround && OnGround)
        {
            if (Time.time - TimeOfLanding >= settings.landingEffectCooldown)
                settings.landingEffect.Create(props.EffectTools);

            TimeOfLanding = Time.time;

            OnContactGround?.Invoke();
        }
    }

}
