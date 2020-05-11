using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(Collider2D))]
public class ItemPickup : MonoBehaviour
{
    public Item item;
    public GameObject pickupEffect;

    public event Action<Player> OnPickup;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.TryGetComponent(out Player p))
        {
            GameObject fx = Instantiate(pickupEffect);
            fx.transform.position = transform.position;

            OnPickup?.Invoke(p);
            p.OnPickupTrigger(this);

            Destroy(gameObject);
        }
    }
}
