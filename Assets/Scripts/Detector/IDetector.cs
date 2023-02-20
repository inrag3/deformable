using System;
using UnityEngine;

namespace Detector
{
    public interface IDetector
    {
        public event Action<Collision> Detected;
    }
}