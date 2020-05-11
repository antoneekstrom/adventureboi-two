using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowingCamera : MonoBehaviour
{

    public Transform target;
    public Vector3 offset = new Vector3(0, 3, -100);
    public float lerpSpeed = 3f;
    public bool targetWithMouse = true;

    private void Update()
    {
        if (!target) return;

        if (targetWithMouse && Input.GetMouseButtonDown(1))
        {
            RaycastHit2D hit = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(Input.mousePosition));
            if (hit.transform)
                target = hit.transform;
        }
    }

    private void LateUpdate()
    {
        if (!target) return;

        Vector3 desiredPosition = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, lerpSpeed * Time.deltaTime);
    }

}
