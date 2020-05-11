using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Toggle : MonoBehaviour
{
    public StringVariable enabledEvent, disabledEvent, stateChangedEvent;

    public bool State { get => _state; set => Set(value); }
    private bool _state;

    [SerializeField] private EventDispatcher ed;
    private Animator animator;

    public void Set(bool state)
    {
        if (!ed) ed = GetComponent<EventDispatcher>();

        _state = state;
        ed.Dispatch(ed.CreateEventAction(stateChangedEvent));
        ed.Dispatch(ed.CreateEventAction(state ? enabledEvent : disabledEvent));

        if (animator) animator.SetBool("State", state);
    }

    public void Interact() => Set(!_state);

    private void Awake()
    {
        animator = GetComponent<Animator>();
        ed = GetComponent<EventDispatcher>();
    }
}
