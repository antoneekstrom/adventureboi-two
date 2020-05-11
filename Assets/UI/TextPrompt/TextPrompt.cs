using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TextPrompt : MonoBehaviour
{
    public StringVariable text;
    public Transform parent;
    public Vector2 sideOfParent;
    public Vector2 offset;
    public bool cameraAware = false;
    public bool isUi = false;

    private Transform rect;
    private TMP_Text tmp;
    private Vector2 originSideOfParent;
    private Vector2 extents;

    private void Awake()
    {
        tmp = GetComponentInChildren<TMP_Text>();
        extents = GetComponent<Image>().sprite.bounds.extents;
        rect = isUi ? GetComponent<RectTransform>() : transform;
        originSideOfParent = sideOfParent;
    }

    private void Update()
    {
        rect.position = GetScreenPosition();
        tmp.text = text.Value;
    }

    public Vector2 GetScreenPosition()
    {
        Vector2 worldPos = (Vector2)parent.position + sideOfParent * (extents * 2) + sideOfParent * offset;
        Vector2 screenPos = Camera.main.WorldToScreenPoint(worldPos);

        if (!cameraAware)
            return screenPos;

        if (rect.position.x < extents.x)
            sideOfParent = Vector2.right;
        if (Screen.width - rect.position.x < extents.x)
            sideOfParent = Vector2.left;

        if (rect.position.y < extents.y)
            sideOfParent = Vector2.up;
        if (Screen.height - rect.position.y < extents.y)
            sideOfParent = Vector2.down;

        return screenPos;
    }
}