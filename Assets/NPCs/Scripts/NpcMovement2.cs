using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class NpcMovement2 : MonoBehaviour, IMovementController
{
    public bool Freeze { get => freeze; set => freeze = value; }
    public bool OnGround => OnGroundCached;

    public NpcMovementSettings settings;
    public bool freeze = false;

    public Vector2 Target { get; set; }
    public bool IsSprinting { get; set; }

    public Vector2 MovementDirection { get; private set; }
    public bool OnGroundCached { get; private set; }
    public bool IsGap { get; private set; }
    public bool IsWall { get; private set; }

    public bool CanJump => Time.time - timeOfJump >= settings.jumpCooldown;
    public float GapDistance => _gap.Distance;
    public float WallHeight => _wall.Height;

    private float timeOfJump;
    private Gap _gap;
    private Wall _wall;

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
        Gizmos.color = IsGap ? Color.red : Color.green;
        Gizmos.DrawLine(rb.position, _gap.Start);

        if (IsGap)
        {
            // gap distance
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(_gap.Start, _gap.End);

            // gap depth
            Gizmos.color = Color.green;
            Gizmos.DrawLine(_gap.Start, _gap.Start + Vector2.down * _gap.Depth);
        }

        // wall detection range
        Gizmos.color = IsWall ? Color.red : Color.green;
        Gizmos.DrawLine(rb.position, _wall.Start);

        if (IsWall)
        {
            // wall height
            Gizmos.color = Color.red;
            Gizmos.DrawLine(_wall.Start, _wall.Start + Vector2.up * _wall.Height);
        }
    }
    private void FixedUpdate()
    {
        // Look for walls and gaps
        UpdateSensors();
        if (freeze) return;

        // Horizontal movement velocity
        Vector2 desiredVel = Vector2.right * (MovementDirection * settings.movementVelocity).x;
        Vector2 dVel = desiredVel - rb.velocity;

        if (!ReachedTarget && (OnGroundCached || !IsGap))
        {
            rb.velocity += (dVel.magnitude > 0.01f ? dVel : Vector2.zero)
                   * (OnGroundCached ? 1 : settings.inAirMovementModifier)
                   * (IsSprinting ? settings.sprintModifier : 1f)
                   * Time.fixedDeltaTime;
        }

        // Jump walls
        if (IsWall)
        {
            float minWallDist = 0.2f;

            if (_wall.Distance <= minWallDist)
            {
                //float timeToJump = TimeToJumpHeight(_wall.Height);
                //float timeToReachWall = TimeToMoveDistance(_wall.Distance);
                //rb.AddForce(Vector2.up * JumpHeightToForce(_wall.Height));

            }
        }
    }

    protected void UpdateSensors()
    {
        MovementDirection = (Vector2.right * TargetDistance).normalized;
        OnGroundCached = PhysicsHelper.OnGroundRaycast(rb, col, settings.onGroundTolerance);

        float wallRange = 10f;
        float gapRange = 10f;

        IsWall = rb.FindWall(col, MovementDirection.Direction() * Vector2.right, out _wall, distance: wallRange);
        IsGap = rb.FindGap(col, MovementDirection.Direction() * Vector2.right, out _gap, distance: gapRange);
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
        float f = rb.mass * settings.inAirGravityModifier;
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

    protected bool DetectGap(Vector2 direction, out Vector2 point, float distance = 2f, float heightOffset = 2f)
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

    public void ToggleFreeze() => freeze = !freeze;

    protected bool DetectGap(Vector2 direction, float distance = 2f, float heightOffset = 2f) => DetectGap(direction, out Vector2 _, distance: distance, heightOffset: heightOffset);

}
