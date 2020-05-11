using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnClickEvent : MonoBehaviour
{
    public StringVariable eventName;
    public int mouseButton = 1;

    private Collider2D col;
    private EventDispatcher ed;

    private void Awake()
    {
        ed = GetComponent<EventDispatcher>();
        col = GetComponent<Collider2D>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(mouseButton) && col.OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition)))
            ed.Dispatch(ed.CreateEventAction(eventName));
    }
}
