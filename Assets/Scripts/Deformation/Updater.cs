using System.Collections.Generic;
using System.Linq;

namespace Deformation
{
    public class Updater : IInitializable<IList<IDeformable>>
    {
        
        private IList<IDeformable> _deformables;
        private IList<MeshVertices> _vertices;
    
        public void Initialize(IList<IDeformable> deformables)
        {
            _deformables = deformables;
            _vertices = _deformables.Select(x => x.InitialVertices).ToArray();
        }

        public void Update()
        {
            foreach (var deformable in _deformables)
            {
                var vertices = deformable.Filter.mesh.vertices;
                
            }
        }
    }
}