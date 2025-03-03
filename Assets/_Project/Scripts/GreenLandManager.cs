using UnityEngine;
using UnityEngine.SceneManagement;

public class GreenLandManager : MonoBehaviour
{
    [SerializeField] private Renderer seeObj;

    [SerializeField] private PlayerContoller playerContoller;
    [SerializeField] private GameObject player;

    [SerializeField] private Transform endPos;

    [SerializeField] private MoveObjects moveObjects;

    [SerializeField] private float timeMove;

    [SerializeField] private Animator fadeAnim;
    [SerializeField] private Transform[] posesLastCS;

    private bool active;
    private bool gameEnded;

    private bool VHSActived;

    public void Activate(){
        active = true;
    }

    public void SetVHS(bool a){
        VHSActived = a;
    }

    void Update()
    {
        if (!active || gameEnded || !VHSActived) return;

        if (CamFuncs.VisibleFromCamera(seeObj, Camera.main)){
            gameEnded = true;
            EndGame();
        }
    }

    void EndGame(){
        playerContoller.FreezePlayer(true);
        playerContoller.ResetCamRot();
        moveObjects.AddObjectToMove(player, endPos.position, endPos.rotation, timeMove, PlayerMoved);
    }

    void PlayerMoved(){
        Invoke("PlayFade", 2.5f);
        Invoke("LoadLastScene", 3);
    }

    void PlayFade(){
        fadeAnim.SetTrigger("FadeIn");
    }

    void LoadLastScene(){
        player.transform.position = posesLastCS[0].position;
        player.transform.rotation = posesLastCS[0].rotation;
        fadeAnim.SetTrigger("FadeOut");
        Invoke("MovePlayerToPainting", 1.5f);
    }

    void MovePlayerToPainting(){
        moveObjects.AddObjectToMove(player, posesLastCS[1].position, posesLastCS[1].rotation, 3, PlayerAtFinalPos);
    }

    void PlayerAtFinalPos(){
        Invoke("PlayFade", 2.5f);
        Invoke("LoadMenu", 3f);
    }

    void LoadMenu(){
        SceneManager.LoadScene("MENU");
    }
}
