using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.Video;

public class VHCController : MonoBehaviour
{
    [SerializeField] private Animator camAnim;
    [SerializeField] private Animator animFade;
    [SerializeField] private InputActionReference F;
    [SerializeField] private GameObject CamPref;

    [SerializeField] private VHSWorldManager vhsWorldManager;
    
    [SerializeField] private GameObject VHSCanvas;
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private Volume volume;
    [SerializeField] private VolumeProfile DefaultVolume;
    [SerializeField] private VolumeProfile VHSVolume;
    [SerializeField] private ScriptableRendererFeature[] VHSscreenEffects;

    private bool VHSActive;
    private bool inAnimation;

    void OnEnable(){
        F.action.performed += ctx => OpenVHC();
    }
    void OnDisable(){
        F.action.performed -= ctx => OpenVHC();
        for(int i = 0; i < VHSscreenEffects.Length; i++){
            VHSscreenEffects[i].SetActive(false);
        }
    }

    void OpenVHC(){
        if (inAnimation) return;
        inAnimation = true;

        if (!VHSActive){
            VHSActive = true;
            CamPref.SetActive(true);
            camAnim.SetTrigger("OpenVHC");
        }
        else{
            StartFade();
            VHSActive = false;
            Invoke("closeCam", 0.5f);
        }
    }

    void closeCam(){
        vhsWorldManager.VHSDisabled();
        CamPref.SetActive(true);
        LoadDefaultSettings();
        animFade.SetTrigger("FadeOut");
        camAnim.SetTrigger("CloseVHC");
    }

    public void StartFade(){
        animFade.SetTrigger("FadeIn");
    }

    void LoadDefaultSettings(){
        for(int i = 0; i < VHSscreenEffects.Length; i++){
            VHSscreenEffects[i].SetActive(false);
        }
        volume.profile = DefaultVolume;
        videoPlayer.Stop();
        VHSCanvas.SetActive(false);
    }

    public void SetupCamVHC(){
        inAnimation = false;
        if(!VHSActive){
            CamPref.SetActive(false);
            return;
        }
        vhsWorldManager.VHSEnabled();
        CamPref.SetActive(false);
        animFade.SetTrigger("FadeOut");
        for(int i = 0; i < VHSscreenEffects.Length; i++){
            VHSscreenEffects[i].SetActive(true);
        }
        volume.profile = VHSVolume;
        videoPlayer.Play();
        VHSCanvas.SetActive(true);
    }
}
