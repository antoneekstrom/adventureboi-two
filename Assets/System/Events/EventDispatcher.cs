using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EventDispatcher : MonoBehaviour
{
    public event Action<EventAction> OnDispatch;

    public EventHandle eventHandle;

    public void Dispatch(EventAction a)
    {
        OnDispatch?.Invoke(a);
        if (eventHandle)
            eventHandle.Invoke(a);
    }

    public EventAction Dispatch(string name)
    {
        EventAction a = CreateEventAction(name);
        Dispatch(a);
        return a;
    }

    public EventAction Dispatch(StringVariable name) => Dispatch(name.Value);

    public EventAction CreateEventAction(string name) => new EventAction(name, this);
}

public class EventAction
{
    public EventDispatcher Dispatcher { get; }
    public string Name { get; }

    public EventAction(string name, EventDispatcher dispatcher)
    {
        Name = name;
        Dispatcher = dispatcher;
    }
}