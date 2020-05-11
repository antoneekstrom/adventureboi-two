using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EffectAsset : ScriptableObject
{
    public void Trigger(EventAction a) => Trigger(a.Dispatcher.GetComponent<EffectTools>());

    public abstract void Trigger(EffectTools et);
}