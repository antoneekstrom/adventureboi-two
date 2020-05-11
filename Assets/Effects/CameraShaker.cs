using System.Collections;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class CameraShaker : MonoBehaviour
{

    public bool IsShaking { get; private set; }
    public static CameraShaker Instance { get; private set; }

    private float strength = 1f;
    private WaitForSecondsRealtime wait;

    public static void Shake(float strength, float duration) => Instance.ShakeCamera(strength, duration);

    protected void ShakeCamera(float strength, float duration)
    {
        wait = new WaitForSecondsRealtime(duration);
        this.strength = strength;
        StartCoroutine(ShakeRoutine());
    }

    private IEnumerator ShakeRoutine()
    {
        IsShaking = true;
        yield return wait;
        IsShaking = false;
    }

    private void Update()
    {
        if (IsShaking)
        {
            transform.position += (Vector3)((Vector2)Random.insideUnitSphere * strength * Time.deltaTime);
        }
    }

    private void Awake()
    {
        if (!Instance)
            Instance = this;
    }

}
