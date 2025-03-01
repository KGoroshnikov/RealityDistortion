using UnityEngine;
using UnityEngine.Events;

public class PortalTraveller : MonoBehaviour
{
    public Vector3 previousOffsetFromPortal { get; set; }

    [SerializeField] private UnityEvent<Transform> onTeleport;
    [SerializeField] private UnityEvent onPreTeleport;
    public virtual void Teleport (Transform fromPortal, Transform toPortal, Vector3 pos, Quaternion rot) {
        onPreTeleport.Invoke();
        transform.position = pos;
        transform.rotation = rot;
        onTeleport.Invoke(fromPortal);
    }

}
