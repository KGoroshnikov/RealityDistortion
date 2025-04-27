using System.Collections.Generic;
using _Project.Scripts.Saves;
using UnityEngine;
using UnityEngine.Events;

public class Lever : SaveableBehaviour, IInteractable
{
    [SerializeField] private Transform leverTransform;
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

    [SerializeField] private AudioSource audioSource;

    private bool interacted;
    private Material origMat;

    public void Interact(Interaction player)
    {
        if (interacted) return;
        interacted = true;
        audioSource.Play();
        
        gameObject.tag = "Untagged";
        for(int i = 0; i < meshes.Length; i++) meshes[i].material = defaultMat;
        onActivate.Invoke();
        SetState($"Lever_{name}_{Guid}_Activated");
    }

    public void Hover(Interaction player)
    {
        
    }

    public void EndHover(Interaction player)
    {
        
    }
    
    private void Start()
    {
        Initialize();
        origMat = meshes[0].material;
    }

    public override void ResetState(Dictionary<string, object> states) { }
    public override void ApplyState(Dictionary<string, object> states)
    {
        interacted = false;
        gameObject.tag = "Interactable";
        onDeactivate.Invoke();
        for(int i = 0; i < meshes.Length; i++) meshes[i].material = origMat;
        if (states.ContainsKey($"Lever_{name}_{Guid}_Activated")) Interact(null);
    }
    public override void OnCommit() { }
}
