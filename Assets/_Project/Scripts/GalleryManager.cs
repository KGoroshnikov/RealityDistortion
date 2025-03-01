using System.Collections.Generic;
using _Project.Scripts.Saves;
using UnityEngine;

public class GalleryManager : SaveableBehaviour
{
    [SerializeField] private Animator wallAnimator;

    private void Start() => Initialize();
    public void ActivateLever(){
        wallAnimator.enabled = true;
        wallAnimator.Play("MoveWall", 0, 0);
    }

    public override void ResetState(Dictionary<string, object> states)
    {
        throw new System.NotImplementedException();
    }

    public override void ApplyState(Dictionary<string, object> states)
    {
        throw new System.NotImplementedException();
    }
}
