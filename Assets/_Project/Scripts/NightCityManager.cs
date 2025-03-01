using System.Collections.Generic;
using _Project.Scripts.Saves;
using UnityEngine;

public class NightCityManager : SaveableBehaviour
{
    [SerializeField] private MoveObjects moveObjects;
    [SerializeField] private Transform[] portalPoses;
    [SerializeField] private GameObject portalObj;
    [SerializeField] private Portal portal;

    public void EndGame(){
        portal.onTeleport.AddListener(ResetPortalToDefault_Night);
        moveObjects.AddObjectToMove(portalObj, portalPoses[1].position, portalPoses[1].rotation, 1);
    }

    public void ResetPortalToDefault_Night(){
        Debug.Log("ASdfgasasas");
        portalObj.transform.position = portalPoses[0].position;
        portalObj.transform.rotation = portalPoses[0].rotation;
        portal.onTeleport.RemoveAllListeners();
    }

    private void Start() => Initialize();
    public override void ResetState(Dictionary<string, object> states)
    {
        throw new System.NotImplementedException();
    }
    public override void ApplyState(Dictionary<string, object> states)
    {
        throw new System.NotImplementedException();
    }
}
