using System;
using UnityEngine;
using Random = UnityEngine.Random;

public static class VectorHelper
{
    public static Vector3 SetPosition(this Vector3 vec, Vector2 pos)
    {
        return new Vector3(pos.x, pos.y, vec.z);
    }

    public static Vector2 FacingDirection(this Transform t)
    {
        return t.localScale.x < 0 ? Vector2.left : Vector2.right;
    }

    public static Vector2 Direction(this Vector2 self) => Vector2.right * (self.x < 0 ? -1 : 1) + Vector2.up * (self.y < 0 ? -1 : 1);

    public static Vector2 RandomPointInsideCollider(this Collider2D col)
    {
        return new Vector2(Random.Range(col.bounds.min.x, col.bounds.max.x), Random.Range(col.bounds.min.y, col.bounds.max.y));
    }

}