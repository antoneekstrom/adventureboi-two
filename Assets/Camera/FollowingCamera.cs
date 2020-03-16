using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowingCamera : MonoBehaviour
{

    public Transform target;
    public Vector3 offset = new Vector3(0, 3, -100);
    public float lerpSpeed = 3f;

    private void LateUpdate()
    {
        Vector3 desiredPosition = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, lerpSpeed * Time.deltaTime);
    }

}
