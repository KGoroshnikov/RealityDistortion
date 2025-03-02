using System.Collections.Generic;
using _Project.Scripts.Saves;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class GalleryManager : SaveableBehaviour
{
    [SerializeField] private Animator wallAnimator;

    [SerializeField] private ScriptableRendererFeature VHSscreenEffects;

    private void Start() => Initialize();
    public void ActivateLever(){
        wallAnimator.enabled = true;
        wallAnimator.Play("MoveWall", 0, 0);
    }

    void OnEnable()
    {
        VHSscreenEffects.SetActive(false);
    }

    void OnDisable()
    {
        VHSscreenEffects.SetActive(false);
    }

    public void ActivateBlackAndWhite(bool onOff){
        VHSscreenEffects.SetActive(onOff);
    }

    public override void ResetState(Dictionary<string, object> states)
    {
        throw new System.NotImplementedException();
    }

    public override void ApplyState(Dictionary<string, object> states)
    {
        throw new System.NotImplementedException();
    }
    public override void OnCommit() { }
}
