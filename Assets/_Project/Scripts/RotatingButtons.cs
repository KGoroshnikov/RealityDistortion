using UnityEngine;

public class RotatingButtons : MonoBehaviour
{
    [SerializeField] private float speedRotation;
    [SerializeField] private float scaleSelected;

    [System.Serializable]
    public class mButton{
        public Transform button;
        public Transform img;
    }
    [SerializeField] private mButton[] buttons;

    [SerializeField] private AudioSource uiSounds;
    [SerializeField] private AudioClip selectClip;
    
    private int selected;

    void Update()
    {
        if (selected == -1) return;
        buttons[selected].img.localEulerAngles = new Vector3(buttons[selected].img.localEulerAngles.x, 
                buttons[selected].img.localEulerAngles.y + speedRotation * Time.deltaTime, buttons[selected].img.localEulerAngles.z);
    }

    public void Hover(int id){
        uiSounds.clip = selectClip;
        uiSounds.Play();
        selected = id;
        buttons[selected].button.localScale = new Vector3(scaleSelected, scaleSelected, scaleSelected);
    }

    public void Leave(int id){
        buttons[id].button.localScale = new Vector3(1, 1, 1);
        buttons[id].img.localEulerAngles = new Vector3(buttons[id].img.localEulerAngles.x, 0, buttons[id].img.localEulerAngles.z);
        selected = -1;
    }
}
