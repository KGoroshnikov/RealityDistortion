using System;
using System.Collections.Generic;
using _Project.Scripts.Saves;
using TMPro;
using UnityEngine;

public class IslandOfDeathManager : SaveableBehaviour
{
    [SerializeField] private PlayerContoller playerContoller;
    [SerializeField] private Inventory inventory;
    [SerializeField] private MoveObjects moveObjects;
    [SerializeField] private float timeMove;

    [SerializeField] private GameObject leverUI;
    [SerializeField] private TMP_Text leverText;

    [SerializeField] private Transform posPlayer;

    [SerializeField] private Anger angel;

    [SerializeField] private GameObject kletka;
    [SerializeField] private GameObject rockexit;

    [SerializeField] private LightGuy lightGuy;

    [SerializeField] private Transform portal;
    [SerializeField] private Transform[] portalPoses;

    private bool portalOnBoat;

    private int levaverActived;

    public void MovePlayer(){
        if (portalOnBoat) return;
        playerContoller.FreezePlayer(true);
        moveObjects.AddObjectToMove(playerContoller.gameObject, posPlayer.position, posPlayer.rotation, timeMove, PlayerMoved);
    }

    void PlayerMoved(){
        playerContoller.UnfreezePlayer(true);
        
    }

    public void MovePortalOnBoat(){
        portal.position = portalPoses[1].position;
        portal.rotation = portalPoses[1].rotation;
        portalOnBoat = true;

        inventory.RemoveItem(2);
        inventory.AddBucket();
    }

    public void ActivateAngel(){
        UseLever();
        angel.ActivateMe();
        rockexit.SetActive(true);
    }

    public void KeyPicked(){
        lightGuy.PlayerHaveKey();
        angel.DisableMe();
        rockexit.SetActive(false);
    }

    public void LeverPicked(){
        lightGuy.PlayerHaveLever();
    }

    public void UseLever(){
        levaverActived++;
        if (!leverUI.activeSelf) leverUI.SetActive(true);
        leverText.text = levaverActived + "/4";

        if (levaverActived >= 4) OpenGates();
    }

    void OpenGates(){
        leverUI.SetActive(false);
        kletka.SetActive(false);
    }

    private void Start() => Initialize();
    public override void ResetState(Dictionary<string, object> states)
    {
        throw new NotImplementedException();
    }
    public override void ApplyState(Dictionary<string, object> states)
    {
        throw new NotImplementedException();
    }
}
