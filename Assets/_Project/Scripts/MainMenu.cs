using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    [SerializeField] private Animator animatorCam;
    [SerializeField] private Animator animatorFade;

    [SerializeField] private MainMenuCam mainMenuCam;

    public void Click(int id){
        if (id == 0){
            animatorCam.enabled = true;
            if (Random.value > 0.5f) animatorCam.Play("CamEnterGame", 0, 0);
            else animatorCam.Play("CamEnterGame2", 0, 0);
            animatorFade.SetTrigger("FadeIn");
            mainMenuCam.LockMousePos();
            Invoke("LoadGame", 3f);
        }
    }

    void LoadGame(){
        SceneManager.LoadScene("GAME1");
    }
}
