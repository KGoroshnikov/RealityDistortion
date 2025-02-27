using UnityEngine;
using UnityEngine.AI;

public class Anger : MonoBehaviour
{
    [SerializeField] private Transform player;

    [SerializeField] private FOV playerFOV;

    [SerializeField] private NavMeshAgent agent;

    void Start()
    {
        agent.acceleration = 10000f;
        agent.angularSpeed = 10000f;
        agent.autoBraking = false;
        agent.stoppingDistance = 0f;
    }

    void Update()
    {
        if (!playerFOV.isMeVisible(gameObject))
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
