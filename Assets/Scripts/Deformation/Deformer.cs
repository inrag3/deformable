using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Deformation
{
    [DisallowMultipleComponent]
    public class Deformer : MonoBehaviour
    {
        [Header("Impulse")]
        [SerializeField] [Min(0.5f)] private float _impulseThreshold;

        [SerializeField] [Min(1f)] private float _impulseMultiplier;

        private float _impulse;

        [Header("Radius")]
        [SerializeField] [Min(0f)] private float _radiusMultiplier = 0.2f;
        private float _radius;

        [Header("Softness")]
        [SerializeField] [Min(1f)] private float _softness;

        private IEnumerable<IDeformable> _deformables = Enumerable.Empty<IDeformable>();


        private void Awake()
        {
            _deformables = GetComponentsInChildren<IDeformable>();
            
        }

        private void OnEnable()
        {
            foreach (var deformable in _deformables)
            {
                deformable.Entered += OnEntered;
            }
        }

        private void OnDisable()
        {
            foreach (var deformable in _deformables)
            {
                deformable.Entered -= OnEntered;
            }
        }
        
        private void OnEntered(Collision collision, IDeformable deformable)
        {
            if (collision.relativeVelocity.magnitude < _impulseThreshold)
                return;
            var contact = collision.GetContact(0);
            _radius = collision.transform.localScale.magnitude / 4;
            _radius *= _radiusMultiplier;
            var vertices = deformable.Filter.mesh.vertices;
            for (var i = 0; i < vertices.Length; i++)
            {
                var vertex = vertices[i];
                var point = transform.TransformPoint(vertex);
                var distance = Vector3.Distance(contact.point, point);
                if (!(distance < _radius)) continue;
                var direction = collision.relativeVelocity;
                distance = _radius - distance;
                if (_softness > 1.0f)
                    distance = Mathf.Pow(distance, _softness);
                point += direction.normalized * (_impulse * distance);
                vertices[i] = transform.InverseTransformPoint(point);
            }
            deformable.Filter.mesh.MarkDynamic();
            deformable.Filter.mesh.SetVertices(vertices);
            deformable.Filter.mesh.RecalculateNormals();
            deformable.Filter.mesh.RecalculateBounds();
        }
    }
}