using System;
using System.Collections;
using UnityEngine;

public static class GameFeelHelper
{

    public static void SlowTime(this MonoBehaviour obj, float duration, float modifier) => obj.StartCoroutine(SlowTimeCoroutine(duration, modifier));

    public static IEnumerator SlowTimeCoroutine(float duration, float modifier)
    {
        Time.timeScale = modifier;
        yield return new WaitForSecondsRealtime(duration);
        Time.timeScale = 1;
    }


    public static void Shake(this MonoBehaviour _, float duration, float strength) => Shake(duration, strength);

    public static IEnumerator Shake(float duration, float strength)
    {
        yield return new WaitForSecondsRealtime(duration);
    }
}