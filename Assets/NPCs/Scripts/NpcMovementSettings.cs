using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Properties/NPC Movement Settings", fileName = "New NPC Movement Settings")]
public class NpcMovementSettings : ScriptableObject
{
    [Header("Movement")]
    public float movementVelocity = 10f;
    public float sprintModifier = 1.5f;
    public float inAirMovementModifier = 0.5f;

    [Header("Senses")]
    public float maxGapRange = 10f;
    public float wallRange = 5f;
    public float maxWallRange = 15f;
    public float onGroundTolerance = 0.1f;

    [Header("Jump")]
    public float maxJumpForce = 250f;
    public float jumpCooldown = 0.1f;
    public float inAirGravityModifier = 1.5f;
}
