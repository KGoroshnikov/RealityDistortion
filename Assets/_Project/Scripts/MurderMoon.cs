using System;
using System.Collections.Generic;
using Triggers;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

namespace _Project.Scripts
{
    public class MurderMoon : MonoBehaviour
    {
        [SerializeField] private bool inDetectMode;
        [SerializeField] private Transform moon;
        [SerializeField] private Transform player;
        [SerializeField] private LayerMask obstacleLayer;
        
        [SerializeField] private UnityEvent<float> onTimerStarted;
        [SerializeField] private UnityEvent onDetectStarted;
        [SerializeField] private UnityEvent onPlayerDetected;

        [SerializeField] private Animator animator;
    
        [SerializeField] private Vector2 timeNotSee;
        [SerializeField] private Vector2 timeSee;

        [SerializeField] private float warningTime = 2f;
        private float rotateTime = 0.5f;

        private bool seeing;

        [SerializeField] private DeathManager playerDie;

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(moon.position, player.position);
        }

        private void Start() => StartTimer();

        private void TryDetect()
        {
            seeing = true;
            Invoke("LookBack", Random.Range(timeSee.x, timeSee.y));
            onDetectStarted.Invoke();
        }

        void WarnPlayer(){
            Debug.Log("Warning");
        }

        void RotateToLook(){
            animator.SetTrigger("See");
        }

        void FixedUpdate()
        {
            if (!seeing || !inDetectMode) return;
            if (Physics.Linecast(moon.position, player.position, obstacleLayer)) return;
            playerDie.Die();
            onPlayerDetected.Invoke();
        }

        void LookBack(){
            seeing = false;
            StartTimer();
            animator.SetTrigger("DontSee");
        }

        private void StartTimer()
        {
            var time = Random.Range(timeNotSee.x, timeNotSee.y);
            Invoke("WarnPlayer", time - warningTime);
            Invoke("RotateToLook", time - rotateTime);
            Invoke(nameof(TryDetect), time);
            onTimerStarted.Invoke(time);
        }
        
        public void ChangeDetectMode(bool mode) => inDetectMode = mode;
    }
}