using UnityEngine;

namespace Settings
{
    [CreateAssetMenu(fileName = "Settings", menuName = "ScriptableObjects/Settings")]
    public class Settings : ScriptableObject, ISettings
    {
        [SerializeField] [Range(0, 1)] private float _impulseMaximumThreshold = 1f;
        [SerializeField] [Min(0.1f)] private float _impulseMinimumThreshold = 0.1f;
        [SerializeField] [Min(1f)] private float _radius = 1f;
        [SerializeField] [Min(0f)] private float _multiplier = 1f;

        public float ImpulseMaximumThreshold => _impulseMaximumThreshold;
        public float ImpulseMinimumThreshold => _impulseMinimumThreshold;
        public float Radius => _radius;
        public float Multiplier => _multiplier;
    }
}