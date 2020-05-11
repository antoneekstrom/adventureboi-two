using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EffectTools : MonoBehaviour
{
    public event Action<AnimationEffect> OnEffectCreated;
    public event Action<EffectAsset> OnEffectTriggered;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }       

    public void SetTrigger(string s) => animator.SetTrigger(s);
    public void SetTrigger(StringVariable v) => SetTrigger(v.Value);

    public void TriggerEffect(EffectAsset e)
    {
        e.Trigger(this);
        OnEffectTriggered?.Invoke(e);
    }

    public void Create(AnimationEffect effect)
    {
        AnimationEffect d = Instantiate(effect);
        d.effect = effect.Create(this);
        OnEffectCreated?.Invoke(d);
    }
}
