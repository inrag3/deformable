using System;
using UnityEngine;

public interface IDetector
{
    public event Action<Collision> Detected;
}