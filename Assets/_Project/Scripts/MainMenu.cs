using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    [SerializeField] private Animator animatorCam;
    [SerializeField] private Animator animatorFade;

    [SerializeField] private MainMenuCam mainMenuCam;

    [SerializeField] private GameObject settingsObj;

    [SerializeField] private AudioSource uiSounds;
    [SerializeField] private AudioClip clickClip;

    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Click(int id){
        uiSounds.clip = clickClip;
        uiSounds.Play();
        if (id == 0){
            animatorCam.enabled = true;
            if (Random.value > 0.5f) animatorCam.Play("CamEnterGame", 0, 0);
            else animatorCam.Play("CamEnterGame2", 0, 0);
            animatorFade.SetTrigger("FadeIn");
            mainMenuCam.LockMousePos();
            Invoke("LoadGame", 3f);
        }
        else if (id == 1){
            settingsObj.SetActive(!settingsObj.activeSelf);
        }
        else if (id == 2){
            Application.Quit();
        }
    }

    void LoadGame(){
        SceneManager.LoadScene("GAME1");
    }
}
