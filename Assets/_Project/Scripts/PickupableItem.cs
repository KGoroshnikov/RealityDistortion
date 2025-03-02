using System;
using System.Collections.Generic;
using _Project.Scripts.Saves;
using UnityEngine;
using UnityEngine.Events;

public class PickupableItem : SaveableBehaviour, IInteractable
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
        public string name;
        public Sprite sprite;
        public Vector2 widthHeight;
        public float scale;
        public float blackOffset;
    }
    [SerializeField] private ItemIconData iconData;

    [SerializeField] private UnityEvent pickedEvent;

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
        pickedEvent.Invoke();
        gameObject.SetActive(false);
        SetState($"Item_{name}_{ID}_{Guid}_Used");
    }

    private void Start() => Initialize();

    public override void ResetState(Dictionary<string, object> states) { }

    public override void ApplyState(Dictionary<string, object> states)
    {
        gameObject.SetActive(!states.ContainsKey($"Item_{name}_{ID}_{Guid}_Used"));
    }

    public override void OnCommit() { }
}
