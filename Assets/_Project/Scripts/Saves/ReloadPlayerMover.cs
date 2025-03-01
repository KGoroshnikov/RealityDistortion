using System;
using System.Collections.Generic;
using _Project.Scripts.Saves;
using UnityEngine;

public class ReloadPlayerMover : SaveableBehaviour
{
    [SerializeField] private string stateName;
    [SerializeField] private PlayerContoller player;

    private void Start() => Initialize();

    public override void ResetState(Dictionary<string, object> states) { }

    public override void ApplyState(Dictionary<string, object> states)
    {
        if (!states.ContainsKey(stateName)) return;
        player.FreezePlayer(true);
        Invoke(nameof(Teleport),0);
    }

    private void Teleport()
    {
        player.transform.position = transform.position;
        player.UnfreezePlayer(true);
    }
}