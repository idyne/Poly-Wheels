using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Road : MonoBehaviour
{
    [SerializeField] private GameObject meshPrefab = null;
    [SerializeField] private bool generateOnStart = false;
    [SerializeField] private int defaultSize = 1;

    void Start()
    {
        if (generateOnStart)
        {
            Generate(defaultSize);
            transform.Rotate(Vector3.up * 180);
        }

    }

    public void Generate(int size)
    {
        for (int i = 0; i < size; i++)
        {
            Transform mesh = Instantiate(meshPrefab, transform.position + Vector3.forward * 20f * i, meshPrefab.transform.rotation, transform).transform;
        }
        BoxCollider collider = GetComponent<BoxCollider>();
        collider.size = new Vector3(16, 0.5f, size * 20 + 1000);
        collider.center = new Vector3(0, -0.25f, size * 10 + 500);
    }
}
