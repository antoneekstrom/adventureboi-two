using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Properties/Player Settings", fileName = "New Player Settings")]
public class PlayerKeybindings : ScriptableObject
{
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode dropkickKey = KeyCode.E;
    public KeyCode runKey = KeyCode.LeftShift;
}
