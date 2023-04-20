using System.Collections.Generic;
using System.Linq;
using Settings;
using UnityEngine;

namespace Deformation
{
    public class Updater
    {
        private readonly IList<MeshFilter> _filters;
        private readonly IUpdaterSettings _settings;

        public Updater(IList<MeshFilter> filters, IUpdaterSettings settings)
        {
            _settings = settings;
            _filters = filters;
        }

        public void Update(MeshVertices[] temporaryVertices)
        {
            for (var i = 0; i < _filters.Count; i++)
            {
                var vertices = _filters[i].mesh.vertices;
                for (var j = 0; j < vertices.Length; j++)
                {
                    if (Vector3.Distance(vertices[j], temporaryVertices[i].Vertices[j]) > 0.1f)
                    {
                        vertices[j] += (temporaryVertices[i].Vertices[j] - vertices[j]) *
                                       (Time.deltaTime * _settings.DeformationSpeed);
                    }
                }
                _filters[i].mesh.SetVertices(vertices);
                _filters[i].mesh.MarkDynamic();
                _filters[i].mesh.RecalculateNormals();
                _filters[i].mesh.RecalculateBounds();
            }
        }
    }
}