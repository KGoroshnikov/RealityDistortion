using UnityEngine;
using UnityEngine.Events;

public class EventInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private string tip;
    [SerializeField] private string description;
    public bool canUae = true;

    [SerializeField] private UnityEvent<Interaction> onInteract;
    [SerializeField] private UnityEvent<Interaction> onHover;
    [SerializeField] private UnityEvent<Interaction> onHoverEnd;
    
    public string Tip => tip;
    public string Description => description;
    public bool CanUse => canUae;
    public void Interact(Interaction player) => onInteract.Invoke(player);

    public void Hover(Interaction player) => onHover.Invoke(player);

    public void EndHover(Interaction player) => onHoverEnd.Invoke(player);
}