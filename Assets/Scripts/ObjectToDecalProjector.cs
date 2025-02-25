using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.Serialization;

public class ObjectToDecalProjector : MonoBehaviour
{
    private static readonly int BaseMap = Shader.PropertyToID("Base_Map");
    private static readonly int ObjectPosition = Shader.PropertyToID("_Object_Position");
    private static readonly int CameraPosition = Shader.PropertyToID("_Camera_Position");
    [FormerlySerializedAs("projector")] [SerializeField] private GameObject projectorObject;
    [SerializeField] private Material decalMaterial;
    [SerializeField] private int quality = 64;
    [SerializeField] private float maxDepth = 10;
    [SerializeField] private GameObject objectToRender;
    [SerializeField] private Camera renderCamera;
    
    private RenderTexture texture;


    
    private void Start()
    {
        var forward = (objectToRender.transform.position - renderCamera.transform.position).normalized;
        renderCamera.transform.forward = forward;
        texture = new RenderTexture(
            720, 720, 16, 
            RenderTextureFormat.ARGBFloat
        );
        texture.Create();
        
        var dst = Vector3.Distance(objectToRender.transform.position, renderCamera.transform.position);
        var material = new Material(decalMaterial);
        material.SetTexture(BaseMap, texture);
        material.SetVector(ObjectPosition, objectToRender.transform.position);
        material.SetVector(CameraPosition, renderCamera.transform.position);
        
        
        var projector = projectorObject.AddComponent<DecalProjector>();
        projector.scaleMode = DecalScaleMode.InheritFromHierarchy;
        projector.transform.forward = forward;
        projector.size = new Vector3(dst, dst, maxDepth);
        projector.pivot = new Vector3(0, 0, 0.5f * maxDepth);
        projector.material = material;
        
        // var step = maxDepth / quality;
        // for (var i = 1; i <= quality; i++)
        // {
        //     var projector = projectorObject.AddComponent<DecalProjector>();
        //     projector.scaleMode = DecalScaleMode.InheritFromHierarchy;
        //     projector.transform.forward = forward;
        //     var s = Mathf.Lerp(0.0f, dst, (float) i / quality);
        //     projector.size = new Vector3(s, s, step);
        //     projector.pivot = new Vector3(0, 0, (i + 0.5f) * step);
        //     projector.material = material;
        // }
        
        renderCamera.targetTexture = texture;
        renderCamera.Render();
        Invoke(nameof(CompleteDecalBuild), 0);
        
    }
    private void CompleteDecalBuild()
    {
        renderCamera.targetTexture = null;
        Destroy(renderCamera.gameObject);
        objectToRender.SetActive(false);
    }

    public void DecalToObject()
    {
        objectToRender.SetActive(true);
        projectorObject.SetActive(false);
    }

    public void ObjectToDecal()
    {
        objectToRender.SetActive(false);
        projectorObject.SetActive(true);
    }
}
