using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ScreamGameManager : MonoBehaviour
{
    [SerializeField] private MoveObjects moveObjects;
    [SerializeField] private Transform startPlayerPos;
    [SerializeField] private Transform player;
    [SerializeField] private PlayerContoller playerContoller;



    private List<Transform> portalsPlayerEntered = new List<Transform>();
    [SerializeField] private NavMeshAgent scream;
    private Transform lastEnteredPortal;
    private bool raceStarted;

    public void StartGame(){
        playerContoller.FreezePlayer();
        moveObjects.AddObjectToMove(player.gameObject, startPlayerPos.position, startPlayerPos.rotation, 2, LauchGame);
    }

    void LauchGame(){
        playerContoller.UnfreezePlayer();
        playerContoller.SetRunningMode();
        SetupScream();
    }

    void SetupScream(){
        scream.acceleration = 10000f;
        scream.angularSpeed = 10000f;
        scream.autoBraking = false;
        scream.stoppingDistance = 0f;
        raceStarted = true;
    }

    public void AddEnteredPortal(Transform _portal){
        portalsPlayerEntered.Add(_portal);
    }

    void FixedUpdate()
    {
        if (GetPath() != null){
            
        }

    }

    void GetPortal(){

    }

    NavMeshPath GetPath(){
        NavMeshPath path = new NavMeshPath();
        if (NavMesh.CalculatePath(transform.position, player.position, NavMesh.AllAreas, path))
        {
            bool isvalid = true;
            if (path.status != NavMeshPathStatus.PathComplete) isvalid = false;
            if (isvalid)
            {
                return path;
            }
            else
            {
                return null;
            }
        }
        return null;
    }
}
