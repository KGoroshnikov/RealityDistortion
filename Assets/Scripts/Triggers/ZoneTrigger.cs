using UnityEngine;
using UnityEngine.Events;

namespace Triggers
{
    public class ZoneTrigger : AbstractTrigger
    {
        [SerializeField] private UnityEvent onZoneEnter;
        [SerializeField] private UnityEvent onZoneExit;

        private void OnTriggerEnter(Collider other) => onZoneEnter?.Invoke();
        private void OnTriggerExit(Collider other) => onZoneExit?.Invoke();
    }
}