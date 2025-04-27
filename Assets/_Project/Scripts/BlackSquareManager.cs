using System;
using System.Collections.Generic;
using _Project.Scripts.Saves;
using UnityEngine;
using UnityEngine.Events;

public class BlackSquareManager : SaveableBehaviour
{   
    [SerializeField] private Transform startPosPortal;
    [SerializeField] private Transform endPosPortal;
    [SerializeField] private Transform portalTrans;
    [SerializeField] private Portal portal;
    [SerializeField] private Inventory inventory;
    private bool lvlPassed;
    
    [SerializeField] private UnityEvent onRespawn;

    public void TeleportPortalToExit(){
        if (lvlPassed) return;

        portalTrans.position = endPosPortal.position;
        portalTrans.rotation = endPosPortal.rotation;

        portal.onTeleport.AddListener(PlayerTeleportedBack);

        lvlPassed = true;
        SetState("BlackSquareManager_lvlPassed");
    }

    void PlayerTeleportedBack(){
        portal.onTeleport.RemoveListener(PlayerTeleportedBack);
        inventory.AddBucket();
        Commit();
    }


    private void Start() => Initialize();

    public override void ResetState(Dictionary<string, object> states)
    {
        
    }

    public override void ApplyState(Dictionary<string, object> states)
    {
        if (states.ContainsKey("BlackSquareManager_lvlPassed"))
            TeleportPortalToExit();
    }

    public override void OnCommit()
    {
    }

    public void RespawnAll() => Invoke(nameof(RespawnAllCall), 0);
    private void RespawnAllCall() => onRespawn.Invoke();
}
