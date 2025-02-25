using UnityEngine;
using UnityEngine.Rendering;

public class CameraRender : MonoBehaviour
{
    public void SendRenderRequests(RenderTexture texture2D)
    {
        Camera cam = GetComponent<Camera>();
        RenderPipeline.StandardRequest request = new RenderPipeline.StandardRequest();
        if (RenderPipeline.SupportsRenderRequest(cam, request))
        {
            request.destination = texture2D;
            RenderPipeline.SubmitRenderRequest(cam, request);
        }
    }
}
