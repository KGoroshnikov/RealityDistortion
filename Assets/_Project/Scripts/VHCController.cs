using System;
using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.Saves;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.Video;

public class VHCController : SaveableBehaviour
{
    [SerializeField] private Animator camAnim;
    [SerializeField] private Animator animFade;
    [SerializeField] private InputActionReference F;
    private Action<InputAction.CallbackContext> fdelegate;
    [SerializeField] private GameObject CamPref;

    [SerializeField] private Inventory inventory;

    [SerializeField] private VHSWorldManager vhsWorldManager;
    
    [SerializeField] private GameObject VHSCanvas;
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private GameObject VHSVolume;
    [SerializeField] private ScriptableRendererFeature[] VHSscreenEffects;

    private bool VHSActive;
    private bool inAnimation;

    private bool canPressF = true;

    void Awake()
    {
        loadPriority = 100;
        Initialize();
        fdelegate = ctx => OpenVHC();   
    }

    public void SetInputRection(bool a){
        canPressF = a;
    }

    void OnEnable(){
        F.action.performed += fdelegate;
    }
    void OnDisable(){
        F.action.performed -= fdelegate;
        for(int i = 0; i < VHSscreenEffects.Length; i++){
            VHSscreenEffects[i].SetActive(false);
        }
    }

    private void OpenVHC(){
        if (inAnimation || !inventory.GetHaveCamera() || !canPressF) return;
        inAnimation = true;

        if (!VHSActive){
            VHSActive = true;
            CamPref.SetActive(true);
            camAnim.SetTrigger("OpenVHC");
            SetState("Camera_Active");
        }
        else{
            StartFade();
            VHSActive = false;
            Invoke("closeCam", 0.5f);
            RemoveState("Camera_Active");
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
        //volume.profile = DefaultVolume;
        VHSVolume.SetActive(false);
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
        //volume.profile = VHSVolume;
        VHSVolume.SetActive(true);
        videoPlayer.Play();
        VHSCanvas.SetActive(true);
    }

    public override void ResetState(Dictionary<string, object> states) { }

    public override void ApplyState(Dictionary<string, object> states)
    {
        VHSActive = states.ContainsKey("Camera_Active");
        Invoke(nameof(UpdateCameraState), 0.6f);
    }

    private void UpdateCameraState()
    {
        
        SetupCamVHC();
        if (!VHSActive) closeCam();
    }

    public override void OnCommit() { }
}
