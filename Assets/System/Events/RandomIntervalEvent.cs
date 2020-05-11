using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomIntervalEvent : MonoBehaviour
{
    public StringVariable eventName;
    public float frequency = 0.5f;
    public float chance = 0.5f;

    public bool RoutineShouldRun { get; set; } = true;

    private EventDispatcher ed;
    private WaitForSecondsRealtime wait;

    private void Awake()
    {
        ed = GetComponent<EventDispatcher>();
        wait = new WaitForSecondsRealtime(frequency);
    }

    private void Start()
    {
        StartCoroutine(IntervalRoutine());
    }

    private void TryDispatch()
    {
        if (Random.value < chance)
            ed.Dispatch(ed.CreateEventAction(eventName));
    }

    protected IEnumerator IntervalRoutine()
    {
        yield return wait;
        TryDispatch();

        if (RoutineShouldRun)
            StartCoroutine(IntervalRoutine());
    }
}
