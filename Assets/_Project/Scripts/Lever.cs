using System.Collections.Generic;
using _Project.Scripts.Saves;
using UnityEngine;
using UnityEngine.Events;

public class Lever : SaveableBehaviour, IInteractable
{
    [SerializeField] private string tip;
    public string Tip => tip;
    [SerializeField] private string description;
    public string Description => description;
    [SerializeField] private bool canUse;
    public bool CanUse => canUse;
    [SerializeField] private int ID;

    [SerializeField] private MeshRenderer[] meshes;
    [SerializeField] private Material defaultMat;
    [SerializeField] private UnityEvent onActivate;
    [SerializeField] private UnityEvent onDeactivate;

    private bool interacted;

    public void Interact(Interaction player)
    {
        if (interacted) return;
        interacted = true;

        gameObject.tag = "Untagged";
        for(int i = 0; i < meshes.Length; i++) meshes[i].material = defaultMat;

        onActivate.Invoke();
    }

    public void Hover(Interaction player)
    {
        
    }

    public void EndHover(Interaction player)
    {
        
    }
    
    private void Start() => Initialize();
    public override void ResetState(Dictionary<string, object> states)
    {
        onDeactivate.Invoke();
    }
    public override void ApplyState(Dictionary<string, object> states)
    {
        if (states.ContainsKey($"{name}_{ID}_Activated"))
            onActivate.Invoke();
    }
    public override void OnCommit() { }
}
