using System;
using System.Collections.Generic;
using Deformation;
using Settings;
using UnityEngine;

public class Damager : MonoBehaviour, IInitializable<IList<IDeformable>, IDamagerSettings>
{
    private IList<IDeformable> _deformables;
    private Transform _direction;
    private IList<IDeformable> _deformables1;
    private IDamagerSettings _settings;

    private void Awake()
    {
        _direction = new GameObject("Direction").transform;
        _direction.SetParent(transform, false);
    }

    public void Initialize(IList<IDeformable> deformables, IDamagerSettings settings)
    {
        _settings = settings;
        _deformables1 = deformables;
    }
    
    public void Change(ContactPoint contact, float impulse)
    {
        var vertices = Array.Empty<Vector3>();
        foreach (var deformable in _deformables)
        {
            vertices = deformable.Filter.mesh.vertices;
            var filter = deformable.Filter;
            var distance = Vector3.Distance(FindClosestVertex(contact, vertices), contact.point);
            _direction.rotation = Quaternion.FromToRotation(Vector3.forward, contact.normal);
            if (!(distance <= _settings.Radius))
                continue;
            var point = filter.transform.InverseTransformPoint(contact.point);
            for (var i = 0; i < vertices.Length; i++)
            {
                distance = (point - vertices[i]).magnitude;
                if (distance <= _settings.Radius)
                {
                    // Уменьшаем урон мере увеличения расстояния от точки столкновения
                    impulse -= impulse * Mathf.Clamp01(distance / _settings.Radius);
                    
                    _direction.position = filter.transform.TransformPoint(vertices[i]) +
                                          _direction.forward * impulse * (_settings.Multiplier / 10f);
                    
                    vertices[i] = filter.transform.InverseTransformPoint(_direction.position);
                }
            }

            filter.mesh.SetVertices(vertices);
            filter.mesh.MarkDynamic();
            filter.mesh.RecalculateBounds();
            filter.mesh.RecalculateNormals();
        }
    }

    private static Vector3 FindClosestVertex(ContactPoint contact, IEnumerable<Vector3> vertices)
    {
        var closestPoint = Vector3.zero;
        var closestDistance = float.MaxValue;

        foreach (var vertex in vertices)
        {
            var distance = Vector3.Distance(vertex, contact.point);
            if (!(distance < closestDistance)) 
                continue;
            closestDistance = distance;
            closestPoint = vertex;
        }

        return closestPoint;
    }
}