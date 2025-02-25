using UnityEngine;
using UnityEngine.Rendering.Universal;

public class ObjectToDecalProjector : MonoBehaviour
{
    private static readonly int BaseMap = Shader.PropertyToID("Base_Map");
    [SerializeField] private DecalProjector projector;
    [SerializeField] private GameObject objectToRender;
    [SerializeField] private Camera renderCamera;
    
    private RenderTexture texture;

    private void Start()
    {
        var forward = (objectToRender.transform.position - renderCamera.transform.position).normalized;
        projector.transform.forward = forward;
        renderCamera.transform.forward = forward;

        var s = Vector3.Distance(objectToRender.transform.position, renderCamera.transform.position);
        projector.size = new Vector3(s, s, projector.size.z);
        texture = new RenderTexture(
            720, 720, 16, 
            RenderTextureFormat.ARGBFloat
        );
        texture.Create();
        projector.material = new Material(projector.material);
        projector.material.SetTexture(BaseMap, texture);
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
        projector.enabled = false;
    }

    public void ObjectToDecal()
    {
        objectToRender.SetActive(false);
        projector.enabled = true;
    }
}
