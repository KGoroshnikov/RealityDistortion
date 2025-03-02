using UnityEngine;
using UnityEngine.Rendering.Universal;

public class GalleryManager : MonoBehaviour
{
    [SerializeField] private Animator wallAnimator;

    [SerializeField] private ScriptableRendererFeature VHSscreenEffects;

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
}
