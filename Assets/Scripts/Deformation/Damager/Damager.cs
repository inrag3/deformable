using System;
using System.Collections.Generic;
using Deformation;
using UnityEngine;

public class Damager : MonoBehaviour, IInitializable<IList<IDeformable>>
{
    [SerializeField] [Min(1f)] private float _radius = 1f;
    [SerializeField] [Min(0f)] private float _multiplier = 1f;
    private IList<IDeformable> _deformables;
    private Transform _direction;

    private void Awake()
    {
        _direction = new GameObject("Direction").transform;
        _direction.SetParent(transform, false);
    }
    
    public void Initialize(IList<IDeformable> deformables)
    {
        _deformables = deformables;
    }
    
    public void Change(ContactPoint contact, float impulse)
    {
        Vector3[] vertices = Array.Empty<Vector3>();
        foreach (var deformable in _deformables)
        {
            vertices = deformable.Filter.mesh.vertices;
            var filter = deformable.Filter;
            var distance = Vector3.Distance(Closest(contact, vertices), contact.point);
            _direction.rotation = Quaternion.FromToRotation(Vector3.forward, contact.normal);
            if (!(distance <= _radius))
                continue;
            var point = filter.transform.InverseTransformPoint(contact.point);
            for (var i = 0; i < vertices.Length; i++)
            {
                distance = (point - vertices[i]).magnitude;
                if (distance <= _radius)
                {
                    // Уменьшаем урон мере увеличения расстояния от точки столкновения
                    impulse -= impulse * Mathf.Clamp01(distance / _radius);
                    
                    
                    _direction.position = filter.transform.TransformPoint(vertices[i]) +
                                          _direction.forward * impulse * (_multiplier / 10f);
                    
                    
                    vertices[i] = filter.transform.InverseTransformPoint(_direction.position);
                    //TODO посчитать общий дамаг.
                }
            }
            filter.mesh.SetVertices(vertices);
            filter.mesh.MarkDynamic();
            filter.mesh.RecalculateBounds();
            filter.mesh.RecalculateNormals();
        }
        
    }

    private Vector3 Closest(ContactPoint contact, IReadOnlyList<Vector3> vertices)
    {
        var closestPoint = vertices[0];
        var closestDistance = Vector3.Distance(vertices[0], contact.point);

        for (var i = 1; i < vertices.Count; i++)
        {
            var distance = Vector3.Distance(vertices[i], contact.point);
            if (!(distance < closestDistance))
                continue;
            closestDistance = distance;
            closestPoint = vertices[i];
        }

        return closestPoint;
    }
}