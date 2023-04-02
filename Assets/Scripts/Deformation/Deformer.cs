using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Deformation
{
    [RequireComponent(typeof(Damager))]
    [DisallowMultipleComponent]
    public class Deformer : MonoBehaviour
    {
        [SerializeField] private Settings.Settings _settings;
        private IList<IDeformable> _deformables = Array.Empty<IDeformable>();
        private Damager _damager;

        private Updater _updater;
        // TODO сделать удобную API для взаимодейтсвия с Repairer'ом.
        // private Repairer _repairer;
        //TODO сделать Applier для применения рассчетов Damager, возможно туда его и поместить

        private void Awake()
        {
            _damager = GetComponent<Damager>();
            _deformables = GetComponentsInChildren<IDeformable>();
            _damager.Initialize(_deformables, _settings);
            //_updater.Initialize(_deformables);
            //TODO сделать обработку, если нет ниодного deforamble
        }

        private void OnEnable()
        {
            foreach (var deformable in _deformables)
            {
                deformable.Entered += OnEntered;
            }
        }

        private void OnEntered(Collision collision)
        {
            var impulse = collision.impulse.magnitude;
            impulse = Mathf.Clamp(impulse, 0f, 10f);
            if (impulse < _settings.ImpulseMinimumThreshold)
                return;
            var contact = collision.GetContact(0);
            _damager.Change(contact, impulse);
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