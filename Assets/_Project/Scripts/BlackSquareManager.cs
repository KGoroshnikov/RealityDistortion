using UnityEngine;

public class BlackSquareManager : MonoBehaviour
{   
    [SerializeField] private Transform startPosPortal;
    [SerializeField] private Transform endPosPortal;
    [SerializeField] private Transform portalTrans;
    [SerializeField] private Portal portal;
    [SerializeField] private Inventory inventory;
    private bool lvlPassed;

    public void TeleportPortalToExit(){
        if (lvlPassed) return;

        portalTrans.position = endPosPortal.position;
        portalTrans.rotation = endPosPortal.rotation;

        portal.onTeleport.AddListener(PlayerTeleportedBack);

        lvlPassed = true;
    }

    void PlayerTeleportedBack(){
        portal.onTeleport.RemoveListener(PlayerTeleportedBack);
        inventory.AddBucket();
    }
    

}
