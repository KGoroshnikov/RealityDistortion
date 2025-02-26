using System;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

namespace _Project.Scripts
{
    public class MurderMoon : MonoBehaviour
    {
        [SerializeField] private Transform moon;
        [SerializeField] private Transform player;
        [SerializeField] private LayerMask obstacleLayer;
        
        [SerializeField] private UnityEvent<float> onTimerStarted;
        [SerializeField] private UnityEvent onDetectStarted;
        [SerializeField] private UnityEvent onPlayerDetected;
    
        [SerializeField, Min(0)] private float minTimerDuration = 1;
        [SerializeField, Min(0)] private float maxTimerDuration = 2;

        private void Start() => RestartTimer();

        private void TryDetect()
        {
            onDetectStarted.Invoke();
            RestartTimer();
            if (Physics.Linecast(moon.position, player.position, obstacleLayer)) return;
            onPlayerDetected.Invoke();
        }

        private void RestartTimer()
        {
            var time = Random.Range(minTimerDuration, maxTimerDuration);
            Invoke(nameof(TryDetect), time);
            onTimerStarted.Invoke(time);
        }
    }
}