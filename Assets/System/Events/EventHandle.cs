using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(menuName = "System/Event Handle", fileName = "New Event Handle")]
public class EventHandle : ScriptableObject
{
    public event Action<EventAction> OnEvent;

    public void Invoke(EventAction a) => OnEvent?.Invoke(a);
}