using System.Collections.Generic;
using Triggers;
using UnityEngine;
using UnityEngine.Events;

public class CountTrigger : AbstractTrigger
{
    [SerializeField] private UnityEvent<int> onValueChanged;
    [SerializeField] private UnityEvent onValueLimitReached;
    
    [SerializeField, Min(0)] private int limit = 1;

    [SerializeField] private int value;
    
    public void Increment()
    {
        value++;
        onValueChanged.Invoke(value);
        if (value == limit) onValueLimitReached.Invoke();
        SetState($"CountTrigger_{Guid}_Count", value);
    }
    public void Decrement()
    {
        value--;
        onValueChanged.Invoke(value);
        if (value == limit) onValueLimitReached.Invoke();
        SetState($"CountTrigger_{Guid}_Count", value);
    }
    private void Start() => Initialize();

    public override void ResetState(Dictionary<string, object> states) { }
    public override void ApplyState(Dictionary<string, object> states)
    {
        value = (int)states.GetValueOrDefault($"CountTrigger_{Guid}_Count", 0);
        onValueChanged.Invoke(value);
        if (value == limit) onValueLimitReached.Invoke();
    }
    public override void OnCommit() { }
}