using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(menuName = "System/Float Variable", fileName = "New Float")]
public class FloatVariable : ScriptableObject, IVariable<float>
{
    public float Value { get => _value; set => _value = value; }
    [SerializeField] private float _value;

    public event Action<float> OnValueChanging;

    protected void SetValue(float newValue)
    {
        OnValueChanging?.Invoke(newValue);
        _value = newValue;
    }

    public bool Compare(object other) => Value.Equals(other);
}
