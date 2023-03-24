using UnityEngine;

namespace Refresher
{
    [DisallowMultipleComponent]
    public class DelayRefresher : Refresher
    {
        [SerializeField] private float _delay = 1f;
        private float _timer;
        
        protected override void LateUpdate()
        {
            _timer += Time.deltaTime;

            if (_timer >= _delay)
            {
                Refresh();
            }
        }
    }
}