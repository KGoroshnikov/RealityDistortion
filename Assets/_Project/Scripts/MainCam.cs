using UnityEngine;
using UnityEngine.Rendering;

public class MainCam : MonoBehaviour
{
    Portal[] portals;

    void Awake () {
        portals = FindObjectsByType<Portal>(FindObjectsSortMode.None);
    }
    void OnEnable()
    {
        RenderPipelineManager.beginCameraRendering += CustomOnPostRender;
    }
    void CustomOnPostRender(ScriptableRenderContext context, Camera camera)
    {
        for (int i = 0; i < portals.Length; i++) {
            portals[i].Render ();
        }
    }

    void OnDisable()
    {
        RenderPipelineManager.beginCameraRendering -= CustomOnPostRender;
    }
}
