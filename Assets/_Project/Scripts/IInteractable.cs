using UnityEngine;

public interface IInteractable
{
    public string Tip { get; }
    public string Description { get; }

    public bool CanUse { get; }

    public void Interact(Interaction player);

    public void Hover(Interaction player);
    public void EndHover(Interaction player);
}
