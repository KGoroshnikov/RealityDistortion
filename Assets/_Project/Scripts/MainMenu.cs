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
    
    [SerializeField] private GameObject movieCanvas;
    [SerializeField] private Animator animatorMovie;
    [SerializeField] private GameObject[] slides;
    [SerializeField] private Animator fadeAnim;
    private int movieCntr;
    private bool playPressed;
    private bool watchingMovie;

    [SerializeField] private InputActionReference space, enter;

    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void Update()
    {
        if (!watchingMovie) return;
        if (space.action.triggered || enter.action.triggered){
            watchingMovie = false;
            fadeAnim.SetTrigger("FadeIn");
            Invoke("LoadGame", 2f);
        }
    }

    public void Click(int id){
        if (playPressed) return;
        uiSounds.clip = clickClip;
        uiSounds.Play();
        if (id == 0){
            playPressed = true;
            animatorCam.enabled = true;
            if (Random.value > 0.5f) animatorCam.Play("CamEnterGame", 0, 0);
            else animatorCam.Play("CamEnterGame2", 0, 0);
            animatorFade.SetTrigger("FadeIn");
            mainMenuCam.LockMousePos();
            Invoke("LoadMovie", 3f);
        }
        else if (id == 1){
            settingsObj.SetActive(!settingsObj.activeSelf);
        }
        else if (id == 2){
            Application.Quit();
        }
    }

    void LoadMovie(){
        watchingMovie = true;
        fadeAnim.SetTrigger("FadeOut");
        movieCanvas.SetActive(true);
        animatorMovie.enabled = true;
        animatorMovie.Play("Slide1", 0, 0);
    }

    void LoadGame(){
        SceneManager.LoadScene("GAME1");
    }

    public void FadeInMovie(){
        fadeAnim.SetTrigger("FadeIn");
    }
    public void SecondFunc(){
        movieCntr++;
        if (movieCntr == 1){
            fadeAnim.SetTrigger("FadeOut");
            slides[0].SetActive(false);
            slides[1].SetActive(true);
        }
        else if (movieCntr == 2){
            fadeAnim.SetTrigger("FadeIn");
            Invoke("LoadGame", 2f);
        }
    }
}
