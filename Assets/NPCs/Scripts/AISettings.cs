using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Properties/AI Settings", fileName = "New AI Settings")]
public class AISettings : ScriptableObject
{
    [Header("Senses")]
    public float detectionRange = 10f;
}
