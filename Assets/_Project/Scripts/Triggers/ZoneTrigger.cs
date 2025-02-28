using Triggers;
using UnityEngine;
using UnityEngine.Events;

public class ZoneTrigger : AbstractTrigger
{
    [SerializeField] private bool useTag = true;
    [SerializeField] private string tag;
    [SerializeField] private LayerMask layerMask;
    
    [SerializeField] private UnityEvent<Collider> onEnter;
    [SerializeField] private UnityEvent<Collider> onStay;
    [SerializeField] private UnityEvent<Collider> onExit;

    private void OnTriggerEnter(Collider other)
    {
        var layer = 1 << (other.gameObject.layer - 1);
        if ((!useTag ||other.CompareTag(tag)) 
            && (layerMask.value & layer) == layer) 
            onEnter.Invoke(other);
    }
    private void OnTriggerStay(Collider other)
    {
        var layer = 1 << (other.gameObject.layer - 1);
        if ((!useTag ||other.CompareTag(tag)) 
            && (layerMask.value & layer) == layer) 
            onStay.Invoke(other);
    }
    private void OnTriggerExit(Collider other)
    {
        var layer = 1 << (other.gameObject.layer - 1);
        if ((!useTag ||other.CompareTag(tag)) 
            && (layerMask.value & layer) == layer) 
            onExit.Invoke(other);
    }
}