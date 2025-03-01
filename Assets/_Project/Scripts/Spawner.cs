using UnityEngine;

[ExecuteInEditMode]
public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject prefab;

    private void Awake() => TryRespawn();

    public void TryRespawn()
    {
        if (transform.childCount != 0) return;
        Instantiate(prefab, transform);
    }
    
}