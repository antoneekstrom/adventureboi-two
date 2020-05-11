using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Effects/Camera Shake", fileName = "New Camera Shake")]
public class CameraShakeAsset: EffectAsset
{
    public float strength = 5f;
    public float duration = 0.5f;

    public override void Trigger(EffectTools et)
    {
        CameraShaker.Shake(strength, duration);
    }
}
