using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    [SerializeField] Transform target;
    [SerializeField] Vector3 offset;
    Vector3 desiredPos;
    Vector3 smoothedPos;

    [SerializeField] float smoothSpeed;

    private void Start()
    {
        target = FindObjectOfType<PlayerController>().transform;
    }

    private void FixedUpdate()
    {
        desiredPos = target.position + offset;
        smoothedPos = Vector3.Lerp(transform.position, desiredPos, smoothSpeed * GamePause.deltaTime);
    }

    private void LateUpdate()
    {
        

        transform.position = smoothedPos;
    }
}
