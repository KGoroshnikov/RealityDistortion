using _Project.Scripts.Saves;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class DeathManager : MonoBehaviour
{
    [SerializeField] private SaveManager saveManager;
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private GameObject deathScreen;
    [SerializeField] private GameObject canvas;
    [SerializeField] private GameObject black;

    [SerializeField] private PlayerContoller playerContoller;

    [SerializeField] private ScriptableRendererFeature[] VHSscreenEffects;

    private bool died;

    void Start()
    {
        //Invoke("Die", 2);
    }

    void OnDisable(){
        for(int i = 0; i < VHSscreenEffects.Length; i++){
            VHSscreenEffects[i].SetActive(false);
        }
    }

    public void Die(){
        if (died) return;
        died = true;
        canvas.SetActive(false);
        deathScreen.SetActive(true);
        videoPlayer.Play();
        for(int i = 0; i < VHSscreenEffects.Length; i++){
            VHSscreenEffects[i].SetActive(true);
        }
        playerContoller.FreezePlayer();

        Invoke("BlackScreen", 2.5f);
    }

    void BlackScreen(){
        black.SetActive(true);
        Invoke("ReloadScene", 0.5f);
    }

    void ReloadScene(){
        saveManager.Revert();
        // SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
