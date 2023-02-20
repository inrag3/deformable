using System;
using UnityEngine;

namespace Detector
{
    public interface IDetector<out T>
    {
        public event Action<T> Detected;
    }
}