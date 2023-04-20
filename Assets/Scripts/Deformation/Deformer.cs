using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using UnityEngine;

namespace Deformation
{
    [RequireComponent(typeof(Damager))]
    [DisallowMultipleComponent]
    public class Deformer : MonoBehaviour
    {
        [SerializeField] private Settings.Settings _settings;
        private IList<MeshFilter> _filters = Array.Empty<MeshFilter>();
        private IList<IDeformable> _deformables = Array.Empty<IDeformable>();
        private ContactPoint _contactPoint;
        private MeshVertices[] _temporaryVertices;
        private Damager _damager;
        private Updater _updater;
        
        public void Awake()
        {
            _deformables = GetComponentsInChildren<IDeformable>();
            _damager = GetComponent<Damager>();
            
            _filters = _deformables.Select(x => x.Filter).ToArray();
            _damager.Initialize(_filters, _settings);
            _updater = new Updater(_filters, _settings);
            InitializeTemporaryVertices();
        }

        private void InitializeTemporaryVertices()
        {
            _temporaryVertices = new MeshVertices[_filters.Count];
            for (var i = 0; i < _filters.Count; i++)
                _temporaryVertices[i].Vertices = _filters[i].mesh.vertices;
        }

        private void Update() => 
            _updater.Update(_temporaryVertices);

        private void OnEnable()
        {
            foreach (var deformable in _deformables)
            {
                deformable.Entered += OnEntered;
            }
        }

        private void OnEntered(Collision collision)
        {
          
            var impulse = collision.impulse.magnitude / 100f;
            impulse = Mathf.Clamp(impulse, 0f, _settings.ImpulseMaximumThreshold);
            if (impulse < _settings.ImpulseMinimumThreshold)
                return;
            
            _contactPoint = collision.GetContact(0);
            _damager.Damage(_contactPoint, impulse, _temporaryVertices);
        }

        private void OnDisable()
        {
            foreach (var deformable in _deformables)
            {
                deformable.Entered -= OnEntered;
            }
        }
    }
}