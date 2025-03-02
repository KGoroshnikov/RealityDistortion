using System;
using System.Collections.Generic;
using _Project.Scripts.Saves;
using UnityEngine;

public class LightGuy : SaveableBehaviour, IInteractable
{
    [SerializeField] private string tip;
    public string Tip => tip;
    [SerializeField] public string description;
    public string Description => description;
    [SerializeField] private bool canUse;
    public bool CanUse => canUse;
    [SerializeField] private int ID;

    [SerializeField]private IslandOfDeathManager islandOfDeathManager;

    private bool playerTookLever;
    private bool playerHaveKey;

    public void PlayerHaveKey(){
        //playerHaveKey = true;
        canUse = true;
        tip = "Отдать ключ";
        Initialize();
        SetState("PlayerHaveKey");
    }

    public void PlayerHaveLever(){
        //playerTookLever = true;
        canUse = true;
        tip = "Уйти";
        Initialize();
        SetState("PlayerHaveLever");
    }

    public void Interact(Interaction player)
    {
        canUse = false;
        tip = "";

        if (playerHaveKey && playerTookLever){
            description = "ПОКА!";
            islandOfDeathManager.MovePortalOnBoat();
        }
        else if (playerHaveKey && !playerTookLever)
            description = "ТЫ ЗАБЫЛ РЫЧАГ! ОН НЕПОДАЛЕКУ ОТ ПОДЗЕМЕЛЬЯ";
        else description = "У ТЕБЯ НЕТ КЛЮЧА! ОН В ПОДЗЕМЕЛЬЕ";

        player.RefreshTips();
    }

    public void Hover(Interaction player)
    {
        if (player.GetInventory().HaveItem(2) && !playerHaveKey){
            playerHaveKey = true;
            PlayerHaveKey();
        }
        if (player.GetInventory().HaveItem(3) && !playerTookLever){
            playerTookLever = true;
            PlayerHaveLever();
        }
    }

    public void EndHover(Interaction player)
    {
        
    }

    private void Start() => Initialize();

    public override void ResetState(Dictionary<string, object> states) { }

    public override void ApplyState(Dictionary<string, object> states)
    {
        playerHaveKey = (bool)states.GetValueOrDefault("PlayerHaveKey", false);
        playerTookLever = (bool)states.GetValueOrDefault("PlayerTookLever", false);
    }

    public override void OnCommit() { }
}
