using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBehaviour : MonoBehaviour
{
    private readonly float zMaxIncrease = 10f;
    public static float baseZ = 50;

    public bool useCamera = true;
    public float depth = 1f;
    public Transform parent;

    private Vector3 origin;

    private void Start()
    {
        if (useCamera)
            parent = Camera.main.transform;

        origin = transform.position;
        origin.z = baseZ + depth * zMaxIncrease;
    }

    private void Update()
    {
        Vector2 delta = parent.transform.position - origin;
        transform.position = origin + (Vector3)delta * depth;
    }

    private void OnValidate()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, baseZ + depth * zMaxIncrease);
        origin = new Vector3(origin.x, origin.y, baseZ + depth * zMaxIncrease);
    }

}
