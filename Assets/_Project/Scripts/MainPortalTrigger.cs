using UnityEngine;

public class MainPortalTrigger : MonoBehaviour, IInteractable
{
    [SerializeField] private string tip;
    public string Tip => tip;
    [SerializeField] private string description;
    public string Description => description;
    [SerializeField] private bool canUse;
    public bool CanUse => canUse;

    [SerializeField] private MainPortal mainPortal;

    public void Interact(Interaction player)
    {
        if (!player.GetInventory().RemoveItem(4)) return;

        mainPortal.AddProgress();
    }

    public void Hover(Interaction player)
    {
        
    }

    public void EndHover(Interaction player)
    {
        
    }
}
