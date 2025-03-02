using UnityEngine;

public class GalleryPortals : MonoBehaviour
{
    [SerializeField] private GameObject[] portals;

    [SerializeField] private GameObject[] paintingsNoVHS;

    // 0 - island
    // 1 - night
    // 2 - scream
    // 3 - black square

    private int currentLocation = -1;

    private bool isVhsOpened;

    void Start()
    {
        VHSClosed();
    }

    public void SetLocation(int newloc){
        currentLocation = newloc;
        if (!isVhsOpened) VHSClosed();
    }

    public void VHSOpened(){
        isVhsOpened = true;
        for(int i = 0; i < portals.Length; i++){
            portals[i].SetActive(true);
            paintingsNoVHS[i].SetActive(false);
        }
    }

    public void VHSClosed(){
        isVhsOpened = false;
        for(int i = 0; i < portals.Length; i++){
            if (i == currentLocation) continue;
            portals[i].SetActive(false);
            paintingsNoVHS[i].SetActive(true);
        }
    }
}
