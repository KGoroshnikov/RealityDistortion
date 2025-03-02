using System.Collections.Generic;
using _Project.Scripts.Saves;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class GalleryManager : SaveableBehaviour
{
    [SerializeField] private Animator wallAnimator;

    [SerializeField] private ScriptableRendererFeature VHSscreenEffects;

    private Vector3 _wallStartPosition;
    
    private void Start()
    {
        Initialize();
        _wallStartPosition = wallAnimator.transform.position;
    }

    public void ActivateLever()
    {
        wallAnimator.enabled = true;
        wallAnimator.Play("MoveWall", 0, 0);
        SetState("ActivateLever");
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
        if (!states.ContainsKey("ActivateLever"))
            wallAnimator.transform.position = _wallStartPosition;
    }

    public override void ApplyState(Dictionary<string, object> states)
    {
        
    }
    public override void OnCommit() { }
}
