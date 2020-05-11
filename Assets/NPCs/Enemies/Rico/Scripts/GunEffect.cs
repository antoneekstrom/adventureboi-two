using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunEffect : MonoBehaviour
{
    public StringVariable onHitAction;
    public StringVariable onMissAction;

    [HideInInspector]
    public EventDispatcher dispatcher;

    private void Awake()
    {
        dispatcher = GetComponent<EventDispatcher>();
    }

    private void OnDestroy()
    {
        dispatcher.Dispatch(new GunEffectAction(onMissAction.Value, dispatcher, null));
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.TryGetComponent(out Player p))
        {
            dispatcher.Dispatch(new GunEffectAction(onHitAction.Value, dispatcher, p));
            Destroy(gameObject);
        }
    }
}

public class GunEffectAction : EventAction
{
    public Player player;
    public GunEffectAction(string name, EventDispatcher d, Player player) : base(name, d)
    {
        this.player = player;
    }
}
