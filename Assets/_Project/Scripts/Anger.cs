using System;
using System.Collections.Generic;
using _Project.Scripts.Saves;
using UnityEngine;
using UnityEngine.AI;

public class Anger : SaveableBehaviour, IFreezable
{
    [SerializeField] private Transform player;

    [SerializeField] private FOV playerFOV;

    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private GameObject meshToSee;
    [SerializeField] private Renderer rendererMesh;

    [SerializeField] private DeathManager deathManager;
    [SerializeField] private float killRadius;

    private bool started;
    private bool freezed;

    public void ActivateMe(){
        agent.enabled = true;
        agent.acceleration = 10000f;
        agent.angularSpeed = 10000f;
        agent.autoBraking = false;
        agent.stoppingDistance = 0f;
        started = true;
    }

    public void DisableMe(){
        started = false;
        agent.isStopped = true;
        agent.ResetPath();
    }

    public void Freeze()
    {
        if (!started) return;
        freezed = true;
        agent.isStopped = true;
        agent.ResetPath();
    }

    public void UnFreeze()
    {
        freezed = false;
    }

    void Update()
    {
        if (!started || freezed) return;
        if (!playerFOV.isMeVisible(meshToSee)) //if (!CamFuncs.VisibleFromCamera(rendererMesh, Camera.main) && !playerFOV.isMeVisible(meshToSee))
        {
            if (agent.isStopped)
                agent.isStopped = false;
            agent.SetDestination(player.position);

            if (Vector3.Distance(transform.position, player.position) <= killRadius){
                started = false;
                deathManager.Die();
            }
        }
        else
        {
            agent.isStopped = true;
            agent.ResetPath();
        }
    }


    private void Start() => Initialize();
    public override void ResetState(Dictionary<string, object> states)
    {
        agent.enabled = false;
        agent.transform.position = (Vector3)states.GetValueOrDefault("Anger_Position", Vector3.zero);
        agent.transform.rotation = (Quaternion)states.GetValueOrDefault("Anger_Rotation", Quaternion.identity);
    }

    public override void ApplyState(Dictionary<string, object> states) { }

    public override void OnCommit()
    {
        SetState("Anger_Position", transform.position);
        SetState("Anger_Rotation", transform.rotation);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, killRadius);
    }
}
