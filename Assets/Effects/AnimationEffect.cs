using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Effects/Effect Data", fileName = "New Effect Data")]
public class AnimationEffect : ScriptableObject
{
    public string effectName;
    public GameObject effect;
    public Vector3 offset;
    public Transform origin;
    public Transform parent;
    public bool useCreatorAsParent = false;

    public GameObject Create(EffectTools e)
    {
        GameObject instance = Instantiate(effect);
        instance.transform.position = (origin ? origin.position : e.transform.position) + offset;

        if (useCreatorAsParent)
            instance.transform.SetParent(e.transform);
        else if (parent)
            instance.transform.SetParent(parent);

        if (effectName != null && effectName != "") instance.name = effectName;

        return instance;
    }

}