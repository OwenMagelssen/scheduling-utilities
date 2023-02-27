using System;
using UnityEngine;

namespace SchedulingUtilities
{
    public class EventOnDisable : MonoBehaviour
    {
        public event Action onDisable;

        private void OnDisable()
        {
            onDisable?.Invoke();
        }
    }
}
