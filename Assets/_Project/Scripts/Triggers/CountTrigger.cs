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
    }
    public void Decrement()
    {
        value--;
        onValueChanged.Invoke(value);
        if (value == limit) onValueLimitReached.Invoke();
    }
}