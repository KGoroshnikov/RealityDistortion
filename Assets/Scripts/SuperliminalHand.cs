using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class SuperliminalHand : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Transform target;
    [SerializeField] private InputActionReference grab;
 
    [Header("Settings")]
    [SerializeField] private LayerMask targetMask;
    [SerializeField] private LayerMask ignoreTargetMask;
    [SerializeField] private float offsetFactor = 1;
    [SerializeField, Min(1)] private float maxDistance = 100;
    [SerializeField, Min(0)] private float minDistance = 1;
 
    private float originalDistance;
    private Vector3 originalScale;
    private float targetScale;
    private Transform originalParent;


    private void FixedUpdate()
    {
        HandleInput();
        ResizeTarget();
    }
 
    private void HandleInput()
    {
        if (grab.action.IsPressed())
        {
            if (target) return;
            if (!Physics.Raycast(transform.position, transform.forward, out var hit, Mathf.Infinity, targetMask)) return;
            
            target = hit.transform;
            if(target.TryGetComponent<Rigidbody>(out var rb)) rb.isKinematic = true;
            originalDistance = Vector3.Distance(transform.position, target.position);
            originalScale = target.localScale;
            targetScale = target.localScale.x;
            originalParent = target.parent;
            target.parent = transform;
        }
        else
        {
            if (!target) return;
            if(target.TryGetComponent<Rigidbody>(out var rb)) rb.isKinematic = false;
            target.parent = originalParent;
            target = null;
        }
    }
 
    private void ResizeTarget()
    {
        if (!target) return;
        Vector3 point;
        if (!Physics.Raycast(transform.position, transform.forward, out var hit, Mathf.Infinity, ignoreTargetMask))
            point = transform.position + transform.forward * maxDistance;
        else point = hit.point;
        target.position = point;
        var colliders = new Collider[16];
        for (var i = 0; i < 10; i++) 
        {
            var currentDistance = Vector3.Distance(transform.position, target.position);
            if (currentDistance < minDistance)
            {
                target.position += transform.forward * (minDistance - currentDistance);  
                targetScale = minDistance / originalDistance;
                target.localScale = targetScale * originalScale;
                break;
            }
            targetScale = currentDistance / originalDistance;
            target.localScale = targetScale * originalScale;
            if (Physics.OverlapBoxNonAlloc(target.position, target.lossyScale * 0.5f, 
                    colliders, target.rotation, ignoreTargetMask) == 0) return;
            target.position -= transform.forward * (offsetFactor * targetScale);
        }
    }

    private void OnDrawGizmos()
    {
        if (!target) return;
        Gizmos.color = Color.red;
        Gizmos.matrix = Matrix4x4.TRS(target.position, target.rotation, target.lossyScale);
        Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
    }
}
