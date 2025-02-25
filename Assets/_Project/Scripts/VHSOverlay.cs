using TMPro;
using UnityEngine;

public class VHSOverlay : MonoBehaviour
{
    [SerializeField] private GameObject dNCObj;
    [SerializeField] private bool dNCActive;
    [SerializeField] private float dNCTime;

    [SerializeField] private TMP_Text[] timeText;

    public void ActivateDNC(){
        dNCActive = true;
        dNCObj.SetActive(true);
        InvokeRepeating("FlickDNC", dNCTime, dNCTime);
    }
    public void DeactivateDNC(){
        dNCActive = false;
        CancelInvoke("FlickDNC");
        dNCObj.SetActive(false);
    }

    void FlickDNC(){
        if (!dNCActive){
            return;
        }
        dNCObj.SetActive(!dNCObj.activeSelf);
    }

    void Update()
    {
        for(int i = 0; i < timeText.Length; i++){
            int secs = (int)Time.timeSinceLevelLoad % 60;
            string s = secs < 10 ? ("0"+secs) : (secs+"");
            int mins = (int)Time.timeSinceLevelLoad / 60;
            string m = mins < 10 ? ("0"+mins) : (mins+"");
            int hrs = mins / 60;
            string h = hrs < 10 ? ("0"+hrs) : (hrs+"");
            timeText[i].text = h + ":" + m + ":" + s;
        }
    }
}
