using System;
using System.Collections.Generic;
using System.Linq;
using Deformation;
using JetBrains.Annotations;
using UnityEngine;

public class Damager : MonoBehaviour
{
    [SerializeField] [Min(1f)] private float _radius = 1f;
    private IEnumerable<IDeformable> _deformables;

    public void Init(IEnumerable<IDeformable> deformables)
    {
        _deformables = deformables;
    }

    public void Change(ContactPoint contact, float impulse)
    {
        foreach (var deformable in _deformables)
        {
            var vertices = deformable.Filter.mesh.vertices;
            var distance = Vector3.Distance(Closest(contact, vertices), contact.point);

            if (!(distance <= _radius))
                continue;
            var point = transform.InverseTransformPoint(contact.point);
            for (var i = 0; i < vertices.Length; i++)
            {
                distance = (point - vertices[i]).magnitude;
                if (distance <= _radius)
                {
                    // Уменьшаем урон мере увеличения расстояния от точки столкновения
                    impulse *= Mathf.Clamp01(distance / _radius);
                }
            }
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