using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Properties/Movement Settings", fileName = "New Movement Settings")]
public class PlayerMovementSettings : ScriptableObject
{
    public float movementVelocity = 10f;
    public float runModifier = 1.5f;
    public float inAirMovementModifier = 0.5f;
    public bool constantVelocity = true;
}
