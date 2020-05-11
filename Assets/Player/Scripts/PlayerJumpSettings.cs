using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Properties/Jump Settings", fileName = "New Jump Settings")]
public class PlayerJumpSettings : ScriptableObject
{
    public float jumpVelocity = 12f;
    public float jumpFallModifier = 1.2f;
    public float jumpHoldModifier = 0.05f;
    public float gravityModifier = 1.2f;
    public float groundCheckOffset = 0.11f;
    public AnimationEffect landingEffect;
    public float landingEffectCooldown = 0.5f;
}
