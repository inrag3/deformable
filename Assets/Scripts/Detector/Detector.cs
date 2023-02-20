using System;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(MeshCollider))]
public class Detector : MonoBehaviour, IDetector
{
    public event Action<Collision> Detected;

    private void OnCollisionEnter(Collision collision)
    {
        Detected?.Invoke(collision);
    }
}