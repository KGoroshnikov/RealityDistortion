using System.Collections.Generic;
using _Project.Scripts.Saves;
using UnityEngine;
using UnityEngine.AI;

public class ScreamGameManager : SaveableBehaviour, IFreezable
{
    [SerializeField] private MoveObjects moveObjects;
    [SerializeField] private Transform startPlayerPos;
    [SerializeField] private Transform player;
    [SerializeField] private PlayerContoller playerContoller;
    [SerializeField] private Inventory inventory;
    [SerializeField] private DeathManager deathManager;

    [SerializeField] private Transform[] posesPortal;
    [SerializeField] private Transform portalTransform;
    [SerializeField] private Portal portal;


    [SerializeField] private float killRadius;
    [SerializeField] private AudioSource screamAudio;
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

    private bool freezed;

    private NavMeshPath path;

    private bool lvlCompleted;
    private Vector3 screamOrigin;

    public void StartGame(){
        if (lvlCompleted) return;
        playerContoller.FreezePlayer(true);
        playerContoller.ResetCamRot();
        moveObjects.AddObjectToMove(player.gameObject, startPlayerPos.position, startPlayerPos.rotation, 2, LauchGame);
    }

    void LauchGame(){
        if (lvlCompleted) return;

        screamAudio.Play();

        playerContoller.SyncYaw(null);
        playerContoller.UnfreezePlayer(true);
        playerContoller.SetRunningMode();
        SetupScream();

        portalTransform.position = posesPortal[1].position;
        portalTransform.rotation = posesPortal[1].rotation;
        portal.onTeleport.RemoveAllListeners();
        portal.onTeleport.AddListener(PlayerCompletedGame);
    }
    
    public void PlayerCompletedGame(){
        if (lvlCompleted) return;
        lvlCompleted = true;
        SetState("ScreamGameManager_lvlCompleted");
        RemoveState("ScreamGameManager_StartGame");

        screamAudio.Stop();

        playerContoller.SetWalkMode();

        raceStarted = false;
        state = screamState.idle;
        screamAnimator.SetTrigger("ToIdle");
        scream.isStopped = true;
        scream.ResetPath();
        inventory.AddBucket();
        Commit();
    }

    void SetupScream(){
        scream.transform.position = screamOrigin;
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
        if (!raceStarted || freezed) return;

        if (Vector3.Distance(player.position, scream.transform.position) <= killRadius){
            raceStarted = false;
            deathManager.Die();
        }

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
            if (portalsPlayerEntered.Count != 0) portalsPlayerEntered.Clear();
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
                if (scream.isStopped) scream.isStopped = false;
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

    public void Freeze()
    {
        if (!raceStarted) return;

        freezed = true;
        scream.isStopped = true;
        scream.ResetPath();
        scream.enabled = false;
    }

    public void UnFreeze()
    {
        freezed = false;
    }

    private void Start()
    {
        Initialize();
        screamOrigin = scream.transform.position;
    }

    public override void ResetState(Dictionary<string, object> states)
    {
    }

    public override void ApplyState(Dictionary<string, object> states)
    {
        if (states.ContainsKey("ScreamGameManager_lvlCompleted"))
            lvlCompleted = true;
        if (states.ContainsKey("ScreamGameManager_StartGame"))
            StartGame();
    }

    public override void OnCommit() { }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(scream.transform.position, killRadius);
    }
}
