using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Effects/Slow Time", fileName = "New Slow Time Asset")]
public class SlowTimeAsset : EffectAsset
{
    public float duration = 0.5f;
    public float timeModifier = 0.5f;

    public override void Trigger(EffectTools et)
    {
        et.SlowTime(duration, timeModifier);
    }
}
