using System;
using UnityEngine;
using UnityEngine.Events;

public class PortalTraveller : MonoBehaviour
{
    public Vector3 previousOffsetFromPortal { get; set; }

    [SerializeField] private UnityEvent<Transform> onTeleport;
    [SerializeField] private UnityEvent onPreTeleport;
    
    private Rigidbody rb;

    private void Start() => rb = GetComponent<Rigidbody>();

    public virtual void Teleport (Transform fromPortal, Transform toPortal, Vector3 pos, Quaternion rot) {
        onPreTeleport.Invoke();
        transform.position = pos;
        transform.rotation = rot;
        if (rb)
        {
            rb.position = pos;
            rb.rotation = rot;
            rb.linearVelocity = Vector3.zero;
        }
        onTeleport.Invoke(fromPortal);
        //Debug.Log(gameObject + " TELEPORTED!: from " + fromPortal.gameObject + " " + fromPortal.position + " to: " + toPortal.gameObject + " " + toPortal.position + " prevpos: " + previousOffsetFromPortal);
    }

}
