using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompetitorTrail : MonoBehaviour
{

    private Quaternion initialRotation;
    private Vector3 initialLocalPosition;

    private void Awake()
    {
        initialRotation = transform.rotation;
        initialLocalPosition = transform.localPosition;
    }
    private void LateUpdate()
    {
        transform.rotation = initialRotation;
        transform.localPosition = initialLocalPosition;
    }
}
