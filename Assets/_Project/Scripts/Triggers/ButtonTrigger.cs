using System;
using System.Collections.Generic;
using Triggers;
using UnityEngine;
using UnityEngine.Events;

public class ButtonTrigger : AbstractTrigger
{ 
    [SerializeField] private bool useTag = true;
    [SerializeField] private string tag;
    [SerializeField] private LayerMask layerMask;
    [SerializeField, Min(0)] private float detectTime = 0.1f;
    
    [SerializeField] private UnityEvent onActivate;
    [SerializeField] private UnityEvent onDeactivate;
    
    private void OnTriggerEnter(Collider other)
    {
        var layer = 1 << (other.gameObject.layer - 1);
        if ((useTag && !other.CompareTag(tag))
            || (layerMask.value & layer) != layer) return;
        Invoke(nameof(Activate), detectTime);
        CancelInvoke(nameof(Deactivate));
    }
    private void OnTriggerExit(Collider other)
    {
        var layer = 1 << (other.gameObject.layer - 1);
        if ((useTag && !other.CompareTag(tag))
            || (layerMask.value & layer) != layer) return;
        Invoke(nameof(Deactivate), detectTime);
        CancelInvoke(nameof(Activate));
    }

    private void Start() => Initialize();

    public void Activate() => onActivate.Invoke();

    public void Deactivate() => onDeactivate.Invoke();

    public override void ResetState(Dictionary<string, object> states)
    {
        onDeactivate.Invoke();
    }

    public override void ApplyState(Dictionary<string, object> states) { }
    public override void OnCommit() { }
}