using System;
using System.Collections.Generic;
using _Project.Scripts.Saves;
using UnityEngine;
using UnityEngine.UI;

public class LocationNameManager : SaveableBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Image imgLocName;
    
    public void AppearName(Sprite locName){
        imgLocName.sprite = locName;
        animator.Play("Appear", 0, 0);
    }


    private void Start() => Initialize();

    public override void ResetState(Dictionary<string, object> states)
    {
        
    }

    public override void ApplyState(Dictionary<string, object> states)
    {
        imgLocName.sprite = states["LocationName"] as Sprite;
        animator.Play("Appear", 0, 0);
    }

    public override void OnCommit()
    {
        SetState("LocationName", imgLocName.sprite);
    }
}
