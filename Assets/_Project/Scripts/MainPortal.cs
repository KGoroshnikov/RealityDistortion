using System.Collections.Generic;
using _Project.Scripts.Saves;
using UnityEngine;

public class MainPortal : SaveableBehaviour
{
    [SerializeField] private GameObject[] masks;
    private int currentProgress;

    [SerializeField] private GameObject paintTrigger;

    [SerializeField] private Animator doorAnimator;

    [SerializeField] private GreenLandManager greenLandManager;
    private bool doorOpened;

    void Start() => Initialize();

    public void AddProgress(){
        AddProgressWithoutSave();
        SetState("MainPortal_Progress", currentProgress);
        Commit();
    }

    private void AddProgressWithoutSave()
    {
        currentProgress++;
        for(int i = 0; i < currentProgress; i++){
            if (currentProgress - 1 >= masks.Length) break;
            masks[i].SetActive(true);
        }
        if (!doorOpened && currentProgress >= masks.Length){
            doorOpened = true;
            paintTrigger.SetActive(false);
            doorAnimator.enabled = true;
            doorAnimator.Play("OpenDoor", 0, 0);
            greenLandManager.Activate();
        }
    }

    public override void ResetState(Dictionary<string, object> states)
    {
        foreach (var mask in masks)
            mask.SetActive(false);
    }

    public override void ApplyState(Dictionary<string, object> states)
    {
        currentProgress = 0;
        var progress = (int)states.GetValueOrDefault("MainPortal_Progress", 0);
        for (var i = 0; i < progress; i++)
            AddProgressWithoutSave();
    }

    public override void OnCommit() { }
}
