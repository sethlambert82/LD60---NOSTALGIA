using UnityEngine;
using UnityEngine.UI;

public class RenderTextureScaler : MonoBehaviour
{
    [SerializeField] Canvas canvas;
    Vector2 canvasRenderingRect;
    CanvasScaler scaler;
    [SerializeField] RenderTexture rt;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        scaler = canvas.GetComponent<CanvasScaler>();
        canvasRenderingRect = new Vector2(rt.width,rt.height);

    }

    // Update is called once per frame
    void Update()
    {
        if(canvas.pixelRect.size!=canvasRenderingRect)
        {
            rt.Release();
            rt.width = (int)canvas.pixelRect.width;
            rt.height = (int)canvas.pixelRect.height;
            rt.Create();

            Debug.Log($"Resizing RT: {rt.width}x{rt.height}");
            canvasRenderingRect = canvas.pixelRect.size;
            rt = new RenderTexture((int)canvas.pixelRect.width, (int)canvas.pixelRect.height, 1);
        }
        //Debug.Log(canvas.pixelRect.size);
        //if(rt.height!=canvas.pixelRect.height) rt.height = (int)canvas.pixelRect.height;
        //if(rt.width!=canvas.pixelRect.width) rt.width = (int)canvas.pixelRect.width;
    }

}
