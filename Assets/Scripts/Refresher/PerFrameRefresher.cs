using UnityEngine;

namespace Refresher
{
    [DisallowMultipleComponent]
    public class PerFrameRefresher : Refresher
    {
        protected override void LateUpdate() => 
            Refresh();
    }
}