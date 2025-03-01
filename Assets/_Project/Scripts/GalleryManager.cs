using UnityEngine;

public class GalleryManager : MonoBehaviour
{
    [SerializeField] private Animator wallAnimator;

    public void ActivateLever(){
        wallAnimator.enabled = true;
        wallAnimator.Play("MoveWall", 0, 0);
    }
}
