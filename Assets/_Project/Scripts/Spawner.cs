using System;
using System.Collections.Generic;
using _Project.Scripts.Saves;
using UnityEngine;

public class Spawner : SaveableBehaviour
{
    [SerializeField] private GameObject prefab;

    private void Awake() => TryRespawn();

    public void TryRespawn()
    {
        if (transform.childCount != 0) return;
        Instantiate(prefab, transform);
    }

    private void Start() => Initialize();

    public override void ResetState(Dictionary<string, object> states)
    {
        DestroyImmediate(transform.GetChild(0).gameObject);
        TryRespawn();
    }

    public override void ApplyState(Dictionary<string, object> states) { }
}