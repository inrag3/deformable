using System.Collections.Generic;

namespace Deformation
{
    internal interface IChanger : IInitializable<IEnumerable<IDeformable>>
    {
        public void Change();
    }
}