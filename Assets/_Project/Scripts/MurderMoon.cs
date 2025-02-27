using System;
using Triggers;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

namespace _Project.Scripts
{
    public class MurderMoon : AbstractTrigger
    {
        [SerializeField] private bool inDetectMode;
        [SerializeField] private Transform moon;
        [SerializeField] private Transform player;
        [SerializeField] private LayerMask obstacleLayer;
        
        [SerializeField] private UnityEvent<float> onTimerStarted;
        [SerializeField] private UnityEvent onDetectStarted;
        [SerializeField] private UnityEvent onPlayerDetected;
    
        [SerializeField, Min(0)] private float minTimerDuration = 1;
        [SerializeField, Min(0)] private float maxTimerDuration = 2;

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(moon.position, player.position);
        }

        private void Start() => StartTimer();

        private void TryDetect()
        {
            StartTimer();
            if (!inDetectMode) return;
            onDetectStarted.Invoke();
            if (Physics.Linecast(moon.position, player.position, obstacleLayer)) return;
            onPlayerDetected.Invoke();
        }

        private void StartTimer()
        {
            var time = Random.Range(minTimerDuration, maxTimerDuration);
            Invoke(nameof(TryDetect), time);
            onTimerStarted.Invoke(time);
        }
        
        public void ChangeDetectMode(bool mode) => inDetectMode = mode;
    }
}