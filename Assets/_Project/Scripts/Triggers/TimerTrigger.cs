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
}