using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dummy : MonoBehaviour
{
    public float yLimit = -100;

    private Vector2 origin;
    private Quaternion originRotation;

    private void Start()
    {
        origin = transform.position;
        originRotation = transform.rotation;
    }

    private void Update()
    {
        if (transform.position.y < yLimit)
        {
            transform.position = origin;
            transform.rotation = originRotation;

            if (TryGetComponent(out Rigidbody2D rb))
            {
                rb.velocity = Vector2.zero;
                rb.angularVelocity = 0;
            }
        }
    }

}
