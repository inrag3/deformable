using System.Collections.Generic;
using UnityEngine;

namespace Deformation
{
    public struct MeshVertices
    {
        public Vector3[] Vertices;

        public MeshVertices(Vector3[] meshVertices)
        {
            Vertices = meshVertices;
        }
    }
}