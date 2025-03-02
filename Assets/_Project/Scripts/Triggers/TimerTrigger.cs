using System.Collections.Generic;
using Triggers;
using UnityEngine;
using UnityEngine.Events;

public class TimerTrigger : AbstractTrigger
{
    [SerializeField] private UnityEvent onTimerStart;
    [SerializeField] private UnityEvent onTimerEnd;
    
    [SerializeField, Min(0)] private float timerDuration = 1;

    public void StartTimer()
    {
        onTimerStart.Invoke();
        Invoke(nameof(Finish), timerDuration);
    }
    private void Finish() => onTimerEnd.Invoke();
    
    public void CancelTimer() => CancelInvoke(nameof(Finish));

    public override void ResetState(Dictionary<string, object> states) => CancelTimer();
    public override void ApplyState(Dictionary<string, object> states) { }
    public override void OnCommit() { }
}