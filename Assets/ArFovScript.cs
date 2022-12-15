using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.XR.ARSubsystems;

public class ArFovScript : MonoBehaviour
{
    private Camera _thisCamera;
    private RenderTexture texture;
    [SerializeField] private float scaleX = 1, scaleY = 1;

    // Start is called before the first frame update
    void Start()
    {
        _thisCamera = this.GetComponent<Camera>();
        texture = new CustomRenderTexture(800, 480, RenderTextureFormat.Default);
        RenderTexture.active = texture;
    }

    void OnPreRender()
    {
        var scaledWidth = Screen.width * scaleX;
        var x = (Screen.width - scaledWidth) / 2;
        var scaledHeight = Screen.height * scaleY;
        var y = (Screen.height - scaledHeight) / 2;

        _thisCamera.rect = new Rect(x, y, scaledWidth, scaledHeight);
        _thisCamera.targetTexture = texture;
    }

    void OnEnable() {
        RenderPipelineManager.endCameraRendering += RenderPipelineManager_endCameraRendering;
    }
    void OnDisable() {
        RenderPipelineManager.endCameraRendering -= RenderPipelineManager_endCameraRendering;
    }
    private void RenderPipelineManager_endCameraRendering(ScriptableRenderContext context, Camera camera) {
        OnPostRender();
    }

    void OnPostRender()
    {
        _thisCamera.targetTexture = null;

        var scaledWidth = Screen.width * scaleX;
        var x = (Screen.width - scaledWidth) / 2;
        var scaledHeight = Screen.height * scaleY;
        var y = (Screen.height - scaledHeight) / 2;

        texture.width = (int)scaledWidth;
        texture.height = (int)scaledHeight;

        Graphics.DrawTexture(new Rect(x, y, scaledWidth, scaledHeight), texture);
    }
}
