using UnityEngine;

public class PickupableItem : MonoBehaviour, IInteractable
{
    [SerializeField] private string tip;
    public string Tip => tip;
    [SerializeField] private string description;
    public string Description => description;
    [SerializeField] private bool canUse;
    public bool CanUse => canUse;
    [SerializeField] private int ID;

    [System.Serializable]
    public class ItemIconData{
        public Sprite sprite;
        public Vector2 widthHeight;
        public float scale;
        public float blackOffset;
    }
    [SerializeField] private ItemIconData iconData;

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
        player.AddItem(ID, iconData);
        gameObject.SetActive(false);
    }
}
