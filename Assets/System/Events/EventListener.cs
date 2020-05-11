using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public class EventListener : MonoBehaviour
{
    public EventHandle EventTrigger { get => _eventTrigger; set => SetEventTrigger(value); }
    [HideInInspector] [SerializeField] private EventHandle _eventTrigger;

    public StringVariable actionName;

    /// <summary>
    /// Only listen to events which are dispatched by this gameObject.
    /// </summary>
    public bool triggerOnlyOnSelf = false;

    public UnityActionEvent _onAction;
    public event Action<EventAction> OnAction;

    public void Trigger(EventAction a)
    {
        if ((!triggerOnlyOnSelf || a.Dispatcher.gameObject.Equals(gameObject)) && actionName == a.Name)
        {
            _onAction.Invoke(a);
            OnAction?.Invoke(a);
        }
    }

    protected void SetEventTrigger(EventHandle h)
    {
        if (_eventTrigger)
            _eventTrigger.OnEvent -= Trigger;

        _eventTrigger = h;

        if (_eventTrigger)
            _eventTrigger.OnEvent += Trigger;
    }

    private void Awake()
    {
        if (_eventTrigger)
            _eventTrigger.OnEvent += Trigger;
    }

    private void OnDestroy()
    {
        if (_eventTrigger)
            _eventTrigger.OnEvent -= Trigger;
    }
}

[Serializable]
public class UnityActionEvent : UnityEvent<EventAction> { }
