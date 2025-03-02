using System;
using System.Collections.Generic;
using System.Collections;
using Triggers;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

namespace _Project.Scripts
{
    public class MurderMoon : MonoBehaviour, IFreezable
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

        [SerializeField] private AudioSource warningAudio;
        [SerializeField] private AudioSource attackAudio;

        private bool freezed;

        private Coroutine warnCoroutine;
        private Coroutine rotateCoroutine;
        private Coroutine detectCoroutine;
        private Coroutine lookBackCoroutine;

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(moon.position, player.position);
        }

        private void OnEnable() => StartTimer();

        private IEnumerator WaitAndExecute(float waitTime, Action action)
        {
            float remaining = waitTime;
            while (remaining > 0f)
            {
                if (!freezed)
                {
                    remaining -= Time.deltaTime;
                }
                yield return null;
            }
            action();
        }

        private void TryDetect()
        {
            attackAudio.Play();
            seeing = true;
            //Invoke("LookBack", Random.Range(timeSee.x, timeSee.y));
            lookBackCoroutine = StartCoroutine(WaitAndExecute(Random.Range(timeSee.x, timeSee.y), LookBack));
            onDetectStarted.Invoke();
        }

        void WarnPlayer(){
            warningAudio.Play();
        }

        void RotateToLook(){
            animator.SetTrigger("See");
        }

        void FixedUpdate()
        {
            if (!seeing || !inDetectMode || freezed) return;
            if (Physics.Linecast(moon.position, player.position, obstacleLayer)) return;
            playerDie.Die();
            onPlayerDetected.Invoke();
        }

        void LookBack(){
            attackAudio.Stop();
            seeing = false;
            StartTimer();
            animator.SetTrigger("DontSee");
        }

        private void StartTimer()
        {
            var time = Random.Range(timeNotSee.x, timeNotSee.y);
            /*Invoke("WarnPlayer", time - warningTime);
            Invoke("RotateToLook", time - rotateTime);
            Invoke(nameof(TryDetect), time);*/
            warnCoroutine = StartCoroutine(WaitAndExecute(time - warningTime, WarnPlayer));
            rotateCoroutine = StartCoroutine(WaitAndExecute(time - rotateTime, RotateToLook));
            detectCoroutine = StartCoroutine(WaitAndExecute(time, TryDetect));

            onTimerStarted.Invoke(time);
        }
        
        public void ChangeDetectMode(bool mode) => inDetectMode = mode;

        public void Freeze()
        {
            freezed = true;
        }

        public void UnFreeze()
        {
            freezed = false;
        }
    }
}