using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class GalleryManager : MonoBehaviour
{
    [SerializeField] private Animator wallAnimator;

    [SerializeField] private ScriptableRendererFeature VHSscreenEffects;

    private bool greenlandOpened;

    [SerializeField] private GameObject[] AllLocations;
    // 0 - Greenlands and final scene
    // 1 - Island
    // 2 - night city
    // 3 - scream
    // 4 - black square

    void Start()
    {
        for(int i = 0; i < AllLocations.Length; i++){
            AllLocations[i].SetActive(false);
        }
    }

    public void ActivateLever(){
        wallAnimator.enabled = true;
        wallAnimator.Play("MoveWall", 0, 0);
    }

    public void OpenGreenLand(){
        greenlandOpened = true;
        AllLocations[0].SetActive(true);
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

    public void EnableIslandAndNightCity(){
        AllLocations[0].SetActive(false);
        AllLocations[3].SetActive(false);
        AllLocations[4].SetActive(false);

        AllLocations[1].SetActive(true);
        AllLocations[2].SetActive(true);
    }
    public void EnableScream(){
        AllLocations[0].SetActive(false);
        AllLocations[1].SetActive(false);
        AllLocations[2].SetActive(false);
        AllLocations[4].SetActive(false);

        AllLocations[3].SetActive(true);
    }

    public void EnableBlackSquareAndGreenLand(){
        AllLocations[1].SetActive(false);
        AllLocations[2].SetActive(false);
        AllLocations[3].SetActive(false);

        if (greenlandOpened) AllLocations[0].SetActive(true);
        AllLocations[4].SetActive(true);
    }
}
