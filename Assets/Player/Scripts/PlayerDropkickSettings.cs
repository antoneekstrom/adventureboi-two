using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Properties/Dropkick Settings", fileName = "New Dropkick Settings")]
public class PlayerDropkickSettings : ScriptableObject
{
    public FloatVariable cooldownPercentageStore;
    public StringVariable onDropkickEvent;
    public float dropkickCooldown = 0.5f;
    public float dropkickTorque = 1f;
    public float dropkickSpeedModifier = 1.2f;
    public Vector2 dropkickForce = new Vector2(30, 30);
    public Vector2 dropkickBoost = Vector2.zero;
    public GameObject dropkickEffect;
    public float dropkickSlowTimeDuration = 0.15f;
    public float dropkickTimeModifier = 0.4f;
}
