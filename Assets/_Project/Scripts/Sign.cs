using UnityEngine;

public class Sign : MonoBehaviour, IInteractable
{
    [SerializeField] private string tip;
    public string Tip => tip;
    [SerializeField] private string description;
    public string Description => description;
    [SerializeField] private bool canUse;
    public bool CanUse => canUse;

    public void EndHover(Interaction player)
    {
        Debug.Log("Stop Hover");
    }

    public void Hover(Interaction player)
    {
        Debug.Log("Hover");
    }

    public void Interact(Interaction player)
    {
        Debug.Log("Interact");
    }

}
