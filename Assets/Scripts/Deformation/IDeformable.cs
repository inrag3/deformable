using System;
using UnityEngine;

namespace Deformation
{
    public interface IDeformable
    {
        public event Action<Collision> Entered;
        public MeshVertices InitialVertices { get; }
        public MeshFilter Filter { get; }
    }
}