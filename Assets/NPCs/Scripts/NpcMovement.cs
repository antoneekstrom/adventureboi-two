using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class NpcMovement : MonoBehaviour, IMovementController
{
    public bool Freeze { get => freeze; set => freeze = value; }
    public bool OnGround => OnGroundCached;

    public NpcMovementSettings settings;
    public bool freeze = false;

    public Vector2 Target { get; set; }
    public bool IsSprinting { get; set; }

    public Vector2 MovementDirection { get; private set; }
    public bool OnGroundCached { get; private set; }
    public bool IsGapCached { get; private set; }
    public bool IsWallCached { get; private set; }

    public bool CanJump => Time.time - timeOfJump >= settings.jumpCooldown;
    public float GapDistance => _gapCached.Distance;
    public float WallHeight => _wallCached.Height;

    private float stepBackFromWallTime = 0.5f;
    private float steppedBackFromWall = 0f;
    private bool stepBackFromWall;

    private float timeOfJump;
    private Gap _gapCached;
    private Wall _wallCached;

    public Vector2 TargetDistance => Target - rb.position;
    public bool ReachedTarget => TargetDistance.sqrMagnitude <= 1.5f;

    private Animator animator;
    private Collider2D col;
    private Rigidbody2D rb;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
    }

    private void Update()
    {
        UpdateAnimator();
    }

    private void OnDrawGizmosSelected()
    {
        if (!col) return;

        // rb to gapstart
        Gizmos.color = IsGapCached ? Color.red : Color.green;
        Gizmos.DrawLine(rb.position, _gapCached.Start);

        if (IsGapCached)
        {
            // gap distance
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(_gapCached.Start, _gapCached.End);

            // gap depth
            Gizmos.color = Color.green;
            Gizmos.DrawLine(_gapCached.Start, _gapCached.Start + Vector2.down * _gapCached.Depth);
        }

        // wall detection range
        Gizmos.color = IsWallCached ? Color.red : Color.green;
        Gizmos.DrawLine(rb.position, _wallCached.Start);

        if (IsWallCached)
        {
            // wall height
            Gizmos.color = Color.red;
            Gizmos.DrawLine(_wallCached.Start, _wallCached.Start + Vector2.up * _wallCached.Height);
        }
    }   

    public void ToggleFreeze() => freeze = !freeze;

    private void FixedUpdate()
    {
        UpdateProperties();

        if (freeze) return;

        UpdateMovement();
        UpdateJump();
    }

    protected void UpdateProperties()
    {
        MovementDirection = (Vector2.right * TargetDistance).normalized;
        OnGroundCached = PhysicsHelper.OnGroundRaycast(rb, col, settings.onGroundTolerance);

        float wallRange = Mathf.Min(2.5f + 0.2f * Mathf.Abs(rb.velocity.x), settings.maxWallRange);
        float gapRange = Mathf.Min(col.bounds.extents.x + 0.3f + 0.3f * Mathf.Abs(rb.velocity.x), settings.maxGapRange);

        IsWallCached = rb.FindWall(col, MovementDirection.Direction() * Vector2.right, out _wallCached, distance: wallRange);
        IsGapCached = rb.FindGap(col, MovementDirection.Direction() * Vector2.right, out _gapCached, distance: gapRange);
    }

    protected void UpdateJump()
    {
        if (!OnGroundCached)
            rb.AddForce(Vector2.up * Physics2D.gravity.y * (settings.inAirGravityModifier - 1));

        if (CanJump && OnGroundCached && (IsGapCached || IsWallCached))
        {
            float f = (IsGapCached ? JumpGap() : 0) + (IsWallCached ? JumpWall() : 0);

            if (f == 0) return;
            timeOfJump = Time.time;

            f = Mathf.Min(f, settings.maxJumpForce);
            rb.AddForce(Vector2.up * f, ForceMode2D.Impulse);
        }
    }

    protected float JumpGap()
    {
        float jf = JumpDistanceToForce(_gapCached.Distance);

        if (_gapCached.Depth <= 3f && _gapCached.HasBottom)
            jf = JumpHeightToForce(0.5f);

        return jf;
    }

    protected float JumpWall()
    {
        if (TimeToMoveDistance(_wallCached.Distance) - TimeToJumpHeight(_wallCached.Height) < 0.1f) return 0f;

        float wf = (IsWallCached && _wallCached.Distance <= 0.2f) ? 1.55f : 1f;
        float jf = JumpHeightToForce(_wallCached.Height) * 1.3f;

        return jf * wf;
    }

    protected void UpdateMovement()
    {
        Vector2 desiredVel = Vector2.right * (MovementDirection * settings.movementVelocity).x;
        Vector2 dVel = desiredVel - rb.velocity;

        if (!ReachedTarget && ((OnGroundCached && !IsGapCached) || !OnGroundCached && IsGapCached))
        {
            rb.velocity += (dVel.magnitude > 0.01f ? dVel : Vector2.zero)
                           * (OnGroundCached ? 1 : settings.inAirMovementModifier)
                           * (IsSprinting ? settings.sprintModifier : 1f)
                           * Time.fixedDeltaTime;

            if (OnGroundCached)
            {
                if (IsWallCached && _wallCached.Distance < 0.15f)
                    stepBackFromWall = true;

                if ((IsWallCached && _wallCached.Distance > 0.7f) || !IsWallCached)
                    stepBackFromWall = false;

                if (stepBackFromWall)
                    rb.velocity *= (Vector2.up + Vector2.left);
            }
        }
        else if (OnGroundCached && !IsGapCached)
        {
            rb.velocity *= Vector2.up;
        }
    }

    /// <summary>
    /// How far one falls during a given timeframe.
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    protected float DistanceToFallTime(float time)
    {
        return -Physics2D.gravity.y * time * time * 0.5f;
    }

    /// <summary>
    /// The time it takes to move a distance.
    /// </summary>
    /// <param name="distance"></param>
    /// <returns></returns>
    protected float TimeToMoveDistance(float distance)
    {
        return distance / rb.velocity.x;
    }

    /// <summary>
    /// The force which makes one jump a certain height.
    /// </summary>
    /// <param name="height"></param>
    /// <returns></returns>
    protected float JumpHeightToForce(float height)
    {
        float v = Mathf.Sqrt(2 * height * -Physics2D.gravity.y);
        float f = rb.mass * settings.inAirGravityModifier * 0.7f;
        return v * f;
    }

    /// <summary>
    /// The force it takes to make one jump a certain distance.
    /// </summary>
    /// <param name="distance"></param>
    /// <returns></returns>
    protected float JumpDistanceToForce(float distance)
    {
        float jumpHeight = DistanceToFallTime(TimeToMoveDistance(distance));
        float jumpForce = JumpHeightToForce(jumpHeight);
        return jumpForce;
    }

    /// <summary>
    /// The time it takes for a jump to reach a certain height.
    /// </summary>
    /// <param name="height"></param>
    /// <returns></returns>
    public float TimeToJumpHeight(float height)
    {
        return height * 0.1f;
    }

    protected bool IsGap(Vector2 direction, float distance = 2f, float heightOffset = 2f) => IsGap(direction, out Vector2 _, distance: distance, heightOffset: heightOffset);

    protected bool IsGap(Vector2 direction, out Vector2 point, float distance = 2f, float heightOffset = 2f)
    {
        point = rb.position + direction * distance + Vector2.down * (col.bounds.extents.y + heightOffset);
        return !Physics2D.OverlapPoint(point);
    }

    protected void UpdateAnimator()
    {
        animator.SetFloat("VelocityX", rb.velocity.x);
        animator.SetFloat("VelocityY", rb.velocity.y);
        animator.SetFloat("VelocityMag", rb.velocity.magnitude);
        animator.SetBool("OnGround", OnGroundCached);
    }
}
