using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class SuperliminalDrag : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Camera camera;
    [SerializeField] private InputActionReference grab;
 
    [Header("Settings")]
    [SerializeField] private LayerMask targetMask;
    [SerializeField] private LayerMask dragMask;
    [SerializeField] private LayerMask ignoreTargetMask;
    [SerializeField, Min(1)] private float maxDistance = 100;
    [SerializeField, Min(0)] private float minDistance = 0.5f;
    [SerializeField, Min(1)] private int numberOfGridColumns = 16;
    [SerializeField, Min(1)] private int numberOfGridRows = 16;
    
 
    [Header("Views")]
    [SerializeField] private Transform target;
    
    private bool isKinematicBuffer;
    private float originalDistance;
    private Vector3 originalScale;
    private float targetScale;
    private Transform originalParent;

    private Vector3 left;
    private Vector3 right;
    private Vector3 top;
    private Vector3 bottom;
    private readonly List<Vector3> shapedGrid = new();


    [SerializeField] private GameObject grabUI;
    private Vector3[] box;

    private void OnDrawGizmos()
    {
        if (!target) return;
        
        Gizmos.color = Color.green;
        foreach (var pos in shapedGrid)
        {
            var point = camera.transform.TransformPoint(pos);
            point += camera.transform.forward * originalDistance;
            if (RaycastFast(camera.transform.position,
                    (point - camera.transform.position).normalized,
                    ignoreTargetMask | targetMask, out var hit))
                Gizmos.DrawSphere(hit.point, hit.distance * .01f);
        }

        Gizmos.matrix = camera.transform.localToWorldMatrix;
        Gizmos.color = Color.red;
        Gizmos.DrawLine(left, right);
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(top, bottom);


        // Gizmos.color = Color.yellow;
        // left = right = top = bottom = Vector2.zero;
        // GetRectConfines(GetBoundingBoxPoints());
        // var grid = SetupGrid();
        // foreach (var point in grid)
        //     Gizmos.DrawSphere(point, .025f);
    }
    

    private void FixedUpdate()
    {
        HandleInput();
        ResizeTarget();
    }
 
    private void HandleInput()
    {
        if (RaycastFast(camera.transform.position, camera.transform.forward, targetMask, out var hit0)){
            if (Physics.Linecast(camera.transform.position, hit0.point, ignoreTargetMask)) return;
            grabUI.SetActive(true);
        }
        else if (grabUI.activeSelf) grabUI.SetActive(false);

        if (grab.action.IsPressed())
        {
            if (target) return;
            if (!RaycastFast(camera.transform.position, 
                    camera.transform.forward, targetMask, out var hit)) return;
            if (Physics.Linecast(camera.transform.position, hit0.point, ignoreTargetMask)) return;
            
            target = hit.transform;
            if (target.TryGetComponent<Rigidbody>(out var rb))
            {
                isKinematicBuffer = rb.isKinematic;
                rb.isKinematic = true;
            }
            originalDistance = Vector3.Distance(camera.transform.position, target.position);
            originalParent = target.parent;
            target.parent = transform;
            originalScale = target.localScale;
            targetScale = 1;
            box = GetBoundingBoxPoints();
            SetupShapedGrid(box);
            target.gameObject.layer = (int) Mathf.Log(dragMask	, 2);
        }
        else
        {
            if (!target) return;
            if(target.TryGetComponent<Rigidbody>(out var rb)) 
                rb.isKinematic = isKinematicBuffer;
            target.parent = originalParent;
            target.gameObject.layer = (int) Mathf.Log(targetMask, 2);
            target = null;
        }
    }

    private void ResizeTarget()
    {
        if (!target) return;

        var dst = maxDistance;
        foreach (var point in shapedGrid
                     .Select(pos => camera.transform.TransformPoint(pos))
                     .Select(point => point + camera.transform.forward * originalDistance))
            if (RaycastFast(camera.transform.position,
                    (point - camera.transform.position).normalized,
                    ignoreTargetMask | targetMask, out var hit))
                dst = Mathf.Min(dst, hit.distance);
        

        dst -= dst / originalDistance;
        dst = Mathf.Max(dst, minDistance);
        targetScale = dst / originalDistance;
        target.position = camera.transform.position + camera.transform.forward * dst;
        target.localScale = targetScale * originalScale;
    }

    #region Calculating grid
    private Vector3[] GetBoundingBoxPoints() 
    {
        var size = target.GetComponent<Renderer>().localBounds.size;
        var x = new Vector3(size.x, 0, 0);
        var y = new Vector3(0, size.y, 0);
        var z = new Vector3(0, 0, size.z);
        var min = target.GetComponent<Renderer>().localBounds.min;
        Vector3[] bbPoints =
            {
            min,
            min + x,
            min + y,
            min + x + y,
            min + z,
            min + z + x,
            min + z + y,
            min + z + x + y
            };
        return bbPoints;
    }

    private void SetupShapedGrid(Vector3[] bbPoints) 
    {
        left = right = top = bottom = Vector2.zero;
        GetRectConfines(bbPoints);
        var grid = SetupGrid();
        GetShapedGrid(grid);
    }

    private void GetRectConfines(Vector3[] bbPoints)
    {
        var closestPoint = target.GetComponent<Renderer>().localBounds.ClosestPoint(camera.transform.position);
        var closestZ = camera.transform.InverseTransformPoint(target.TransformPoint(closestPoint)).z;
        if (closestZ <= 0) throw new Exception("HeldObject's inside the player!");

        for (var i = 0; i < bbPoints.Length; i++)
        {
            var bbPoint = target.TransformPoint(bbPoints[i]);
            Vector2 viewportPoint = camera.WorldToViewportPoint(bbPoint);
            var cameraPoint = camera.transform.InverseTransformPoint(bbPoint);
            cameraPoint.z = closestZ;

            if (viewportPoint.x < 0 || viewportPoint.x > 1
                || viewportPoint.y < 0 || viewportPoint.y > 1) continue;

            if (i == 0) left = right = top = bottom = cameraPoint;

            if (cameraPoint.x < left.x) left = cameraPoint;
            if (cameraPoint.x > right.x) right = cameraPoint;
            if (cameraPoint.y > top.y) top = cameraPoint;
            if (cameraPoint.y < bottom.y) bottom = cameraPoint;
        }
    }

    private Vector3[,] SetupGrid() 
    {
        var rectHrLength = right.x - left.x;
        var rectVertLength = top.y - bottom.y;
        Vector3 hrStep = new Vector2(rectHrLength / (numberOfGridColumns - 1), 0);
        Vector3 vertStep = new Vector2(0, rectVertLength / (numberOfGridRows - 1));

        var grid = new Vector3[numberOfGridRows, numberOfGridColumns];
        grid[0, 0] = new Vector3(left.x, bottom.y, left.z);

        for (var i = 0; i < grid.GetLength(0); i++)
        {
            for (var w = 0; w < grid.GetLength(1); w++)
            {
                if (i == 0 & w == 0) continue;
                if (w == 0)
                    grid[i, w] = Vector3.ProjectOnPlane(
                        grid[i - 1, 0] + vertStep, 
                        Vector3.forward	
                    );
                else 
                    grid[i, w] = Vector3.ProjectOnPlane(
                        grid[i, w - 1] + hrStep, 
                        Vector3.forward	
                    );
            }
        }
        return grid;
    }
    private void GetShapedGrid(Vector3[,] grid)
    {
        shapedGrid.Clear();
        foreach (var point in grid)
            if (RaycastFast(camera.transform.TransformPoint(point), camera.transform.forward, targetMask, out _))
                shapedGrid.Add(point);
    }
    #endregion

    private bool RaycastFast(Vector3 point, Vector3 dir, LayerMask layerMask, out RaycastHit hit) =>
        Physics.Raycast(point, dir, 
            out hit, maxDistance, layerMask);

}
