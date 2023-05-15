using UnityEngine;

namespace Settings
{
    [CreateAssetMenu(fileName = "Settings", menuName = "ScriptableObjects/Settings")]
    public class Settings : ScriptableObject, ISettings
    {
        [Header("Максимальный порог импульса столкновения:")]
        [SerializeField] [Range(0, 15)] private float _impulseMaximumThreshold = 10f;
        [Header("Минимальный порог импульса столкновения:")]
        [SerializeField] [Min(0.1f)] private float _impulseMinimumThreshold = 0.1f;
        [Header("Радиус деформации:")]
        [SerializeField] [Min(1f)] private float _radius = 1f;
        [Header("Множетель изменения вершины:")]
        [SerializeField] [Min(0f)] private float _multiplier = 1f;
        [Header("Скорость изменения полигональной сетки:")]
        [SerializeField] [Min(5f)] private float _deformationSpeed = 5f;
 
        public float ImpulseMaximumThreshold => _impulseMaximumThreshold;
        public float ImpulseMinimumThreshold => _impulseMinimumThreshold;
        public float DeformationSpeed => _deformationSpeed;
        public float Radius => _radius;
        public float Multiplier => _multiplier;
    }
}