using TMPro;
using UnityEngine;

public class IslandOfDeathManager : MonoBehaviour
{
    [SerializeField] private PlayerContoller playerContoller;
    [SerializeField] private MoveObjects moveObjects;
    [SerializeField] private float timeMove;

    [SerializeField] private GameObject leverUI;
    [SerializeField] private TMP_Text leverText;

    [SerializeField] private Transform posPlayer;

    [SerializeField] private Anger angel;

    [SerializeField] private GameObject kletka;
    [SerializeField] private GameObject rockexit;

    [SerializeField] private LightGuy lightGuy;

    private int levaverActived;

    public void MovePlayer(){
        playerContoller.FreezePlayer();
        moveObjects.AddObjectToMove(playerContoller.gameObject, posPlayer.position, posPlayer.rotation, timeMove, PlayerMoved);
    }

    void PlayerMoved(){
        playerContoller.UnfreezePlayer();
        
    }

    public void ActivateAngel(){
        UseLever();
        angel.ActivateMe();
        rockexit.SetActive(true);
    }

    public void KeyPicked(){
        lightGuy.PlayerHaveKey();
        angel.DisableMe();
        rockexit.SetActive(false);
    }

    public void LeverPicked(){
        lightGuy.PlayerHaveLever();
    }

    public void UseLever(){
        levaverActived++;
        if (!leverUI.activeSelf) leverUI.SetActive(true);
        leverText.text = levaverActived + "/4";

        if (levaverActived >= 4) OpenGates();
    }

    void OpenGates(){
        leverUI.SetActive(false);
        kletka.SetActive(false);
    }
}
