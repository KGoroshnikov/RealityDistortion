using UnityEngine;
using UnityEngine.Events;

public class BrokenLever : MonoBehaviour, IInteractable
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

    [SerializeField] private GameObject leverObj;

    private bool interacted;

    void Start(){
        leverObj.SetActive(false);
    }

    public void Interact(Interaction player)
    {
        if (!leverObj.activeSelf &&player.GetInventory().RemoveItem(3)){
            leverObj.SetActive(true);
            tip = "АКТИВИРОВАТЬ";
            description = "Откроет тайный проход";
            player.RefreshTips();
        }
        else if (leverObj.activeSelf){
            if (interacted) return;
            interacted = true;

            gameObject.tag = "Untagged";
            for(int i = 0; i < meshes.Length; i++) meshes[i].material = defaultMat;

            onActivate.Invoke();
        }

    }

    public void Hover(Interaction player)
    {
        
    }

    public void EndHover(Interaction player)
    {
        
    }
}
