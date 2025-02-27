using Triggers;
using UnityEngine;
using UnityEngine.Events;

public class TouchTrigger : AbstractTrigger
{
    [SerializeField] private UnityEvent onEnter;
    [SerializeField] private UnityEvent onExit;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) onEnter.Invoke();
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) onExit.Invoke();
    }
}
