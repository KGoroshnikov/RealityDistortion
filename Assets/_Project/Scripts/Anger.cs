using UnityEngine;
using UnityEngine.AI;

public class Anger : MonoBehaviour, IFreezable
{
    [SerializeField] private Transform player;

    [SerializeField] private FOV playerFOV;

    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private GameObject meshToSee;
    [SerializeField] private Renderer rendererMesh;

    private bool started;
    private bool freezed;

    public void ActivateMe(){
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
        }
        else
        {
            agent.isStopped = true;
            agent.ResetPath();
        }
    }
}
