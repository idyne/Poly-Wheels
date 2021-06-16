using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FateGames;

public class UI1st : MonoBehaviour
{
    
    void Start()
    {
        transform.rotation = Quaternion.Euler(0, 0, -10);
        transform.LeanRotateZ(10, 0.5f).setLoopPingPong();
        transform.LeanScale(Vector3.one * 1.1f, 0.25f).setLoopPingPong();
    }

}
