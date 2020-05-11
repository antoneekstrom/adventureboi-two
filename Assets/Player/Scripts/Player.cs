using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Player : MonoBehaviour
{
    public PlayerKeybindings keybindings;

    public event Action<ItemPickup> OnPickup;

    public EffectTools EffectTools { get; private set; }
    public PlayerDispatcher Dispatcher { get; private set; }
    public Animator Animator { get; private set; }
    public PlayerDropkick PlayerDropkick { get; private set; }

    public static IReadOnlyList<Player> ActivePlayers => _players;
    private static List<Player> _players;

    public void OnPickupTrigger(ItemPickup ip) => OnPickup?.Invoke(ip);

    private void Awake()
    {
        Animator = GetComponentInChildren<Animator>();
        EffectTools = GetComponent<EffectTools>();
        Dispatcher = GetComponent<PlayerDispatcher>();
        PlayerDropkick = GetComponent<PlayerDropkick>();

        if (_players == null)
            _players = new List<Player>();

        if (!_players.Contains(this))
            _players.Add(this);
    }

    private void OnDestroy()
    {
        _players.Remove(this);
    }

    private void Reset()
    {
        var rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
    }

    public static float ClosestPlayer(Vector2 position, out Player closestPlayer)
    {
        closestPlayer = ActivePlayers[0];
        float closestDist = Mathf.Infinity;

        foreach (Player p in ActivePlayers)
        {
            float d = ((Vector2)p.transform.position - position).sqrMagnitude;
            if (d < closestDist)
            {
                closestPlayer = p;
                closestDist = d;
                break;
            }
        }

        return closestDist;
    }

    public void Stun(float duration) => StartCoroutine(StunRoutine(duration));

    public IEnumerator StunRoutine(float duration)
    {
        if (PlayerDropkick.IsDropkicking) yield break;

        Dispatcher.Dispatch(Dispatcher.playerStunned);

        Animator.SetBool("Stunned", true);
        yield return new WaitForSeconds(duration);
        Animator.SetBool("Stunned", false);
    }
}
