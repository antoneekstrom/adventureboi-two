using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface IMovementController
{
    bool OnGround { get; }
    bool Freeze { get; set; }
    Vector2 Target { get; set; }
}
