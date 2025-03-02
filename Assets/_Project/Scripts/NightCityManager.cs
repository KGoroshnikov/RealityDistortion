using System.Collections.Generic;
using _Project.Scripts.Saves;
using UnityEngine;

public class NightCityManager : SaveableBehaviour
{
    [SerializeField] private Inventory inventory;
    [SerializeField] private MoveObjects moveObjects;
    [SerializeField] private Transform[] portalPoses;
    [SerializeField] private GameObject portalObj;
    [SerializeField] private Portal portal;

    private bool lvlCompleted;

    public void EndGame(){
        if (lvlCompleted) return;
        portal.onTeleport.AddListener(ResetPortalToDefault_Night);
        moveObjects.AddObjectToMove(portalObj, portalPoses[1].position, portalPoses[1].rotation, 1);
        SetState("NightCityManager_EndGame");
    }

    public void ResetPortalToDefault_Night(){
        if (lvlCompleted) return;
        lvlCompleted = true;
        inventory.AddBucket();
        portalObj.transform.position = portalPoses[0].position;
        portalObj.transform.rotation = portalPoses[0].rotation;
        portal.onTeleport.RemoveAllListeners();
        Commit();
    }

    private void Start() => Initialize();
    public override void ResetState(Dictionary<string, object> states) { }
    public override void ApplyState(Dictionary<string, object> states)
    {
        if (states.ContainsKey("NightCityManager_EndGame")) EndGame();
    }
    public override void OnCommit() { }
}
