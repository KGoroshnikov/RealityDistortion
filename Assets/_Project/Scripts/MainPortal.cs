using Unity.VisualScripting;
using UnityEngine;

public class MainPortal : MonoBehaviour
{
    [SerializeField] private GameObject[] masks;
    private int currentProgress;

    [SerializeField] private GameObject paintTrigger;

    [SerializeField] private Animator doorAnimator;

    [SerializeField] private GreenLandManager greenLandManager;
    [SerializeField] private GalleryManager galleryManager;

    [SerializeField] private AudioSource doorOpenSound;
    private bool doorOpened;

    void Start()
    {
        
    }

    public void AddProgress(){
        currentProgress++;
        for(int i = 0; i < currentProgress; i++){
            if (currentProgress - 1 >= masks.Length) break;
            masks[i].SetActive(true);
        }
        if (!doorOpened && currentProgress >= masks.Length){
            doorOpened = true;
            paintTrigger.SetActive(false);
            doorAnimator.enabled = true;
            doorAnimator.Play("OpenDoor", 0, 0);
            greenLandManager.Activate();
            doorOpenSound.Play();
            galleryManager.OpenGreenLand();
        }
    }
    
}
