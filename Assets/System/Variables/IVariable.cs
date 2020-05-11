using System;
using UnityEngine;

public interface IVariable<T>
{
    T Value { get; set; }

    event Action<T> OnValueChanging;
}
