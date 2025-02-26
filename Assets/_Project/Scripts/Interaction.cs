using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interaction : MonoBehaviour
{
    [SerializeField] private float distInteract;
    [SerializeField] private LayerMask layerMask;

    [SerializeField] private InputActionReference interactionButton;

    [SerializeField] private GameObject tipObj;
    [SerializeField] private TMP_Text tip;

    [SerializeField] private TMP_Text description;

    private GameObject currentInteractionObj;
    private IInteractable currentInteraction;

    [SerializeField] private Inventory inventory;

    void OnEnable()
    {
        interactionButton.action.performed += ctx => TryInteract();
    }
    void OnDisable()
    {
        interactionButton.action.performed -= ctx => TryInteract();
    }

    void TryInteract(){
        if (currentInteraction == null || !currentInteraction.CanUse) return;
        currentInteraction.Interact(this);
    }

    void Update()
    {
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
}
