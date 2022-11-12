using System;
using UnityEngine;

namespace Deformation
{
    public interface IDeformable
    {
        public event Action<Collision, IDeformable> Entered;
        public MeshVertices InitialVertices { get; }
        public MeshFilter Filter { get; }
        public MeshCollider Collider { get; }
    }
}