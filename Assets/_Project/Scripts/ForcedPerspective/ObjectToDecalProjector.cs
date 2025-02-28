using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Serialization;

public class ObjectToDecalProjector : MonoBehaviour
{
    private static readonly int BaseMap = Shader.PropertyToID("Base_Map");
    private static readonly int CameraPosition = Shader.PropertyToID("_Camera_Position");
    private static readonly int CameraForward = Shader.PropertyToID("_Camera_Forward");
    private static readonly int CameraUp = Shader.PropertyToID("_Camera_Up");
    [FormerlySerializedAs("projector")] [SerializeField] private GameObject projectorObject;
    [SerializeField] private Material decalMaterial;
    [SerializeField] private int quality = 64;
    [SerializeField] private float maxDepth = 10;
    [SerializeField] private GameObject objectToRender;
    [SerializeField] private Camera renderCamera;
    [SerializeField] private bool debug;
    
    private RenderTexture texture;



    private void Start() => StartDecalBuild();
    private void CompleteDecalBuild()
    {
        renderCamera.targetTexture = null;
        renderCamera.gameObject.SetActive(false);
        objectToRender.SetActive(false);
    }
    public void StartDecalBuild()
    {
        renderCamera.gameObject.SetActive(true);
        var forward = (objectToRender.transform.position - renderCamera.transform.position).normalized;
        texture = new RenderTexture(
            720, 720, 16, 
            RenderTextureFormat.ARGBFloat
        );
        texture.Create();
        
        var dst = Vector3.Distance(objectToRender.transform.position, renderCamera.transform.position);
        var material = new Material(decalMaterial);
        material.SetTexture(BaseMap, texture);
        material.SetVector(CameraPosition, renderCamera.transform.position);
        
        
        var projector = projectorObject.AddComponent<DecalProjector>();
        projector.scaleMode = DecalScaleMode.InheritFromHierarchy;
        projector.transform.forward = forward;
        projector.size = new Vector3(dst, dst, maxDepth);
        projector.pivot = new Vector3(0, 0, 0.5f * maxDepth);
        projector.material = material;
        renderCamera.transform.forward = forward;
        material.SetVector(CameraUp, renderCamera.transform.up);
        material.SetVector(CameraForward, renderCamera.transform.forward);
        
        renderCamera.targetTexture = texture;
        renderCamera.Render();
        if (debug) return;
        Invoke(nameof(CompleteDecalBuild), 0);
    }
    public void DecalToObject()
    {
        objectToRender.SetActive(true);
        projectorObject.SetActive(false);
    }
}
