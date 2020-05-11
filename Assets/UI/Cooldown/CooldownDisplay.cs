using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CooldownDisplay : MonoBehaviour
{
    public FloatVariable cooldown;

    private Image img;

    private void Awake()
    {
        img = GetComponent<Image>();
    }

    private void Update()
    {
        img.color = new Color(img.color.r, img.color.g, img.color.b, cooldown.Value < 1 ? 0.4f : 1f);
    }
}
