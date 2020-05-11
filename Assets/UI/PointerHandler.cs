using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;

public class PointerHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    public Sprite onHoverSprite;
    public Sprite onDownSprite;

    public UnityEvent OnEnter;
    public UnityEvent OnExit;

    private bool pointerHover, pointerDown;

    private Sprite defaultSprite;
    private Image img;

    private void Awake()
    {
        img = GetComponent<Image>();
        defaultSprite = img.sprite;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        OnEnter.Invoke();

        if (img && onHoverSprite)
        {
            pointerHover = true;
            img.sprite = onHoverSprite;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        OnExit.Invoke();

        if (img && onHoverSprite)
        {
            pointerHover = false;
            img.sprite = defaultSprite;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (img && onDownSprite)
        {
            pointerDown = true;
            img.sprite = onDownSprite;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (img && onDownSprite)
        {
            pointerDown = false;
            img.sprite = defaultSprite;
        }
    }
}
