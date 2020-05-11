using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "System/String Constant", fileName = "New String Constant")]
public class StringConstant : StringVariable
{
    public override string Value { get => name; set => name = value; }
}
