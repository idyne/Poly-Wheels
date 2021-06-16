using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NameTag : MonoBehaviour
{
    private Vector3 offset = Vector3.zero;
    private Vector3 initialRotation = Vector3.zero;
    [SerializeField] private Competitor competitor = null;
    [SerializeField] private Text text = null;

    private void Awake()
    {
        offset = transform.position - transform.parent.position;
        text.text = competitor.Name;
    }
    private void Update()
    {
        transform.position = transform.parent.position + offset;
        transform.rotation = Quaternion.Euler(initialRotation);
    }
}
