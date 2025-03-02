using UnityEngine;

public class NightCityManager : MonoBehaviour
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
    }

    public void ResetPortalToDefault_Night(){
        if (lvlCompleted) return;
        lvlCompleted = true;
        inventory.AddBucket();
        portalObj.transform.position = portalPoses[0].position;
        portalObj.transform.rotation = portalPoses[0].rotation;
        portal.onTeleport.RemoveAllListeners();
    }
}
