using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bonus : MonoBehaviour
{
    [SerializeField] private GameObject meshPrefab = null;
    void Start()
    {
        Generate();
    }

    private void Generate()
    {
        for (int i = 0; i < 34; i++)
        {
            Transform mesh = Instantiate(meshPrefab, transform.position + Vector3.forward * 37.8f * (i + 1), meshPrefab.transform.rotation, transform).transform;
            mesh.GetChild(0).GetComponentInChildren<Text>().text = "x" + Math.Round(Mathf.Clamp((1.0f + i * 3 * 0.3f), 1.1f, 20f), 1);
            mesh.GetChild(1).GetComponentInChildren<Text>().text = "x" + Math.Round(Mathf.Clamp((1.0f + i * 3 * 0.3f + 0.3f), 1.1f, 20f), 1);
            mesh.GetChild(2).GetComponentInChildren<Text>().text = "x" + Math.Round(Mathf.Clamp((1.0f + i * 3 * 0.3f + 0.6f), 1.1f, 20f), 1);
        }
        BoxCollider collider = GetComponent<BoxCollider>();
        collider.enabled = false;
        //collider.size = new Vector3(16, 0.5f, 1285.2f);
        //collider.center = new Vector3(0, -0.25f, 642.6f);
    }
}
