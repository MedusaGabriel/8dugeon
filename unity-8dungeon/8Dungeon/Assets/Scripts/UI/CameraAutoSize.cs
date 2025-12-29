using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraAutoSize : MonoBehaviour
{
    [SerializeField] private float designWidth = 8f;  // largura alvo em unidades de mundo
    [SerializeField] private float designHeight = 6f; // altura alvo em unidades de mundo

    private void Start()
    {
        Camera cam = GetComponent<Camera>();
        if (!cam.orthographic) return;

        float targetAspect = designWidth / designHeight;
        float windowAspect = (float)Screen.width / Screen.height;

        if (windowAspect >= targetAspect)
        {
            cam.orthographicSize = designHeight * 0.5f;
        }
        else
        {
            float adjustment = targetAspect / windowAspect;
            cam.orthographicSize = designHeight * 0.5f * adjustment;
        }
    }
}