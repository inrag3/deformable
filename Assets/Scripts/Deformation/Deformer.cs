using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Deformation
{
    public class Deformer : MonoBehaviour
    {
        [SerializeField] [Min(2)] private float _minimumImpulse;
        [SerializeField] [Min(0.05f)] private float _malleability;
        [SerializeField] [Min(0)] private float _radius = 0.1f;
        private IEnumerable<IDeformable> _deformables = Enumerable.Empty<IDeformable>();
        private Vector3[] _vertices;

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
            var contact = collision.GetContact(0);
            var point = transform.InverseTransformPoint(contact.point);
            var normal = transform.InverseTransformDirection(contact.normal);
            var impulse = collision.impulse.magnitude;
            if (impulse < _minimumImpulse)
                return;
            
            _vertices = deformable.Filter.mesh.vertices;
            for (var i = 0; i < _vertices.Length; i++)
            {
                var scale = Mathf.Clamp(_radius - (point - _vertices[i]).magnitude, 0, _radius);
                _vertices[i] += normal * impulse * scale * _malleability;
            }

            var mesh = deformable.Filter.mesh;
            mesh.vertices = _vertices;
            deformable.Collider.sharedMesh = mesh;
            deformable.Filter.mesh.RecalculateNormals();
            deformable.Filter.mesh.RecalculateBounds();
        }
    }
}