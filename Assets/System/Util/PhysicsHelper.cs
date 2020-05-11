using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class PhysicsHelper
{
    public static bool OnGroundRaycast(Rigidbody2D rb, Collider2D col, float distanceOffset = 1.5f)
    {
        float d2g = col.bounds.extents.y + distanceOffset;
        RaycastHit2D hitLeft = Physics2D.Raycast(rb.position - Vector2.right * (col.bounds.extents.x / 2), Vector2.down, d2g);
        RaycastHit2D hitRight = Physics2D.Raycast(rb.position + Vector2.right * (col.bounds.extents.x / 2), Vector2.down, d2g);

        return (hitRight.collider != null && !hitRight.collider.isTrigger) || (hitLeft.collider != null && !hitLeft.collider.isTrigger);
    }

    public static bool OnGroundFast(Rigidbody2D rb, float tolerance = 0.1f)
    {
        return Mathf.Abs(rb.velocity.y) < tolerance;
    }

    public static bool IsWall(this Rigidbody2D rb, Vector2 direction, float distance = 2.5f)
    {
        RaycastHit2D hit = Physics2D.Raycast(rb.position, direction, distance);
        return hit.collider && !hit.collider.isTrigger;
    }


    public static bool FindGap(this Rigidbody2D rb, Collider2D col, Vector2 direction, out Gap gap, float distance = 1f, float heightOffset = 1.5f)
    {
        Vector2 point = rb.position + direction * distance + Vector2.down * (col.bounds.extents.y + heightOffset);
        Collider2D ground = Physics2D.OverlapPoint(point);
        bool isGround = ground;

        if (!isGround)
        {
            RaycastHit2D end = Physics2D.Raycast(point, direction);
            RaycastHit2D bottom = Physics2D.Raycast(point, Vector2.down, 10f);

            gap = new Gap(point, end.collider ? end.point : point, bottom.distance, bottom.collider);
        }
        else
            gap = new Gap(point, point, 0, false);

        return !isGround;
    }

    public static bool FindWall(this Rigidbody2D rb, Collider2D col, Vector2 direction, out Wall wall, float distance = 3f)
    {
        RaycastHit2D hit = Physics2D.Raycast(rb.position + Vector2.down * col.bounds.extents.y * 0.5f, direction, distance);
        bool isWall = hit.collider && !hit.collider.isTrigger;

        if (isWall)
        {
            RaycastHit2D top = Physics2D.Raycast(hit.point + direction * 0.1f + Vector2.up * 15f, Vector2.down);

            float distanceToWall = hit.point.x - rb.position.x - col.bounds.extents.x;
            float height = top.point.y - hit.point.y;

            wall = new Wall(hit.point, top.collider ? height : 0f, distanceToWall);
        }
        else
            wall = new Wall(rb.position + direction * distance, 0, 0);

        return isWall;
    }
}

public struct Wall
{
    public Vector2 Start { get; private set; }
    public float Height { get; private set; }
    public float Distance { get; private set; }

    public Wall(Vector2 start, float height, float distance)
    {
        Start = start;
        Height = height;
        Distance = distance;
    }
}

public struct Gap
{
    public Vector2 Start { get; private set; }
    public Vector2 End { get; private set; }
    public float Depth { get; private set; }
    public bool HasBottom { get; private set; }
    public float Distance => Mathf.Abs(End.x - Start.x);

    public Gap(Vector2 start, Vector2 end, float depth, bool hasBottom)
    {
        Start = start;
        End = end;
        Depth = depth;
        HasBottom = hasBottom;
    }
}