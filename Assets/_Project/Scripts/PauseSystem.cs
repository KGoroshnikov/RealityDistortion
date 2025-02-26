using UnityEngine;
using UnityEngine.InputSystem;

public class PauseSystem : MonoBehaviour
{
    [SerializeField] private GameObject PauseUI;
    [SerializeField] private GameObject pauseVolume;
    [SerializeField] private InputActionReference esc;
    [SerializeField] private PlayerContoller playerContoller;

    [SerializeField] private GameObject[] freezables;
    private IFreezable[] _freezables;

    [SerializeField] private Animator pauseAnimator;

    private bool paused;

    void Awake()
    {
        _freezables = new IFreezable[freezables.Length];
        for(int i = 0; i < freezables.Length; i++) _freezables[i] = freezables[i].GetComponent<IFreezable>();   
    }

    void OnEnable(){
        esc.action.performed += ctx => Pause();
    }

    public void Pause(){
        paused = !paused;

        PauseUI.SetActive(paused);
        pauseVolume.SetActive(paused);

        for(int i = 0; i < _freezables.Length; i++){
            if (paused) _freezables[i].Freeze();
            else _freezables[i].UnFreeze();
        }

        if (paused){
            playerContoller.FreezePlayer();
            Cursor.lockState = CursorLockMode.None;
        }
        else{
            playerContoller.UnfreezePlayer();
            Cursor.lockState = CursorLockMode.Locked;
        }
        Cursor.visible = paused;
    }

    public void PauseHover(){
        pauseAnimator.ResetTrigger("Leave");
        if (!pauseAnimator.enabled) pauseAnimator.enabled = true;
        else pauseAnimator.SetTrigger("Hover");
    }
    public void PauseLeave(){
        pauseAnimator.ResetTrigger("Hover");
        pauseAnimator.SetTrigger("Leave");
    }

    /*
            Stencil {
            Ref 10
            Comp Equal
        }
    */
}
