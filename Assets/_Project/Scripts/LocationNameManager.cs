using UnityEngine;
using UnityEngine.UI;

public class LocationNameManager : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Image imgLocName;
    
    public void AppearName(Sprite locName){
        imgLocName.sprite = locName;
        animator.Play("Appear", 0, 0);
    }
}
