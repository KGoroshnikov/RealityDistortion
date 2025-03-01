using System.Collections.Generic;
using _Project.Scripts.Saves;
using UnityEngine;
using UnityEngine.AI;

public class ScreamGameManager : SaveableBehaviour
{
    [SerializeField] private MoveObjects moveObjects;
    [SerializeField] private Transform startPlayerPos;
    [SerializeField] private Transform player;
    [SerializeField] private PlayerContoller playerContoller;

    [SerializeField] private Transform[] posesPortal;
    [SerializeField] private Transform portalTransform;
    [SerializeField] private Portal portal;



    [SerializeField] private Animator screamAnimator;
    private List<Transform> portalsPlayerEntered = new List<Transform>();
    [SerializeField] private NavMeshAgent scream;
    [SerializeField] private float regularSpeed;
    [SerializeField] private float toPortalSpeed;
    private bool raceStarted;
    enum screamState{
        idle, running
    }
    private screamState state;

    private NavMeshPath path;

    public void StartGame(){
        playerContoller.FreezePlayer();
        moveObjects.AddObjectToMove(player.gameObject, startPlayerPos.position, startPlayerPos.rotation, 2, LauchGame);
    }

    void LauchGame(){
        playerContoller.UnfreezePlayer();
        playerContoller.SetRunningMode();
        SetupScream();

        portalTransform.position = posesPortal[1].position;
        portalTransform.rotation = posesPortal[1].rotation;
        portal.onTeleport.RemoveAllListeners();
        portal.onTeleport.AddListener(PlayerCompletedGame);
    }
    
    public void PlayerCompletedGame(){
        raceStarted = false;
        state = screamState.idle;
        screamAnimator.SetTrigger("ToIdle");
        scream.isStopped = true;
        scream.ResetPath();
    }

    void SetupScream(){
        scream.acceleration = 10000f;
        scream.angularSpeed = 10000f;
        scream.autoBraking = false;
        scream.stoppingDistance = 0f;
        raceStarted = true;
    }

    public void AddEnteredPortal(Transform _portal){
        if (!raceStarted) return;
        portalsPlayerEntered.Add(_portal);
    }

    void FixedUpdate()
    {
        if (!raceStarted) return;

        path = GetPath();
        if (path != null){
            if (state != screamState.running){
                state = screamState.running;
                screamAnimator.SetTrigger("ToRunning");
            }
            if (!scream.enabled)scream.enabled = true;
            if (scream.isStopped) scream.isStopped = false;
            scream.speed = regularSpeed;
            scream.SetPath(path);
        }
        else if (GetTargetPortal() != null)
        {
            if (state != screamState.running){
                state = screamState.running;
                screamAnimator.SetTrigger("ToRunning");
            }

            Transform targetPortal = GetTargetPortal();

            Vector3 portalExitPos = targetPortal.position + targetPortal.forward * 0.5f;

            if (!scream.enabled)
            {
                scream.enabled = true;
                scream.ResetPath();
            }
            scream.speed = toPortalSpeed;
            scream.SetDestination(portalExitPos);
        }

        else{
            if (state != screamState.idle){
                state = screamState.idle;
                screamAnimator.SetTrigger("ToIdle");
            }
            if (scream.enabled){
                scream.isStopped = true;
                scream.ResetPath();
                scream.enabled = false;
            }
            Debug.Log("IDK WHAT TO DO");
        }

    }

    Transform GetTargetPortal(){
        if (portalsPlayerEntered.Count == 0) return null;

        return portalsPlayerEntered[0];
    }

    public void ScreamEnteredPortal(Transform _portal){
        if (portalsPlayerEntered.Count == 0 || _portal != portalsPlayerEntered[0]){
            Debug.LogError("WRONG PORTAL: " + portalsPlayerEntered[0]);
        }
        else if (_portal == portalsPlayerEntered[0]){
            portalsPlayerEntered.RemoveAt(0);
        }
    }

    NavMeshPath GetPath(){
        NavMeshPath _path = new NavMeshPath();
        if (NavMesh.CalculatePath(scream.transform.position, player.position, NavMesh.AllAreas, _path))
        {
            bool isvalid = true;
            if (_path.status != NavMeshPathStatus.PathComplete) isvalid = false;
            if (isvalid)
            {
                return _path;
            }
            else
            {
                return null;
            }
        }
        return null;
    }

    private void Start() => Initialize();
    public override void ResetState(Dictionary<string, object> states)
    {
        throw new System.NotImplementedException();
    }
    public override void ApplyState(Dictionary<string, object> states)
    {
        throw new System.NotImplementedException();
    }
}
