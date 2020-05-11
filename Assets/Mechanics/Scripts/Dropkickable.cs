using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dropkickable : MonoBehaviour
{
    private EventDispatcher ed;

    private void Awake()
    {
        ed = GetComponent<EventDispatcher>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerDropkick pd))
        {
            if (pd.TriggerDropkick(this, collision.contacts[0].point))
                ed.Dispatch(new DropkickEvent(pd, pd.settings.onDropkickEvent, ed));
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerDropkick pd))
        {
            if (pd.TriggerDropkick(this, transform.position))
                ed.Dispatch(new DropkickEvent(pd, pd.settings.onDropkickEvent, ed));
        }
    }
}

public class DropkickEvent : EventAction
{

    public PlayerDropkick Player { get; private set; }

    public DropkickEvent(PlayerDropkick player, string name, EventDispatcher dispatcher) : base(name, dispatcher)
    {
        Player = player;
    }
}