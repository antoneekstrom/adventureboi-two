using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(menuName = "System/String Variable", fileName = "New String")]
public class StringVariable : ScriptableObject, IVariable<string>
{
    public virtual string Value { get => _value; set => _value = value; }
    [SerializeField] private string _value;

    public event Action<string> OnValueChanging;

    protected void SetValue(string newValue)
    {
        OnValueChanging?.Invoke(newValue);
        _value = newValue;
    }

    public void Print() => Debug.Log(Value);


    public static implicit operator string(StringVariable sv) => sv.Value;
    public override bool Equals(object other) => Value.Equals(other);
    public override int GetHashCode() => base.GetHashCode();
    public static bool operator ==(StringVariable v, string s) => v.Equals(s);
    public static bool operator !=(StringVariable v, string s) => !v.Equals(s);
}
