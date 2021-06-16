using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindEffect : MonoBehaviour
{
    float initialPositionX = 0;

    private void Awake()
    {
        initialPositionX = transform.position.x;
    }

    void Update()
    {
        Vector3 pos = transform.position;
        pos.x = initialPositionX;
        transform.position = pos;
    }
}
