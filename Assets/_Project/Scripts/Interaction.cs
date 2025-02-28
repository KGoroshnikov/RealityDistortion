using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interaction : MonoBehaviour, IFreezable
{
    [SerializeField] private float distInteract;
    [SerializeField] private LayerMask layerMask;

    [SerializeField] private InputActionReference interactionButton;
    private Action<InputAction.CallbackContext> eDelegate;

    [SerializeField] private GameObject tipObj;
    [SerializeField] private TMP_Text tip;

    [SerializeField] private TMP_Text description;

    private GameObject currentInteractionObj;
    private IInteractable currentInteraction;

    [SerializeField] private Inventory inventory;

    private bool active = true;

    void Awake()
    {
        eDelegate = ctx => TryInteract();
    }

    void OnEnable()
    {
        interactionButton.action.performed += eDelegate;
    }
    void OnDisable()
    {
        interactionButton.action.performed -= eDelegate;
    }

    void TryInteract(){
        if (!active || currentInteraction == null || !currentInteraction.CanUse) return;
        currentInteraction.Interact(this);
    }

    public void SetActive(bool a){
        active = a;
    }

    public void RefreshTips(){
        if (currentInteraction == null){
            description.text = "";
            tipObj.SetActive(false);
            return;
        }
        description.text = currentInteraction.Description;

        if (currentInteraction.CanUse){
            tipObj.SetActive(true);
            tip.text = currentInteraction.Tip;
        }
        else{
            tipObj.SetActive(false);
        }
    }

    void Update()
    {
        if (!active){
            if (currentInteraction != null){
                currentInteraction.EndHover(this);
                description.text = "";
                currentInteraction = null;
                currentInteractionObj = null;
                tipObj.SetActive(false);
            }
            return;
        }
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray.origin, ray.direction, out hit, distInteract, layerMask) && hit.collider.gameObject.CompareTag("Interactable")) {
            if (hit.collider.gameObject != currentInteractionObj && hit.collider.TryGetComponent<IInteractable>(out IInteractable interactable)){
                if (currentInteraction != null && interactable != currentInteraction) currentInteraction.EndHover(this);
                interactable.Hover(this);

                description.text = interactable.Description;

                currentInteraction = interactable;
                currentInteractionObj = hit.collider.gameObject;

                if (currentInteraction.CanUse){
                    tipObj.SetActive(true);
                    tip.text = currentInteraction.Tip;
                }
                else{
                    tipObj.SetActive(false);
                }
            }
        }else{
            if (currentInteraction != null) currentInteraction.EndHover(this);
            description.text = "";
            currentInteraction = null;
            currentInteractionObj = null;
            tipObj.SetActive(false);
        }
    }

    public void AddItem(int id, PickupableItem.ItemIconData itemIconData){
        inventory.AddItem(id, itemIconData);
    }

    public Inventory GetInventory(){
        return inventory;
    }

    public void Freeze()
    {
        throw new System.NotImplementedException();
    }

    public void UnFreeze()
    {
        throw new System.NotImplementedException();
    }
}
