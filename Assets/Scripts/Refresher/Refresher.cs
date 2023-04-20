using System;
using UnityEngine;

namespace Refresher
{
    [RequireComponent(typeof(MeshCollider), typeof(MeshFilter))]
    public abstract class Refresher : MonoBehaviour
    {
        [SerializeField] private MeshColliderCookingOptions _cookingOptions = ~MeshColliderCookingOptions.None;
        private MeshCollider _collider;
        private MeshFilter _filter;

        private void Awake()
        {
            _filter = GetComponent<MeshFilter>();
            _collider = GetComponent<MeshCollider>();
        }

        protected abstract void LateUpdate();

        protected void Refresh()
        {
            _collider.cookingOptions = _cookingOptions;
            _collider.sharedMesh = null;
            Mesh sharedMesh;
            (sharedMesh = _filter.sharedMesh).MarkDynamic();
            _collider.sharedMesh = sharedMesh;
        }
    }
}