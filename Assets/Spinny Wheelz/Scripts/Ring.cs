using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ring : MonoBehaviour
{
    [SerializeField] private ParticleSystem effect = null;
    public void PlayEffect()
    {
        effect.Play();
    }
}
