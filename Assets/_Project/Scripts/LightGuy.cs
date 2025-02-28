using UnityEngine;

public class LightGuy : MonoBehaviour, IInteractable
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
        playerHaveKey = true;
        canUse = true;
        tip = "Отдать ключ";
    }

    public void PlayerHaveLever(){
        playerTookLever = true;
        canUse = true;
        tip = "Уйти";
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
        
    }

    public void EndHover(Interaction player)
    {
        
    }
}
