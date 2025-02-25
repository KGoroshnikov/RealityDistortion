using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class VHSWorldManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> objectToHideInVHS = new List<GameObject>();
    [SerializeField] private List<GameObject> objectToShowInVHS = new List<GameObject>();
    [SerializeField] private UnityEvent onOpenVHS;
    [SerializeField] private UnityEvent onCloseVHS;

    [SerializeField] private VHSOverlay vHSOverlay;

    public void VHSEnabled(){
        for(int i = 0; i < objectToHideInVHS.Count; i++) objectToHideInVHS[i].SetActive(false);
        for(int i = 0; i < objectToShowInVHS.Count; i++) objectToShowInVHS[i].SetActive(true);
        onOpenVHS.Invoke();
    }
    public void VHSDisabled(){
        for(int i = 0; i < objectToHideInVHS.Count; i++) objectToHideInVHS[i].SetActive(true);
        for(int i = 0; i < objectToShowInVHS.Count; i++) objectToShowInVHS[i].SetActive(false);
        onCloseVHS.Invoke();
    }

    public void PlayerInDangerZone(){
        Debug.Log("sht");
        vHSOverlay.ActivateDNC();
    }
    public void PlayerExitedDangerZone(){
        vHSOverlay.DeactivateDNC();
    }
}
