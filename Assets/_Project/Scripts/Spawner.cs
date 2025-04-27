
using System.Collections.Generic;
using _Project.Scripts.Saves;
using UnityEngine;
using UnityEngine.VFX;

public class Spawner : SaveableBehaviour
{
    [SerializeField] private GameObject prefab;

    private void Awake() => TryRespawn();

    public void TryRespawn()
    {
        if (transform.childCount != 0) return;
        Instantiate(prefab, transform);
    }

    public void ForceRespawn()
    {
        DestroyImmediate(transform.GetChild(0).gameObject);
        TryRespawn();
    }

    private void Start() => Initialize();

    public override void ResetState(Dictionary<string, object> states) => ForceRespawn();

    public override void ApplyState(Dictionary<string, object> states) { }
    public override void OnCommit() { }
}