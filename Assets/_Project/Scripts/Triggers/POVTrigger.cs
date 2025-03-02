using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Triggers
{
    public class POVTrigger : AbstractTrigger
    {
        [Header("Events")]
        [SerializeField] private UnityEvent onActivated;
        [SerializeField] private UnityEvent onDeactivated;

        [Header("Trigger Settings")]
        [SerializeField] private Camera camera;
        [SerializeField] private Vector3 triggerSize = new(0.1f, 3, 0.1f);
        [SerializeField] private Transform target;
        [SerializeField] private float activationTime = 0.1f;
        [SerializeField] private float angularTolerance = 0.01f;

    
        private bool _activated;

        private void Awake()
        {
            if (camera == null) 
                camera = Camera.main;
        }
        
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(transform.position, triggerSize);
            if (!target || !camera) return;
            Gizmos.color = Color.green;
            Gizmos.DrawRay(transform.position, (target.position - camera.transform.position).normalized);
        }

        private void FixedUpdate()
        {
            var delta = transform.position - camera.transform.position;
            delta = Vector3.Max(delta, -delta);
            if (Mathf.Abs(delta.x) > triggerSize.x 
                || Mathf.Abs(delta.y) > triggerSize.y
                || Mathf.Abs(delta.z) > triggerSize.z)
            {
                CancelInvoke(nameof(Activate));
                Deactivate();
                return;
            }
            var dir = (target.position - camera.transform.position).normalized;
            if (Vector3.Dot(camera.transform.forward.normalized, dir) < 1 - angularTolerance) {
                CancelInvoke(nameof(Activate));
                Deactivate();
                return;
            }
            Invoke(nameof(Activate), activationTime);
        }

        private void Activate()
        {
            if (_activated) return;
            onActivated.Invoke();
            _activated = true;
        }

        private void Deactivate()
        {
            if (!_activated) return;
            onDeactivated.Invoke();
            _activated = false;
        }
        public override void ResetState(Dictionary<string, object> states) { }
        public override void ApplyState(Dictionary<string, object> states) { }
        public override void OnCommit() { }
    }
}
