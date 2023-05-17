using System;
using System.Collections.Generic;
using System.Linq;
using Deformation;
using Settings;
using Unity.Collections;
using UnityEngine;

public class Damager : MonoBehaviour, IInitializable<IList<MeshFilter>, IDamagerSettings>
{
    protected IList<MeshFilter> _filters;
    protected IDamagerSettings _settings;
    private Vector3 _rotation;

    public void Initialize(IList<MeshFilter> filters, IDamagerSettings settings)
    {
        _settings = settings;
        _filters = filters;
    }
    
    public virtual void Damage(ContactPoint contact, float impulse, MeshVertices[] temporaryVertices)
    {
        for (var i = 0; i < _filters.Count; i++)
        {
            var vertices = temporaryVertices[i].Vertices;
            var point = _filters[i].transform.InverseTransformPoint(contact.point);
            
            for (var j = 0; j < vertices.Length; j++)
            {
                var distance = Vector3.Distance(point, vertices[j]);
                if (!(distance <= _settings.Radius))
                    continue;
                var pulse = impulse;
                // Уменьшаем урон по мере увеличения расстояния от точки столкновения
                pulse -= pulse * Mathf.Clamp01(distance / _settings.Radius);
                
                var position = _filters[i].transform.TransformPoint(vertices[j]);
                
                position += (contact.normal * _settings.Multiplier * pulse) / 10f ;
                
                vertices[j] = _filters[i].transform.InverseTransformPoint(position);
            }
        }
    }
}