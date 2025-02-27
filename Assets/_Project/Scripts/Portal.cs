using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Portal : MonoBehaviour {

    public Portal linkedPortal;
    public MeshRenderer screen;

    public Camera portalCam;
    private Camera playerCam;

    public RenderTexture myRenderTex;

    private List<PortalTraveller> trackedTravellers = new List<PortalTraveller>();

    [SerializeField] private UnityEvent onTeleport;

    private bool invokeTP;

    void Awake(){
        myRenderTex = new RenderTexture(Screen.width, Screen.height, 1);
        portalCam.targetTexture = myRenderTex;
        playerCam = Camera.main;
        screen.material.SetInt("displayMask", 1);
        screen.sharedMaterial.SetTexture("_MainTex", myRenderTex);
    }
    void FixedUpdate(){
        if (invokeTP){
            onTeleport.Invoke();
            invokeTP = false;
        }
        ProtectScreenFromClipping(playerCam.transform.position);
        HandleTravellers();
    }

    void HandleTravellers(){
        for (int i = 0; i < trackedTravellers.Count; i++) {
            PortalTraveller traveller = trackedTravellers[i];
            Transform travellerT = traveller.transform;
            var m = linkedPortal.transform.localToWorldMatrix * transform.worldToLocalMatrix * travellerT.localToWorldMatrix;

            Vector3 offsetFromPortal = travellerT.position - transform.position;
            int portalSide = System.Math.Sign(Vector3.Dot(offsetFromPortal, transform.forward));
            int portalSideOld = System.Math.Sign(Vector3.Dot(traveller.previousOffsetFromPortal, transform.forward));
            if (portalSide != portalSideOld){
                linkedPortal.ProtectScreenFromClipping(m.GetColumn(3));
                var positionOld = travellerT.position;
                var rotOld = travellerT.rotation;
                traveller.Teleport(transform, linkedPortal.transform, m.GetColumn(3), m.rotation);
                linkedPortal.OnTravellerEnterPortal(traveller);
                trackedTravellers.RemoveAt(i);
                invokeTP = true;
                //onTeleport.Invoke();
                i--;

            } else{
                traveller.previousOffsetFromPortal = offsetFromPortal;
            }
        }
    }

    float ProtectScreenFromClipping(Vector3 viewPoint){
        float halfHeight = playerCam.nearClipPlane * Mathf.Tan(playerCam.fieldOfView * 0.5f * Mathf.Deg2Rad);
        float halfWidth = halfHeight * playerCam.aspect;
        float dstToNearClipPlaneCorner = new Vector3(halfWidth, halfHeight, playerCam.nearClipPlane).magnitude;
        float screenThickness = dstToNearClipPlaneCorner;

        Transform screenT = screen.transform;
        bool camFacingSameDirAsPortal = Vector3.Dot(transform.forward, transform.position - viewPoint) > 0;
        screenT.localScale = new Vector3(screenT.localScale.x, screenT.localScale.y, 0.01f);
        screenT.localPosition = Vector3.forward * screenThickness * 8 * ((camFacingSameDirAsPortal) ? 0.5f : -0.5f);
        return screenThickness;
    }

    public void Render(){
        if (!CamFuncs.VisibleFromCamera(linkedPortal.screen, playerCam)){
            
        }
        Matrix4x4 localToWorldMatrix = playerCam.transform.localToWorldMatrix;

        linkedPortal.portalCam.projectionMatrix = playerCam.projectionMatrix;
        localToWorldMatrix = transform.localToWorldMatrix * linkedPortal.transform.worldToLocalMatrix * localToWorldMatrix;
        linkedPortal.portalCam.transform.position = localToWorldMatrix.GetColumn(3);
        linkedPortal.portalCam.transform.rotation = localToWorldMatrix.rotation;

    }

    void OnTravellerEnterPortal(PortalTraveller traveller){
        if (!trackedTravellers.Contains(traveller)){
            traveller.previousOffsetFromPortal = traveller.transform.position - transform.position;
            trackedTravellers.Add(traveller);
        }
    }

    void OnTriggerEnter(Collider other){
        var traveller = other.GetComponent<PortalTraveller>();
        if (traveller){
            OnTravellerEnterPortal(traveller);
        }
    }

    void OnTriggerExit(Collider other){
        var traveller = other.GetComponent<PortalTraveller>();
        if (traveller && trackedTravellers.Contains(traveller)){
            trackedTravellers.Remove(traveller);
        }
    }
}
