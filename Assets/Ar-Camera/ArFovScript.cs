using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.XR.ARSubsystems;

public class ArFovScript : MonoBehaviour
{
    private Camera _thisCamera;

    [SerializeField]
    private Texture texture;
    [SerializeField] 
    private Material material;
    [SerializeField]
     private float scaleX = 1, scaleY = 1;

    // Start is called before the first frame update
    void Start()
    {
        _thisCamera = GetComponent<Camera>();
        texture =  texture ?? new RenderTexture(1000, 1000, 28);
        //RenderTexture.active = texture;
    }

    void OnEnable() {
        RenderPipelineManager.endCameraRendering += RenderPipelineManager_endCameraRendering;
        RenderPipelineManager.beginCameraRendering += RenderPipelineManager_beginCameraRendering;
    }
    void OnDisable()
    {
        RenderPipelineManager.endCameraRendering -= RenderPipelineManager_endCameraRendering;
        RenderPipelineManager.beginCameraRendering -= RenderPipelineManager_beginCameraRendering;
    }

    private void RenderPipelineManager_beginCameraRendering(ScriptableRenderContext arg1, Camera arg2)
    {
        OnPreRender();
    }
    
    private void RenderPipelineManager_endCameraRendering(ScriptableRenderContext context, Camera camera) {
        //_thisCamera.targetTexture = null;
        //_thisCamera.AddCommandBuffer(CameraEvent.BeforeFinalPass, )

        //var scaledWidth = Screen.width * scaleX;
        //var x = (Screen.width - scaledWidth) / 2;
        //var scaledHeight = Screen.height * scaleY;
        //var y = (Screen.height - scaledHeight) / 2;
        //var rect = new Rect(x, y, scaledWidth, scaledHeight);

        var rect = new Rect(0, 0, Screen.width, Screen.height);

        Graphics.DrawTexture(rect, texture, material);
    }

    void OnPreRender()
    {
        //_thisCamera.targetTexture = texture;
    }

   
}
