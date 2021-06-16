using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleTire : MonoBehaviour
{
    [SerializeField] private Transform mesh = null;

    private void Update()
    {
        //RotateMesh();
    }

    /*private void RotateMesh()
    {
        mesh.Rotate(Vector3.forward * Time.deltaTime * 120);
    }*/

    public void GetCollected()
    {
        Destroy(GetComponent<Collider>());
        mesh.LeanScale(Vector3.zero, 0.1f).setOnComplete(() => { Destroy(gameObject); });
    }
}
